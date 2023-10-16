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

/*����,Forward��ǰ���ƶ�Ҳ����motorTorque��brakeTorque. 
��Sideway��ת��Ҳ���ǽű��е�steerAngle.Ȼ������Stiffness���ƵĹ���, ��������Ϊ�ǵ���Ħ����, ��Extremum�� Asymptote,
ǰ���ǿ��Ƹոտ�ʼ��, ���߿��������ٹ��̡�
�ٸ�����, �������г���ʱ���е��տ�ʼ����Ҫ�������ص�, �����������ٶ���ȥ�˾Ͳ���Ҫ��ô������, һ������, Extremum����,Asymptote��������.
����slip��Value���ǿ������𲽺����ٵ�λ�ú�Ħ��������, slip�ǿ��ơ�ʲô�̶Ȳ����ǽ������١�,��Value���Ƿֱ�����Ѿ��𲽺�����״̬ʱ��Ħ����(Stiffness)��Ч�̶���.
Ħ����!=������������Ħ�����˶�����Ϊ���Ӹ�����ĽӴ�������ڵ���������˶��ģ����������ܵ����Ե����Ħ������ǰ��Ϊ�����ṩ������*/
