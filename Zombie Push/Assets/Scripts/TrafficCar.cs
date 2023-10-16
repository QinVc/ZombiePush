using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficCar : MonoBehaviour
{
    public Transform player;                            //ссылка на игрока
    public GameController gameController;

    public float minDistanceToDeleteObjects = 15f;      //расстояние позади игрока, на котором уничтожается авто
    public float maxDistanceToDeleteObjects = 120f;     //расстояние впереди игрока, на котором уничтожается авто

    private float velocity = 0f;                        //скорость авто
    private float timeOfSpawn;

    // Start is called before the first frame update
    void Start()
    {
        timeOfSpawn = Time.time;
        if (gameObject.tag == "Oncoming Line Traffic")
        {
            transform.Rotate(0f, 180f, 0f);
            velocity = Random.Range(10f, 11f);
        }
    }


    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * velocity * Time.deltaTime);

        if (player.position.z - transform.position.z > minDistanceToDeleteObjects ||
            transform.position.z - player.position.z > maxDistanceToDeleteObjects)
            Destroy(gameObject);
    }


    public void SetVelocityByPlayer(float playerVelocity)
    {
        if (playerVelocity < 10f)
            velocity = Random.Range(4f, 5f);
        else
            velocity = playerVelocity / 2f + Random.Range(3f, 4f);
    }


    public void SetVelocity(float newVelo)
    {
        velocity = newVelo;
    }

    public float GetVelocity()
    {
        return velocity;
    }

    private void OnCollisionEnter(Collision other)
    {
    }

}
