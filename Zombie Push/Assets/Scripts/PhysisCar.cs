using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhysisCar : MonoBehaviour
{
    public List<WheelColliderContianer> Wheels;
    public float maxMotor; //F*Dis=MOTOR
    public float maxSteering;
    public float maxBake;
    public float EnterColdDown=0.5f;
    public float CurEnterColdDown = 0.0f;
    public List<Transform> Passner;
    public GameObject Driver;

    private void Start()
    {
        Passner = new List<Transform>();
    }

    private void Update()
    {
        World CurWorld = World.GameWorld.GetComponent<World>();
        if (CurEnterColdDown > 0) CurEnterColdDown -= Time.deltaTime;
        if (CurWorld.player != this.transform) return;
        if (Input.GetKeyDown(KeyCode.F))
        {
            LeaveCar(CurWorld.GetCharacterPlayer());
            return;
        }
    }

    private void FixedUpdate()
    {
        World CurWorld = World.GameWorld.GetComponent<World>();
        if (CurWorld.player != this.transform) return;

        float steering = Input.GetAxis("Horizontal") * maxSteering;
        float motor = Input.GetAxis("Vertical") * maxMotor;
        float brake = Input.GetAxis("Jump") * maxBake;

        foreach (var wheel in Wheels)
        {
            if (wheel.UseMotor)
            {
                wheel.Left.motorTorque = motor;
                wheel.Right.motorTorque = motor;
            }

            if (wheel.UseSteering) 
            {
                wheel.Left.steerAngle = steering;
                wheel.Right.steerAngle = steering;
            }

            wheel.Left.brakeTorque = brake;
            wheel.Right.brakeTorque = brake;

            Vector3 pos;
            Quaternion rot;
            wheel.Left.GetWorldPose(out pos, out rot);
            wheel.Left.transform.position = pos;
            wheel.Left.transform.rotation = rot;

            wheel.Right.GetWorldPose(out pos, out rot);
            wheel.Right.transform.position = pos;
            wheel.Right.transform.rotation = rot;
        }
    }
    public void LeaveCar(Transform PlayerCharacter)
    {
        //leave Car
        CurEnterColdDown = EnterColdDown;
        PlayerCharacter.gameObject.SetActive(true);
        PlayerCharacter.transform.position = this.transform.position - new Vector3(3, 0, 0);
        Passner.Remove(PlayerCharacter);
        if (Driver == PlayerCharacter.gameObject)
        {
            Driver = null;
        }
        World.GameWorld.GetComponent<World>().SetPlayer(PlayerCharacter);
    }

    public void EnterCar(Transform PlayerCharacter)
    {
        if (CurEnterColdDown > 0) return;

        //Enter Car
        World.GameWorld.GetComponent<World>().player=this.transform;
        PlayerCharacter.gameObject.SetActive(false);
        if (Passner.Count == 0)
        {
            Driver = PlayerCharacter.gameObject;

        }
        Passner.Add(PlayerCharacter);
    }
}



[System.Serializable]
public struct WheelColliderContianer
{
    public WheelCollider Left;
    public WheelCollider Right;
    public bool UseMotor;
    public bool UseSteering;
}

/*首先,Forward是前后移动也就是motorTorque和brakeTorque. 
而Sideway是转向也就是脚本中的steerAngle.然后两个Stiffness控制的功能, 你可以理解为是地面摩擦力, 而Extremum和 Asymptote,
前者是控制刚刚开始起步, 后者控制在匀速过程。
举个例子, 你骑自行车的时候会感到刚开始起步需要更用力地蹬, 而骑着骑着速度上去了就不需要那么用力了, 一个道理, Extremum是起步,Asymptote就是匀速.
至于slip和Value就是控制着起步和匀速的位置和摩擦力乘数, slip是控制”什么程度才算是进入匀速”,而Value就是分别控制已经起步和匀速状态时的摩擦力(Stiffness)生效程度了.
摩檫力!=阻力，汽车靠摩檫力运动，因为轮子跟地面的接触点相对于地面是向后运动的，所以轮子受到来自地面的摩擦力向前，为车子提供动力。*/
