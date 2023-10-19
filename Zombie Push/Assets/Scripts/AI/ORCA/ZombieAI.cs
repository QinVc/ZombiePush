using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIState
{
   None,
   Attack,
   Walking,
   Damge,
   Dead
}

public class ZombieAI : Actor
{
    public float DetectDistance;
    public float AttackDistance;

    Animator animator;
    Vector3 PlayerPos;
    GameAgent ORCAagent;
    ActorDamge MyDamge;
    // Start is called before the first frame update

    private void Awake()
    {
        ORCAagent = GetComponent<GameAgent>();
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        MyDamge.from = this;

    }

    private void OnEnable()
    {
        HP = 100;
        if(ORCAagent.sid != -1)
        ORCAagent.OnReborn(transform.position);
    }
    private void Update()
    {
        DetectPlayer();
    }

    public void DetectPlayer() 
    {
        World CurWorld = World.GameWorld.GetComponent<World>();
        float Distance = Vector3.Magnitude(this.transform.position - CurWorld.player.position);

        PlayerPos = CurWorld.player.position;

        if (Distance < AttackDistance)
        {
            if (CurState == AIState.Walking) WalkToAttack();
            CurState = AIState.Attack;
            animator.SetTrigger("Attack");
        }
        else if (Distance < DetectDistance)
        {
            if(CurState==AIState.Attack) AttackToWalk();
            CurState = AIState.Walking;
            animator.SetBool("ChasePlayer", true);
        }
        else 
        {
            AnyStateToIdle();
            CurState =AIState.None;
        }
    }

    public void AttackToWalk() 
    {
        animator.ResetTrigger("Attack");
    }
    public void WalkToAttack()
    {
        animator.SetBool("ChasePlayer", false);
    }

    public void AnyStateToIdle() 
    {
        animator.SetBool("ChasePlayer", false);
    }

    public void AnyStateToHit()
    {
        ORCAagent.bSkipTick = 60;
    }

    public void AnyStateToAttack()
    {
        ORCAagent.bSkipTick = 150;
    }

    public override void OnDamge(ActorDamge inDamge)
    {
        base.OnDamge(inDamge);
        HP -= inDamge.value;
        if (HP <= 0)
        {
            OnDead();
        }
        else 
        {
            if (CurState == AIState.None) 
            {
                DetectDistance = 100;
                Collider[] Neiburs = Physics.OverlapBox(transform.position, new Vector3(10, 10, 10), Quaternion.identity);
                for (int i = 0; i < Neiburs.Length; i++)
                {
                    Neiburs[i].transform.root.GetComponent<ZombieAI>().DetectDistance = 100;
                }
            }
            animator.SetTrigger("Hit");
            AnyStateToHit();
        }
    }

    public void OnAttack()
    {
        AnyStateToAttack();
        RaycastHit hitInfo;
        Vector3 Origin =new Vector3(this.transform.position.x,0.5f, this.transform.position.z);
        if (Physics.Raycast(Origin, this.transform.forward,out hitInfo,3f,LayerMask.GetMask("Player"))) 
        {
            MyDamge.to = hitInfo.transform.gameObject.GetComponent<CPCharacterController>();
            MyDamge.value = 2;
            MyDamge.to.OnDamge(MyDamge);
        }
    }

    public override void OnDead()
    {
        base.OnDead();
        animator.SetTrigger("Dead");
        CurState= AIState.Dead;
        ORCAagent.BeforeDestory();
    }
}
