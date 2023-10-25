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
            Debug.Log("���no����");
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("���Yes����");
        player = World.GameWorld.GetComponent<World>().player.GetComponent<CPCharacterController>();
        weaponController= player.CurWeapon.GetComponent<WeaponController>();
    }
}
