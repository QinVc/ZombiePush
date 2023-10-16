using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Tree = BehaviorTree.Tree;
using JetBrains.Annotations;

public class PigBT : Tree
{
    public Vector3[] wayPoints = new Vector3[3];
    public bool OnColWall=false;
    private void Awake()
    {
        for (int i = 0; i < wayPoints.Length; i++)
        {
            wayPoints[i] = new Vector3(transform.position.x + Random.Range(-3, 3),0, transform.position.z + Random.Range(-3, 3));
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("×²ÁËÂï");
        if(collision.collider.tag == "Ostacle")
        {
            OnColWall = true;
        };
    }
    protected override Node SetupTree()
    {

        Sequence Attack = new Sequence(new List<Node> { new PigAttackInRange(transform), new PigAfterAttack(transform) }) ;
        Sequence GoToEnemy=new Sequence(new List<Node> {new PigDetect(transform),new PigGoToEnemy(transform) });
        Node root = new Selector(new List<Node>() { 
            Attack,
            GoToEnemy,
            new PigProtal(transform,wayPoints),
        }) ;
        return root;
    }
}
