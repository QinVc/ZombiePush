using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardLook : MonoBehaviour
{
    public TrafficCar thisCar;      //ссылка на автомобиль, на котором находится текущий объект

    private void OnTriggerStay(Collider other)
    {
        GameObject go = other.gameObject;

        if (go.tag == "Oncoming Line Traffic" || go.tag == "Same Line Traffic")
        {
            TrafficCar tc = go.GetComponent<TrafficCar>();
            if (thisCar.GetVelocity() > tc.GetVelocity())
                thisCar.SetVelocity(tc.GetVelocity());
        }
        else if (go.tag == "Player")
        {
            Driving dr = go.GetComponent<Driving>();
            if (thisCar.GetVelocity() > dr.GetVelocity())
                thisCar.SetVelocity(dr.GetVelocity());
        }

    }

    private void OnTriggerExit(Collider other)
    {
        thisCar.SetVelocity(Random.Range(2f, 5f));
    }
}


