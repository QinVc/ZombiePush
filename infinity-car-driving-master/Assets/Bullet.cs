using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Vector3 lastPos;
    private void Start()
    {
        lastPos = transform.position;
    }
    private void Update()
    {
        RaycastHit hit;
        if (Physics.Linecast(lastPos, transform.position, out hit))
        {
           Debug.Log(hit.transform.gameObject.name);
        }
        lastPos = transform.position;
    }
}
