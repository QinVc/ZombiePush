using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using Tree = BehaviorTree.Tree;

public class RikayonBT : Tree
{
    protected override BehaviorTree.Node SetupTree()
    {
        
        Sequence AttackSeq = new Sequence(new List<Node>()
        {
            new RikaBeforeAttack(this),
            new RikaAttack(this),
            new RikaAfterAttack(this)

        }) ;
        
        Sequence Move = new Sequence(new List<Node>(){
            new DetectTatget(this),
            new MoveToTarget(this)
        });
        Selector root=new Selector(new List<Node>()
        {
            AttackSeq,
            Move,
            new RikaProtal()
        });

        return root;

    }

    private void Update()
    {
        base.Update();
    }
}
