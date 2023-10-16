using BehaviorTree;
using UnityEngine;

internal class RikaBeforeAttack : Node
{
    private RikayonBT rikayonBT;
    public RikaBeforeAttack(RikayonBT rikayonBT)
    {
        this.rikayonBT = rikayonBT;
    }

    public override NodeState Evaluate()
    {
        World CurWorld = World.GameWorld.GetComponent<World>();
        if (CurWorld.player == null) return NodeState.FAILURE;
        if (Vector3.Distance(rikayonBT.transform.position, CurWorld.player.transform.position) > rikayonBT.GetComponent<Rikayon>().AttackRange) return NodeState.FAILURE;
        if (rikayonBT.GetComponent<Rikayon>().Attackcooldonwn > 0) return NodeState.FAILURE;

        Quaternion lookat = Quaternion.LookRotation(CurWorld.player.transform.position - rikayonBT.transform.position, Vector3.up);
        if (Quaternion.Angle(rikayonBT.transform.rotation, lookat) > 5)
        {
            rikayonBT.transform.rotation = Quaternion.Lerp(rikayonBT.transform.rotation, lookat, 0.2f);
            return NodeState.RUNNING;
        }
        else
        {
            return NodeState.SUCCESS;
        }
    }
}