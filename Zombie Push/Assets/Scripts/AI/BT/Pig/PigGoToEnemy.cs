using BehaviorTree;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PigGoToEnemy : Node
{
    Transform parentTran;
    Transform target;
    Animator animator;
    public PigGoToEnemy(Transform parent)
    {
        this.parentTran = parent;
        animator = parentTran.GetComponent<Animator>();
    }
    public override NodeState Evaluate()
    {

        target = (Transform)GetData("target");

        if(Vector3.Distance(parentTran.position, target.position) > 3.5f)
        {
            Debug.Log("��Ϊ��:̫Զ�ˣ�����");
            ClearData("target");
            state = NodeState.FAILURE;
            return state;
        }
        if (Vector3.Distance(parentTran.position,target.position)>2f)
        {
            Debug.Log("��Ϊ��:�ƶ�������");
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Move"))
                animator.CrossFade("Move", 0f);
            parentTran.position = Vector3.Lerp(parentTran.position, target.position, 1f * Time.deltaTime);
            Quaternion lookat = Quaternion.LookRotation(new Vector3(target.position.x, 0, target.position.z) - new Vector3(parentTran.position.x, 0, parentTran.position.z), new Vector3(0, 1, 0));
            parentTran.rotation = Quaternion.Lerp(parentTran.rotation, lookat, 2f);
            state = NodeState.RUNNING;
            return state;
        }
        else
        {
            Debug.Log("��Ϊ��:�ɹ���������");
            state = NodeState.SUCCESS;
            return state;
        }
       
    }
}
