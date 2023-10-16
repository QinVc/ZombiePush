using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderScript : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameObject go = collision.gameObject;
            /*
            Driving dr = go.GetComponent<Driving>();
            dr.SetVelocity(dr.GetVelocity() / 2);
            if (go.transform.position.x > 0f)
            {
                Vector3 gg = go.transform.position;
                gg.x -= 0.1f;
                go.transform.position = gg;

                float rotY = go.transform.rotation.y;
                go.transform.rotation = Quaternion.Euler(Vector3.zero);
                go.transform.Rotate(0f, rotY / 2 * 57.296f, 0f);

            }
            else
            {
                Vector3 gg = go.transform.position;
                gg.x += 0.1f;
                go.transform.position = gg;

                float rotY = go.transform.rotation.y;
                go.transform.rotation = Quaternion.Euler(Vector3.zero);
                go.transform.Rotate(0f, rotY / 2f * 57.296f, 0f);

            }*/
            go.transform.rotation = Quaternion.Euler(Vector3.zero);
        }
    }
}
