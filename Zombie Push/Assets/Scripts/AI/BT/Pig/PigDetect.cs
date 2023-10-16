using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class PigDetect : Node
{
    Transform parentTran;
    public PigDetect(Transform parent)
    {
        this.parentTran = parent;
    }

    public override NodeState Evaluate()
    {
        if (GetData("target") != null)
        {
            state = NodeState.SUCCESS;
            return state;
        }
        Collider[] colliders = Physics.OverlapSphere(parentTran.position, 2f, LayerMask.GetMask("Player")) ;
        if (colliders.Length > 0)
        {
            parent.parent.SetData("target", colliders[0].transform);
            state = NodeState.SUCCESS;
            return state;
        }

        state = NodeState.FAILURE;
        return state;
    }
}
