using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CPCharacterController : MonoBehaviour
{

    Animator animator;
    CharacterController characterController;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveVec=Vector3.zero;
        moveVec.x = Input.GetAxis("Horizontal");
        moveVec.z = Input.GetAxis("Vertical");

        if (moveVec.x > 0.05 || moveVec.z > 0.05)
        {
            characterController.SimpleMove(this.transform.TransformDirection(moveVec) * Speed);
        }
        Debug.Log(Vector3.Magnitude(moveVec) * Speed);
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

  
    }

    public float Speed;
}
