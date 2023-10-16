using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

internal class MoveToTarget : Node
{
    private BehaviorTree.Tree rikayonBT;
    Animator animator;
    NavMeshAgent agent;

    public MoveToTarget(BehaviorTree.Tree rikayonBT)
    {
        this.rikayonBT = rikayonBT;
        agent=rikayonBT.GetComponent<NavMeshAgent>();
        agent.stoppingDistance = rikayonBT.GetComponent<Rikayon>().AttackRange;
        animator =rikayonBT.GetComponent<Animator>();
    }


    bool isAttack()
    {
        if (rikayonBT.GetComponent<Rikayon>().CurState == AIState.Attack) 
        {
            return true;
        }
        return false;
    }
    public override NodeState Evaluate()
    {
        if (isAttack()) return NodeState.FAILURE;
       
        World CurWorld = World.GameWorld.GetComponent<World>();
        float Distance = Vector3.Magnitude(rikayonBT.transform.position - CurWorld.player.position);

        Quaternion lookat = Quaternion.LookRotation(CurWorld.player.transform.position - rikayonBT.transform.position, Vector3.up);
        rikayonBT.transform.rotation = Quaternion.Lerp(rikayonBT.transform.rotation, lookat, 0.2f);

        agent.SetDestination(CurWorld.player.position);
        animator.SetFloat("Speed", Vector3.Magnitude(agent.velocity));

        if (Distance < rikayonBT.GetComponent<Rikayon>().AttackRange)
        {
            return NodeState.SUCCESS;
        }
        else 
        {
            return NodeState.RUNNING;
        }
    }
}