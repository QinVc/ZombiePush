using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerateTrriger : MonoBehaviour
{
    public Transform[] transforms;
    public int Min;
    public int Max;
    private void OnTriggerEnter(Collider other)
    {
        World.GameWorld.GetComponent<ORCAGameMainManager>().GenEnemy(Min,Max,transforms);
        Destroy(this.gameObject);
    }
}
