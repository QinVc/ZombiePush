using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPProgressBar : MonoBehaviour
{
    Actor player;
    // Start is called before the first frame update
    void Start()
    {
        player = World.GameWorld.GetComponent<World>().player.GetComponent<Actor>();
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Image>().fillAmount = player.HP/(float)player.MaxHP;
    }
}
