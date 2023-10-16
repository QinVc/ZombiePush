using BehaviorTree;
using UnityEngine;

internal class RikaAfterAttack : Node
{
    private RikayonBT rikayonBT;

    public RikaAfterAttack(RikayonBT rikayonBT)
    {
        this.rikayonBT = rikayonBT;
    }

    //攻击后摇
    public override NodeState Evaluate()
    {
        return NodeState.SUCCESS;     
   
    }
}