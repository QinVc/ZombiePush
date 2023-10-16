using BehaviorTree;
using UnityEngine;

internal class DetectTatget : Node
{
    BehaviorTree.Tree rikayon;
   public DetectTatget(BehaviorTree.Tree rikayonBT)
    {
       rikayon = rikayonBT;
    }
    public override NodeState Evaluate()
    {
        /*        Debug.Log("找人");*/
        World CurWorld = World.GameWorld.GetComponent<World>();
        float Distance = Vector3.Magnitude(this.rikayon.transform.position - CurWorld.player.position);
        if (Distance < rikayon.GetComponent<Rikayon>().DetectRange) 
        {
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
        
    }
}