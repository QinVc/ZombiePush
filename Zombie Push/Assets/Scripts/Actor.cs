using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ActorDamge
{
    public Actor from;
    public Actor to;
    public int value;
}
public class Actor : MonoBehaviour
{
    public int HP;
    public AIState CurState;
    public virtual void OnDamge(ActorDamge inDamge) { }
    public virtual void OnDead() { }
}
