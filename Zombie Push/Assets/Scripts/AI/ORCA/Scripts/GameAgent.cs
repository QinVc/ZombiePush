using System;
using System.Collections;
using System.Collections.Generic;
using RVO;
using UnityEngine;
using Random = System.Random;
using Vector2 = RVO.Vector2;

public class GameAgent : MonoBehaviour
{
    [HideInInspector] public int sid = -1;

    /** Random number generator. */
    private Random m_random = new Random();
    public float StopDistance;
    public float LostDistance;
    //受击之类的特殊事件导致的短暂停止,值代表帧数
    public int bSkipTick=0;
    private bool delmark=false;
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (delmark) return;

        World CurWorld = World.GameWorld.GetComponent<World>();
        Vector2 goalVector= new Vector2(0, 0);
        if (bSkipTick<=0&&Vector3.Magnitude(CurWorld.player.position - transform.position) > StopDistance&& Vector3.Magnitude(CurWorld.player.position - transform.position) <= LostDistance)
        {
            Simulator.Instance.StartAgent(sid);
            goalVector = new Vector2(CurWorld.player.position.x, CurWorld.player.position.z) - Simulator.Instance.getAgentPosition(sid);
        }
        else 
        {
            if(bSkipTick>0) bSkipTick--;
            Simulator.Instance.StopAgent(sid);
        }

        if (sid >= 0)
        {
            Vector2 pos = Simulator.Instance.getAgentPosition(sid);
            Vector2 vel = Simulator.Instance.getAgentPrefVelocity(sid);
            transform.position = new Vector3(pos.x(), transform.position.y, pos.y());
            if (Math.Abs(vel.x()) > 0.01f && Math.Abs(vel.y()) > 0.01f)
                transform.forward = new Vector3(vel.x(), 0, vel.y()).normalized;
        }

        if (RVOMath.absSq(goalVector) > 1.0f)
        {
            goalVector = RVOMath.normalize(goalVector);
        }

        Simulator.Instance.setAgentPrefVelocity(sid, goalVector);

        /* Perturb a little to avoid deadlocks due to perfect symmetry. */
/*        float angle = (float) m_random.NextDouble()*2.0f*(float) Math.PI;
        float dist = (float) m_random.NextDouble()*0.0001f;

        Simulator.Instance.setAgentPrefVelocity(sid, Simulator.Instance.getAgentPrefVelocity(sid) +
                                                     dist*
                                                     new Vector2((float) Math.Cos(angle), (float) Math.Sin(angle)));*/
    }

    public void BeforeDestory() 
    {
        Simulator.Instance.StopAgent(sid);
        World.GameWorld.GetComponent<ObjectPoolManager>().DelayDeSpawn(this.gameObject, 3f);
        Simulator.Instance.setAgentPosition(sid, new Vector2(120, -280));
        delmark = true;
    }
    public void OnReborn(Vector3 Pos) 
    {
        Simulator.Instance.StartAgent(sid);
        Simulator.Instance.setAgentPosition(sid, new Vector2(Pos.x, Pos.z));
        delmark = false;
    }

    private void OnDestroy()
    {
        GetComponent<ORCAGameMainManager>().DeleteAgent(sid);
    }
}