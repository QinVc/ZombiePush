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
        CarObj = Instantiate<GameObject>(CarPrefab);
        CharacterObj=Instantiate<GameObject>(CharacterPrefab);
        player = CharacterObj.transform;
        GameWorld = this.gameObject;
    }
    
    void Update()
    {

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