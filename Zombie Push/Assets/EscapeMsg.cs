using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EscapeMsg : MonoBehaviour
{
    public GameObject MsgGurd;
    private void Start()
    {
        Instantiate(MsgGurd, transform.position, Quaternion.identity);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            World.GameWorld.GetComponent<DelegateManager>().OnPlayerGetMsg();
            Destroy(this.gameObject);
        }
    }
}
