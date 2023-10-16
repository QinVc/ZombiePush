using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
 
    public abstract class Tree : MonoBehaviour
    {
        private Node _root = null;

        protected void Start()
        {
            _root = SetupTree();
        }

        protected void Update()
        {
            if (_root != null)
            {
                _root.Evaluate();
             /*   Debug.Log("ÐÐÎªÊ÷:Ñ­»·");*/
            }
                
        }

        protected abstract Node SetupTree();

    }

}
