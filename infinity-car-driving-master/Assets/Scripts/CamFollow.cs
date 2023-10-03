using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Camera cam;
    public Transform target;
    public Transform player;

    void LateUpdate()
    {
        player = World.GameWorld.GetComponent<World>().GetCurPlayer();
        target = player.GetChild(0).transform;
        if (player == null) return;

        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        Quaternion rotationX = Quaternion.AngleAxis(x, Vector3.up);
        Quaternion rotationY = Quaternion.AngleAxis(-y, cam.transform.right);
        /*        Quaternion rot = rotationY * rot* rotationX;*/
        Quaternion rot = rotationX * rotationY * cam.transform.rotation;
        Debug.Log(LayerMask.GetMask("Player"));
        if (Mathf.Pow(2, player.gameObject.layer) == LayerMask.GetMask("Player"))
        {
            cam.transform.position = target.transform.TransformPoint(new Vector3(0, 0f, -5f));
            player.transform.rotation = Quaternion.Slerp(Quaternion.Euler(0, player.rotation.eulerAngles.y, 0), Quaternion.Euler(0, rot.eulerAngles.y, 0), 15 * Time.deltaTime);
            target.transform.rotation = Quaternion.Slerp(target.transform.rotation, rot, 15 * Time.deltaTime);
        }
        else 
        {
            cam.transform.position = target.transform.TransformPoint(new Vector3(0, 0f, -5f));
            target.transform.rotation = Quaternion.Slerp(target.transform.rotation, rot, 15 * Time.deltaTime);
        }

        if (rot.eulerAngles.x < 180)
        {
            rot = Quaternion.Euler(Mathf.Clamp(rot.eulerAngles.x, 0, 60), rot.eulerAngles.y, 0);
        }
        else
        {
            rot = Quaternion.Euler(Mathf.Clamp(rot.eulerAngles.x, 300, 360), rot.eulerAngles.y, 0);
        }
            
        cam.transform.rotation = rot;
    }
}
