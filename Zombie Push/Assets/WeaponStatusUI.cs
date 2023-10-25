using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponStatusUI : MonoBehaviour
{
    public Image Icon;
    public Text AmmoText;
    private CPCharacterController player;
    private WeaponController weaponController;

    private void Start()
    {
        StartCoroutine("BindDelegate");
    }

    private void Update()
    {
        if (player != null)
           AmmoText.text = weaponController.CurAmmoInClip + " / " + weaponController.CurAmmoOutClip;
    }

    IEnumerator BindDelegate() 
    {
        while (World.GameWorld == null)
        {
            Debug.Log("玩家no加载");
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("玩家Yes加载");
        player = World.GameWorld.GetComponent<World>().player.GetComponent<CPCharacterController>();
        weaponController= player.CurWeapon.GetComponent<WeaponController>();
    }
}
