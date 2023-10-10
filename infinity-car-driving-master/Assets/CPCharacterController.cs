using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

struct Damge
{

}
public enum CharacterState 
{
    None = 0,
    Runing= 1,
    PreShoot= 2,
}

public class CPCharacterController : Actor
{

    Animator animator;
    CharacterController characterController;
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
            moveVec=Camera.main.transform.TransformDirection(moveVec);
            moveVec = new Vector3 (moveVec.x,0, moveVec.z);
            characterController.Move(moveVec * Speed * SpeedOffset * Time.deltaTime);
            if(CurrentState!=CharacterState.PreShoot)
                transform.GetChild(1).rotation = Quaternion.LookRotation(moveVec,Vector3.up);
        }
        animator.SetFloat("Speed", Vector3.Magnitude(moveVec)*Speed);

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
            //Unity�Դ���Triggerֻ���ڽ��������Ż����ã������ǰ����ȴû��ִ�ж�����״̬�������,��Ҫ�ֶ����
            animator.ResetTrigger("Shoot");
            animator.SetBool("PreShoot", true);
            SpeedOffset = 0.3f;
            CurrentState = CharacterState.PreShoot;
        }
        else
        {
            animator.SetBool("PreShoot", false);
            SpeedOffset = 1.0f;
            CurrentState = CharacterState.None;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Unity�Դ���Trigger�ظ��Ƚ������һỺ��Trigger����Ҫ�ֶ����
/*            animator.ResetTrigger("Shoot");*/
            animator.SetTrigger("Shoot");
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
    
    public CharacterState GetCurState() { return CurrentState; }
    public float Speed;
    private float SpeedOffset=1.0f;
    private CharacterState CurrentState = CharacterState.None;
    public Transform CurWeaponSlot;
    public GameObject Test_DefaultWeapon;
}
