using System.Collections;
using System.Timers;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class DelyDestoryObj 
{
    public GameObject obj;
    public float life;

    public DelyDestoryObj(GameObject Object, float time) 
    {
        obj = Object;
        life = time;
    }
}
public class ObjectPoolManager : MonoBehaviour
{
    public Dictionary<GameObject, ObjectPool> Prefab2PoolLinks = new Dictionary<GameObject, ObjectPool>();
    public Dictionary<GameObject, GameObject> Object2PrefabLinks = new Dictionary<GameObject, GameObject>();
    public List<DelyDestoryObj> DelyDestoryList = new List<DelyDestoryObj>();
    public GameObject TestPrefab;
    public GameObject Spawn(GameObject Prefab, Vector3 Pos, Quaternion Rot,int CreateCapacity=100)
    {
        GameObject GenObject;
        if (Prefab2PoolLinks.ContainsKey(Prefab))
        {
            ObjectPool objectPool = Prefab2PoolLinks[Prefab];
            if (objectPool.Cache.Count > 0)
            {
                GenObject = objectPool.Cache[0];
                GenObject.transform.position = Pos;
                GenObject.transform.rotation = Rot;
                GenObject.SetActive(true);
                objectPool.Cache.RemoveAt(0);
            }
            else
            {
                GenObject = Instantiate(Prefab, Pos, Rot);
                Object2PrefabLinks.Add(GenObject, Prefab);
            }
        }
        else 
        {
            ObjectPool NewObject = new ObjectPool(CreateCapacity);
            GenObject = Instantiate(Prefab, Pos, Rot);

            //Create New Link
            Object2PrefabLinks.Add(GenObject, Prefab);
            Prefab2PoolLinks.Add(Prefab, NewObject);
        }
        return GenObject;
    }


    public void DeSpawn(GameObject gameObject)
    {
        if (Object2PrefabLinks.ContainsKey(gameObject))
        {
            var Prefab = Object2PrefabLinks[gameObject];
            ObjectPool objectPool = Prefab2PoolLinks[Prefab];
            if (objectPool.Cache.Count < objectPool.Capacity) 
            {
                objectPool.Cache.Add(gameObject);
                gameObject.SetActive(false);
            }
            else 
            {
                Debug.Log("Pool capacity overflow!");
                Destroy(gameObject);
            }
        }
        else
        {
            Debug.Log("You can't Despawn a gameobject not spawn with objectpool!");
            Destroy(gameObject);
        }
    }

    public void DelayDeSpawn(GameObject gameObject,float time)
    {
        if (Object2PrefabLinks.ContainsKey(gameObject))
        {
            DelyDestoryList.Add(new DelyDestoryObj(gameObject,time));
        }
        else
        {
            Debug.Log("You can't Despawn a gameobject not spawn with objectpool");
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        for(int i = 0; i < DelyDestoryList.Count; i++) 
        {
            DelyDestoryList[i].life -= Time.deltaTime;
            if (DelyDestoryList[i].life <= 0) 
            {
                DeSpawn(DelyDestoryList[i].obj);
                if (DelyDestoryList[i].obj!=null) 
                {
                    DelyDestoryList[i].obj.transform.position = new Vector3(120, 0, -280);
                }
                DelyDestoryList.RemoveAt(i);
            }
        }
    }

    private void OnDestroy()
    {
        Prefab2PoolLinks.Clear();
        Object2PrefabLinks.Clear();
    }
}
