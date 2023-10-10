using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject From;
    public GameObject HitActorFX;
    private Vector3 lastPos;
    private ActorDamge damge;
    private void Start()
    {
        damge.from = From.GetComponent<Actor>();
        lastPos = transform.position;
    }
    private void Update()
    {
        RaycastHit hit;
        if (Physics.Linecast(lastPos, transform.position, out hit))
        {
            lastPos = transform.position;
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("EnemyHead")) { damge.value = 100; DamgeActor(hit); }
            else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("EnemyBody")) { damge.value = 50; DamgeActor(hit); }
            else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("EnemyHand")) { damge.value = 30; DamgeActor(hit); }
            else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("EnemyLeg")) { damge.value = 30; DamgeActor(hit); }
            else { Destroy(this.gameObject); }
        }
 
    }

    private void DamgeActor(RaycastHit hit) 
    {

        Instantiate(HitActorFX, hit.point, Quaternion.identity);

        damge.to = hit.transform.root.GetComponent<Actor>();
        damge.to.OnDamge(damge);
        Destroy(this.gameObject);
    }
}
