using Lean;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    public int Capacity;
    public List<GameObject> Cache;

    public ObjectPool(int capacity)
    {
        Capacity = capacity;
        Cache = new List<GameObject>();
    }
}
