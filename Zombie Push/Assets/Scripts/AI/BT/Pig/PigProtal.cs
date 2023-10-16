using BehaviorTree;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PigProtal : Node
{
    Transform parentTran;
    Animator animator;
    Vector3[] waypoints;
    int index = 0;

    public PigProtal(Transform parent, Vector3[] waypoints)
    {
        this.parentTran = parent;
        this.waypoints = waypoints;
        animator=parent.GetComponent<Animator>();
        

    }
    public float xzDistance(Vector3 a, Vector3 b) {
        return (float)Math.Sqrt(Math.Pow(b.x - a.x, 2) + Math.Pow(b.z - a.z, 2));
    }

    public override NodeState Evaluate()
    {
       
       
        if (xzDistance(parentTran.position, waypoints[index])< 0.1f|| parentTran.GetComponent<PigBT>().OnColWall)
        {
            index =(index+1)%waypoints.Length;
            parentTran.GetComponent<PigBT>().OnColWall = false;
        }
        else
        {
            animator.CrossFade("Move", 0.1f);
            Debug.Log("ÐÐÎªÊ÷:Ñ²Âß");
            Quaternion lookat = Quaternion.LookRotation(waypoints[index] -new Vector3(parentTran.position.x, 0, parentTran.position.z), new Vector3(0,1,0));
            parentTran.rotation = Quaternion.Lerp(parentTran.rotation,lookat,2f) ;
            parentTran.position = Vector3.MoveTowards(parentTran.position, waypoints[index], 1f*Time.deltaTime);

        }
        state = NodeState.RUNNING;
        return state;
    }
}
