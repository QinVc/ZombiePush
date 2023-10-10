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
public class WeaponController : MonoBehaviour
{

    private void Update()
    {

    }
    public void Fire()
    {
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
        MyBulletObj.GetComponent<Bullet>().From = transform.parent.gameObject;
        MyBulletObj.transform.position = FirePoint.position;
        MyBulletObj.transform.LookAt(targetPos);
        MyBulletObj.GetComponent<Rigidbody>().AddForce(FireDirection*WeaponDistance,ForceMode.Impulse);
    }

    public void Reload()
    {

    }

    public void SetOwner(CPCharacterController cPCharacter) 
    {
        Owner = cPCharacter;
    }

    public Transform FirePoint;
    public float WeaponDistance;
    public CPCharacterController Owner;
    private Vector3 targetPos;
    private Vector3 FireDirection;
    private WeaponState CurWeaponState=WeaponState.None;
    public GameObject MyBullet;
}
