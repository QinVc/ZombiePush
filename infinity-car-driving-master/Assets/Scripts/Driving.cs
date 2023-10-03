using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Driving : MonoBehaviour
{
    [Header("Куда выводить информацию")]
    public Text textFPS;
    public Text textSpeed;
    public Text textPath;
    public Text textTotalPath;

    private GameObject[] wheels;        //колеса передние


    [Header("Данные для рассчета движения")]
    public int N_hp = 100;              //мощность - л.с
    public int M = 1000;                //масса - кг
    public float len = 1f;              //число для подбора угла поворота
    public float mu = 0.5f;             //коэф. торможения
    private int N;                      //мощность - дж
    private float v = 0f;               //скорость авто в м/c
    [Range(0f,  90f)]
    public float maxWheelAngle = 40f;   //максимальный угол поворота колес


    [Header("Данные для учета сопростивления")]
    public bool needToCalculateResist = true;
    public float K = 0.3f;
    public float S = 2.7f;
    public float Ro = 1.27f;

    
    [Header("Текущие данные об авто")]
    public float V_in_km_h = 0;         //скорость авто в км/ч
    public float MaxSpeed = 0f;         //макс. скорость авто в м/с
    public float TotalPath = 0f;        //пройденный путь
    
    private const float g = 9.80665f;   //ускорение свободного падения
    private float t = 0;                //время t
    private float v0 = 0;               //скорость v0


    void Start()
    {
        N = N_hp * 735;         //перевод в дж из л.с. 
        MaxSpeed = Mathf.Pow((2 * N / (K * S * Ro)), 0.3333f);

        wheels = new GameObject[2];
        wheels[0] = transform.GetChild(1).gameObject;
        wheels[1] = transform.GetChild(2).gameObject;

        StartCoroutine(dataShow(textFPS.GetComponent<Text>(), textSpeed.GetComponent<Text>()));
    }

    void Update()
    {
        calcS();        //расчет движения вперед
        calcR();        //расчет поворота
    }


    void calcS()
    {
        if (Input.GetButtonDown("RB") || Input.GetButtonUp("RB") || Input.GetButtonDown("LB") || Input.GetButtonUp("LB"))
        {
            v0 = v;
            t = 0;
        }

        int k = 0;

        if (Input.GetButton("RB")) k += 1;
        if (Input.GetButton("LB")) k -= 1;

        t = t + Time.deltaTime;

        if (needToCalculateResist)
        {
            if (k == 1)              //авто ускоряется - сила тяги равна N/v
            {
                if (v > 0f)
                    v = 1f / M * (N / v - K * S * Ro * v * v / 2) * Time.deltaTime + v;
                else
                    v = 1f / M * (N * 0.3f - K * S * Ro * v * v / 2) * Time.deltaTime + v;
            }
            else if (k == -1)        //авто тормозит - сила тяги равна силе трения
            {
                v = 1f / M * (-mu * g * M - K * S * Ro * v * v / 2) * Time.deltaTime + v;
            }
            else                     //авто катится - сила тяги равна 0
            {
                v = 1f / M * (0 - K * S * Ro * v * v / 2) * Time.deltaTime + v;
            }
        }
        else
        {
            if (k == 1)              //авто ускоряется
            {
                v = Mathf.Sqrt(v0 * v0 + (2 * N * t) / M);
            }
            else if (k == -1)        //авто замедляется
            {
                v = v0 - mu * g * t;
            }
            /*else                   //авто катится
            {
                v = v;
            }*/
        }

        if (v < 0) v = 0;

        V_in_km_h = v * 3.6f;

        float dist = v * Time.deltaTime;
        transform.position += transform.forward * dist;
        TotalPath += dist;
    }


    void calcR()
    {
        float rotationValue = Input.GetAxis("Horizontal") * maxWheelAngle;

        wheels[0].transform.localRotation = Quaternion.Euler(0f, rotationValue, 0f);
        wheels[1].transform.localRotation = Quaternion.Euler(0f, rotationValue, 0f);

        float wheelAngle = wheels[0].transform.localEulerAngles.y;

        if (wheelAngle >= 360f - maxWheelAngle)
            wheelAngle -= 360f;

        if (v > 0f)
        {
            transform.Rotate(Vector3.up, wheelAngle * Time.deltaTime);
        }
    }


    public float GetVelocity()
    {
        return v;
    }

    public void SetVelocity(float newVelo)
    {
        v = newVelo;
    }

    IEnumerator dataShow(Text fpsText, Text speedText)
    {
        while (true)
        {
            fpsText.text = "FPS = " + ((int)(1 / Time.deltaTime)).ToString();
            speedText.text = ((int)V_in_km_h).ToString() + " km/h";
            textPath.text = ((int)TotalPath).ToString() + " m";
            textTotalPath.text = "Your path is " + ((int)TotalPath).ToString() + " m";
            yield return new WaitForSeconds(0.1f);
        }
    }
    
}
