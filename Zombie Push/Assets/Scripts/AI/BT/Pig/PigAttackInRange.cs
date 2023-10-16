using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigAttackInRange : Node
{
    Transform parentTran;
    Animator animator;
    float attRange = 2f;
    float attackcool = 1f;
    float curattackcool = 1f;
    public PigAttackInRange(Transform parent)
    {
        parentTran = parent;
        animator=parentTran.GetComponent<Animator>();
    }
    public override NodeState Evaluate()
    {
       ;
        Transform target = (Transform)GetData("target");
        if (target != null)
        {

            if (Vector3.Distance(parentTran.position, target.position) <= attRange)
            {
                if (curattackcool >= 0f)
                {
                   
                    curattackcool -= Time.deltaTime;
                    state = NodeState.FAILURE;
                    return state;
                }
                else
                {
                    Debug.Log("ÐÐÎªÊ÷: £¡¹¥»÷");
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
                        animator.CrossFade("Attack", 0.1f);
                    curattackcool = attackcool;
                    state = NodeState.SUCCESS;
                    return state;
                }

            }
            else
            {
                Debug.Log("ÐÐÎªÊ÷: ³¬³ö¹¥»÷¾àÀë");
                state = NodeState.FAILURE;
                return state;
            }
        }
        else
        {
            state = NodeState.FAILURE;
            return state;
        }
    }
}
