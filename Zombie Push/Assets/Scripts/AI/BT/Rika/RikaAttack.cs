using BehaviorTree;
using UnityEngine;
using UnityEngine.UIElements;

internal class RikaAttack : Node
{
    Transform m_Transform;
    Animator m_Animator;
    private RikayonBT rikayonBT;

    public RikaAttack(RikayonBT rikayonBT)
    {
        this.rikayonBT = rikayonBT;
        m_Animator=rikayonBT.GetComponent<Animator>();
    }

    public override NodeState Evaluate()
    {
        World CurWorld = World.GameWorld.GetComponent<World>();
        //太近了，底下攻击
        if (Vector3.Distance(rikayonBT.transform.position, CurWorld.player.transform.position) < rikayonBT.GetComponent<Rikayon>().AttackRange)
        {
            m_Animator.SetTrigger("Attack_1");

            if (rikayonBT.GetComponent<Actor>().CurState == AIState.Attack) return NodeState.RUNNING;
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}