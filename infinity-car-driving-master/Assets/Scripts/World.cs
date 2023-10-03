using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public static GameObject GameWorld;
    public GameObject CarPrefab;
    public GameObject CharacterPrefab;
    private GameObject CharacterObj;
    private GameObject CarObj;
    public Transform player;
    public GameObject[] tilePrefabs;                                  
    public float tileLength = 24f;                             
    public float distanceToReload = 500f;                            
    public int numberOfTiles = 5;                                    
    private float zSpawn = 0f;                                        
    private List<GameObject> activeTiles = new List<GameObject>();  
    public Transform motherOfTiles;                             


    void Start()
    {
        SpawnTile(-1);
        CarObj = Instantiate<GameObject>(CarPrefab);
        CharacterObj=Instantiate<GameObject>(CharacterPrefab);
        player = CharacterObj.transform;
        GameWorld = this.gameObject;
    }
    
    void Update()
    {
        if (player.position.z - (tileLength + 5) > zSpawn - (numberOfTiles * tileLength))
        {
            SpawnTile(Random.Range(0, tilePrefabs.Length));
            DeleteTile();
        }

        if (player.transform.position.z > distanceToReload)
            MoveWorld();
    }

    public void SpawnTile(int tileIndex)
    {
        if (tileIndex == -1)        //создание карты в начале
        {
            for (int i = 0; i < numberOfTiles; i++)
                SpawnTile(Random.Range(0, tilePrefabs.Length));
            
        }
        else                        //создание карты после начала
        {
            GameObject go = Instantiate(tilePrefabs[tileIndex], transform.forward * zSpawn, transform.rotation);
            go.transform.SetParent(motherOfTiles);
            activeTiles.Add(go);
            zSpawn += tileLength;
        }
    }

    private void DeleteTile(int num = 1)
    {
        if (num == -1)
        {
            for (int i = 0; i < numberOfTiles; i++)
            {
                Destroy(activeTiles[0]);     //удаляем со сцены
                activeTiles.RemoveAt(0);     //удаляем со списка
            }
            zSpawn = 0;
        }
        else
        {
            Destroy(activeTiles[0]);     //удаляем со сцены
            activeTiles.RemoveAt(0);     //удаляем со списка
        }
    }

    void MoveWorld()
    {
        print("lapWorld");

        DeleteTile(-1);
        SpawnTile(-1);

        player.position = new Vector3(player.position.x, player.position.y, 0f);
    }

    public Transform GetCurPlayer()
    {
        return player;
    }

    public Transform GetCharacterPlayer()
    {
        return CharacterObj.transform;
    }

    public void SetPlayer(Transform inplayer)
    {
        player=inplayer;
    }

}