using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigAfterAttack : Node
{
    Transform parentTran;
    float attackcool = 0.5f;
    float curattackcool = 0.5f;
    public PigAfterAttack(Transform parent)
    {
        parentTran = parent;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("行为树:攻击后休息");

        parentTran.GetComponent<Animator>().CrossFade("Idle", 0.1f);
        if (curattackcool <= 0)
        {
            curattackcool = attackcool;
            state = NodeState.SUCCESS;
            return state;
        }
        else
        {
            curattackcool -= Time.deltaTime;
            state = NodeState.RUNNING;
            return state;

        }
        
        
    }
}
