using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum WeaponState 
{
    None,
    PreFire,
    Firing,
    AfterFiring,
    Reload,
    Equie,
    Unequie
}

public enum WeaponType 
{
    None,
    Rifle,
    Pistol,
    Thrown
}
public class WeaponController : MonoBehaviour
{
    public Transform FirePoint;
    public float WeaponDistance;
    public CPCharacterController Owner;
    private Vector3 targetPos;
    private Vector3 FireDirection;
    private WeaponState CurWeaponState = WeaponState.None;
    public GameObject MyBullet;
    public WeaponType weaponType;
    public int MaxAmmoInClip=30;
    public int CurAmmoInClip=30;
    public int CurAmmoOutClip = 270;
    public int MaxAmmo=300;
    private void Update()
    {

    }
    public void Fire()
    {
        //处理子弹逻辑
        if (CurAmmoInClip <= 0) 
        {
            return;
        }
        else 
        {
            CurAmmoInClip -= 1;
        }

        //发射子弹逻辑
        Ray RayFromCamCenter = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit raycastHit;
        if (Physics.Raycast(RayFromCamCenter, out raycastHit))
        {
            targetPos = raycastHit.point;
        }
        else
        {
            targetPos = RayFromCamCenter.GetPoint(100);
        }

        FireDirection = Vector3.Normalize(targetPos - FirePoint.position);
        GameObject MyBulletObj = Instantiate(MyBullet);
        MyBulletObj.GetComponent<Projectile>().From = transform.parent.gameObject;
        MyBulletObj.transform.position = FirePoint.position;
        MyBulletObj.transform.LookAt(targetPos);
        if (weaponType != WeaponType.Thrown) 
        {
            MyBulletObj.GetComponent<Rigidbody>().AddForce(FireDirection * WeaponDistance, ForceMode.Impulse);
        }
    }

    public void Reload()
    {
        if(CurAmmoOutClip <= 0)
        {
            Debug.Log("没子弹了"); 
        }
        else 
        {
            CurAmmoOutClip-=(MaxAmmoInClip-CurAmmoInClip);
            CurAmmoInClip = MaxAmmoInClip;
        }
    }

    public void SetOwner(CPCharacterController cPCharacter) 
    {
        Owner = cPCharacter;
    }


}
