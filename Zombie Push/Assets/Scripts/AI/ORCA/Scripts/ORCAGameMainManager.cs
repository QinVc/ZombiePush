using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Lean;
using RVO;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Assertions.Comparers;
using Vector2 = RVO.Vector2;

public class ORCAGameMainManager : SingletonBehaviour<ORCAGameMainManager>
{
    public GameObject agentPrefab;
    private Plane m_hPlane = new Plane(Vector3.up, Vector3.zero);
    private Dictionary<int, GameAgent> m_agentMap = new Dictionary<int, GameAgent>();

    // Use this for initialization
    void Start()
    {
        Simulator.Instance.setTimeStep(0.025f);
        // setAgentDefaults(float neighborDist, int maxNeighbors, float timeHorizon, float timeHorizonObst, float radius, float maxSpeed, Vector2 velocity)
        Simulator.Instance.setAgentDefaults(5.0f, 50, 3.0f, 3.0f, 1f, 3.0f, new Vector2(0.0f, 0.0f));

        // add in awake
        Simulator.Instance.processObstacles();
    }

    public void GenEnemy(int min,int max, Transform[] points)
    {
        for(int i = 0; i < points.Length; i++)
        {
            int EachPointCount= UnityEngine.Random.Range(min,max);
            for (int j=0;j<EachPointCount;j++)
                CreatAgent(points[i].position+new Vector3(UnityEngine.Random.Range(-10,10),0, UnityEngine.Random.Range(-10, 10)));
        }
    }

    public void DeleteAgent(int agentNo)
    {
        float rangeSq = float.MaxValue;
        if (agentNo == -1 || !m_agentMap.ContainsKey(agentNo))
            return;

        Simulator.Instance.delAgent(agentNo);
        /*        LeanPool.Despawn(m_agentMap[agentNo].gameObject,2f);*/
        GetComponent<ObjectPoolManager>().DelayDeSpawn(m_agentMap[agentNo].gameObject, 3f);
        m_agentMap.Remove(agentNo);
    }

    void CreatAgent(Vector3 pos)
    {
        int sid = Simulator.Instance.addAgent(new Vector2(pos.x,pos.z));
        if (sid >= 0)
        {
            /*            GameObject go = LeanPool.Spawn(agentPrefab, new Vector3(pos.x, 0, pos.z), Quaternion.identity);*/
            GameObject go = GetComponent<ObjectPoolManager>().Spawn(agentPrefab, new Vector3(pos.x, 0, pos.z), Quaternion.EulerAngles(new Vector3(0f,UnityEngine.Random.Range(0,360),0)));
            GameAgent ga = go.GetComponent<GameAgent>();
            Assert.IsNotNull(ga);
            ga.sid = sid;
            m_agentMap.Add(sid, ga);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        Simulator.Instance.doStep();
    }
}