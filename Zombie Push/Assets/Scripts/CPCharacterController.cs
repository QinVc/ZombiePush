using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
public enum CharacterState 
{
    None = 0,
    Runing= 1,
    PreShoot= 2,
    Reload=3
}

public class CPCharacterController : Actor
{

    Animator animator;
    CharacterController characterController;
    public float Speed;
    public bool IsAim=false;
    private float SpeedOffset = 1.0f;
    private CharacterState CurrentState = CharacterState.None;
    public Transform CurWeaponSlot;
    public GameObject Test_DefaultWeapon;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        InitWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveVec=Vector3.zero;
        moveVec.x = Input.GetAxis("Horizontal");
        moveVec.z = Input.GetAxis("Vertical");

        if (Mathf.Abs(moveVec.x) > 0.05f || Mathf.Abs(moveVec.z) > 0.05f)
        {
            //人物的行走坐标系是相机View
            moveVec =Camera.main.transform.TransformDirection(moveVec);
            moveVec = new Vector3(moveVec.x, 0, moveVec.z);
            moveVec = Vector3.Normalize(moveVec);
            characterController.Move(moveVec * Speed * SpeedOffset * Time.deltaTime);
            if(!IsAim)
                transform.GetChild(1).rotation = Quaternion.LookRotation(moveVec,Vector3.up);
        }
        animator.SetFloat("Speed", Vector3.Magnitude(moveVec)*Speed);

        if (CurWeaponSlot.GetChild(0).GetComponent<WeaponController>().weaponType == WeaponType.Pistol)
            animator.SetBool("Rifle", false);
        else
            animator.SetBool("Rifle", true);

        if (Input.GetKeyDown(KeyCode.F))
        {
            World CurWorld = World.GameWorld.GetComponent<World>();
            if (CurWorld.player == this.transform)
            {
                Collider[] Res = Physics.OverlapSphere(transform.position, 3.0f, LayerMask.GetMask("Car"));
                if (Res.Length > 0)
                {
                    PhysisCar physisCar = Res[0].transform.GetChild(0).gameObject.GetComponent<PhysisCar>();
                    physisCar.EnterCar(this.transform);
                }
            }
        }

        if (Input.GetKey(KeyCode.Mouse1)) 
        {
            //Unity自带的Trigger只有在结束动画才会重置，如果提前按下却没有执行动画则状态不被清除,需要手动清除
            animator.ResetTrigger("Shoot");
            animator.SetBool("PreShoot", true);
            IsAim = true;
            SpeedOffset = 0.3f;
            CurrentState = CharacterState.PreShoot;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        if (Input.GetKeyUp(KeyCode.Mouse1)) 
        { 
            IsAim=false;
            animator.SetBool("PreShoot", false);
            if (CurrentState == CharacterState.Reload) 
            {
                SpeedOffset = 0.3f;
            }
            else 
            {
                SpeedOffset = 1.0f;
                CurrentState = CharacterState.None;
            }
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
          /*  if (CurrentState == CharacterState.Reload) return;*/
            //Unity自带的Trigger回复比较慢，且会缓存Trigger，需要手动清除
            animator.ResetTrigger("Shoot");
            if (CurWeaponSlot.GetChild(0).GetComponent<WeaponController>().CurAmmoInClip > 0&&CurrentState!=CharacterState.Reload) 
            {
                Debug.Log(CurrentState);
                Debug.Log("+");
                animator.SetTrigger("Shoot");
                animator.SetBool("RifleStop", false);
            }
            else 
            {
                animator.SetBool("RifleStop", true);
            }

        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            animator.ResetTrigger("Shoot");
            animator.SetBool("RifleStop",true);
        }
    }

    private void InitWeapon()
    {
        GameObject CurWeapon = Instantiate(Test_DefaultWeapon,CurWeaponSlot);
        CurWeapon.transform.localPosition = Vector3.zero;
        CurWeapon.transform.localRotation= Quaternion.identity;
        CurWeapon.transform.localScale = Vector3.one;
    }

    public void OnFire() 
    {
       CurWeaponSlot.GetChild(0).GetComponent<WeaponController>().Fire();
    }

    public void Reload() 
    {
        CurrentState = CharacterState.Reload;
        SpeedOffset = 0.3f;
        animator.SetBool("Reload",true);
    }

    public void AfterReloadAnim()
    {
        CurrentState=CharacterState.None;
        SpeedOffset = 1f;
        CurWeaponSlot.GetChild(0).GetComponent<WeaponController>().Reload();
        animator.SetBool("Reload", false);
    }

    public override void OnDamge(ActorDamge inDamge)
    {
        base.OnDamge(inDamge);
        HP -= inDamge.value;
        World.GameWorld.GetComponent<DelegateManager>().OnPlayerBeDamge(HP);
        if (HP <= 0) 
        {
            OnDead();
        }
    }

    public override void OnDead()
    {
        base.OnDead();
        World.GameWorld.GetComponent<DelegateManager>().OnGameStageChange(GameStage.Lose);
        Destroy(this.gameObject);
    }

    public CharacterState GetCurState() { return CurrentState; }

}
