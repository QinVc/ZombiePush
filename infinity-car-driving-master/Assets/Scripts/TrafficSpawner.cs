using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficSpawner : MonoBehaviour
{
    public Transform player;                                           //ссылка на игрока
    private Driving playerStats;
    public GameController gameController;
    public GameObject[] trafficCarPrefabs;                             //префабы препятствий  

    public int maxCarsOnLine = 4;                                      //количество одновременно существующих машин на сцене каждого типа
    public float minDistanceToDeleteObjects = 15f;                     //расстояние позади игрока, на котором уничтожится барьер
    public float maxDistanceToDeleteObjects = 120f;                    //расстояние впереди игрока, на котором уничтожится барьер
    public float distanceToReload = 500f;                              //расстояние, на котором траффик сместится в нуль

    public float minSpawnTime = 0.4f;                                  //минимальное время спавна траффика
    public float maxSpawnTime = 2f;                                    //максимальное время спавна траффика

    public int minSpawnDistanceX = 3;                                  //минимальная дистанция по X от центра
    public int maxSpawnDistanceX = 7;                                  //максимальная дистанция по X от центра

    public Transform[] motherOfTraffic = new Transform[2];             //контейнер для заспавненных встречных(0) и попутных(1) авто

    private GameObject[] lastTraffic = new GameObject[2];              //последние заспавненные встречные(0) и попутные(1) авто

    private float nextSpawnTime;


    void Start()
    {
        nextSpawnTime = maxSpawnTime;
        playerStats = player.gameObject.GetComponent<Driving>();
        StartCoroutine(SpawnOncomingObject());
        StartCoroutine(SpawnSameLineObject());
    }

    void Update()
    {
        float koefSpawn = playerStats.GetVelocity() / playerStats.MaxSpeed ;   //коэфициент частоты появления траффика - от 0 до 1
        nextSpawnTime = maxSpawnTime * (1 - koefSpawn) + minSpawnTime;
        
        //перемещение всех авто к нулю
        if (player.transform.position.z > distanceToReload)
            MoveWorld();
    }


    //спавн встречных авто
    IEnumerator SpawnOncomingObject()
    {
        while (true)
        {
            if (motherOfTraffic[0].childCount < maxCarsOnLine)
            {
                Vector3 coord;

                if (lastTraffic[0] == null)
                    lastTraffic[0] = player.gameObject;

                coord = new Vector3(Random.Range(-maxSpawnDistanceX, -minSpawnDistanceX),
                                    0f,
                                    player.position.z + 100f);

                if (Mathf.Abs(lastTraffic[0].transform.position.z - coord.z) < 5f)
                {
                    coord.z += 15f;
                }
                lastTraffic[0] = Instantiate(trafficCarPrefabs[Random.Range(0, trafficCarPrefabs.Length)], coord, transform.rotation);
                lastTraffic[0].transform.SetParent(motherOfTraffic[0]);
                lastTraffic[0].tag = "Oncoming Line Traffic";

                TrafficCar tc = lastTraffic[0].GetComponent<TrafficCar>();
                tc.minDistanceToDeleteObjects = maxDistanceToDeleteObjects;
                tc.minDistanceToDeleteObjects = minDistanceToDeleteObjects;
                tc.player = player;
                tc.gameController = gameController;
                }

            yield return new WaitForSeconds(nextSpawnTime);
        }
    }


    //спавн попутных авто
    IEnumerator SpawnSameLineObject()
    {
        while (true)
        {
            if (motherOfTraffic[1].childCount < maxCarsOnLine)
            {
                Vector3 coord;

                if (lastTraffic[1] == null)
                    lastTraffic[1] = player.gameObject;

                coord = new Vector3(Random.Range(minSpawnDistanceX, maxSpawnDistanceX),
                                    0f,
                                    player.position.z + 100f);

                bool canSpawn = true;       //можно ли спавнить авто в данной позиции

                if (playerStats.GetVelocity() < 4f)
                {
                    coord.z = player.position.z - 10f;

                    if (coord.x - player.position.x < 1.5f && coord.x - player.position.x > -1.5f)
                    {
                        canSpawn = false;
                        lastTraffic[1] = null;
                    }

                    if (Mathf.Abs(lastTraffic[0].transform.position.z - coord.z) < 20f)
                        canSpawn = false;
                }
                else
                {
                    if (Mathf.Abs(lastTraffic[0].transform.position.z - coord.z) < 10f)
                        coord.z += 15f;
                }

                if (canSpawn)
                {
                    lastTraffic[1] = Instantiate(trafficCarPrefabs[Random.Range(0, trafficCarPrefabs.Length)], coord, transform.rotation);
                    lastTraffic[1].transform.SetParent(motherOfTraffic[1]);
                    lastTraffic[1].tag = "Same Line Traffic";

                    TrafficCar tc = lastTraffic[1].GetComponent<TrafficCar>();
                    tc.minDistanceToDeleteObjects = maxDistanceToDeleteObjects;
                    tc.minDistanceToDeleteObjects = minDistanceToDeleteObjects;
                    tc.player = player;
                    tc.gameController = gameController;
                    tc.SetVelocityByPlayer(playerStats.GetVelocity());
                }
            }
            yield return new WaitForSeconds(nextSpawnTime);
        }
    }
    

    void MoveWorld()
    {
        print("lapTraffic");

        //перемещение объектов встречного и попутного движения
        for (int j = 0; j < 2; j++)
            for (int i = 0; i < motherOfTraffic[j].childCount; i++)
            {
                try
                {
                    float zz = player.position.z - motherOfTraffic[j].GetChild(i).position.z;        //расстояние между персонажем и данным объектом

                    motherOfTraffic[j].GetChild(i).transform.position = new Vector3(motherOfTraffic[j].GetChild(i).transform.position.x,
                                                                                    motherOfTraffic[j].GetChild(i).transform.position.y,
                                                                                    0 - zz);
                }
                catch
                {
                    print("smth bad happened");
                }
            }
    }


    
}
