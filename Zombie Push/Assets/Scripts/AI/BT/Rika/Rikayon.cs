using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tree = BehaviorTree.Tree;


public class Rikayon : Actor {

    public Animator animator;
    ActorDamge DamgeEvent;
    public float Attackcooldonwn = 2f;
    public float MaxAttackCooldown = 2.0f;
    public float AttackRange = 10f;
    public Transform AttackPoint;

    // Use this for initialization
    void Start () {
		animator=GetComponent<Animator>();
        DamgeEvent.from = this;
    }
	
	// Update is called once per frame
	void Update () 
	{
        if (Attackcooldonwn>=0)
        {
            Attackcooldonwn -= Time.deltaTime;
        }
	}

    public void OnAttack() 
    {
        CurState = AIState.Attack;
    }

    public void CreateDamge(int value) 
    {
        Collider[] colliders = Physics.OverlapBox(AttackPoint.position, new Vector3(1,1,5), Quaternion.identity, LayerMask.GetMask("Player"));
        for (int i = 0; i < colliders.Length; i++)
        {
            Actor Victim = colliders[i].GetComponent<Actor>();
            DamgeEvent.to = Victim;
            Victim.OnDamge(DamgeEvent);
        }
    }

    public void AfterAttack() 
    {
        CurState = AIState.None;
        Attackcooldonwn = MaxAttackCooldown;

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
            animator.SetTrigger("Hit");
        }
    }
}
