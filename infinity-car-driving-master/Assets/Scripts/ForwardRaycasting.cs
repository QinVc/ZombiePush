using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardRaycasting : MonoBehaviour
{
    public TrafficCar thisCar;      //ссылка на автомобиль, на котором находится текущий объект
    private Transform[] rayPoints;  //ссылки на иcходящие точки рейкастов
    public float rayLength = 3f;    //длина рейкастов
    private RaycastHit hit;         //мишень рейкаста

    private void Start()
    {
        rayPoints = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            rayPoints[i] = transform.GetChild(i);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < rayPoints.Length; i++)
        {
            if (Physics.Raycast(rayPoints[i].position, transform.forward, out hit, rayLength))
            {
                print("got smth");

                GameObject go = hit.collider.gameObject;
                if (go.tag == "OncomingLineTraffic" || go.tag == "SameLineTraffic")
                {
                    TrafficCar tc = go.GetComponent<TrafficCar>();
                    if (thisCar.GetVelocity() > tc.GetVelocity())
                        thisCar.SetVelocity(tc.GetVelocity() - 0.1f);
                }
                else if (go.tag == "Player")
                {
                    Driving dr = go.GetComponent<Driving>();
                    if (thisCar.GetVelocity() > dr.GetVelocity())
                        thisCar.SetVelocity(dr.GetVelocity() - 0.1f);
                }
            }
        }
    }
}
