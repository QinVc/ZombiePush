using BehaviorTree;
using UnityEngine;

internal class GetDamge : Node
{
    /// <summary>
    /// 仅指收到硬直而已，没有其他伤害逻辑
    /// </summary>
    Transform m_Transform;
    bool isDamge;
    float suffVal;
    public GetDamge(Transform transform)
    {
        m_Transform = transform;

    }
    public override NodeState Evaluate()
    {
        if(m_Transform.GetComponent<Actor>().CurState==AIState.Damge)
            return NodeState.SUCCESS;
        else return NodeState.FAILURE;
    }
}