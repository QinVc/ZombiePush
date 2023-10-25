using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrown : Projectile
{
    public float Speed=1.0f;
    public float reflectValue = 1f;
    //����ϵ��
    public float damping =1f;
    public float g = 0.98f;

    public float ExplosionTime = 3f;

    float lastTime = 0;
    float RemainTime = 0;
    Vector3 Velocity;
    // Start is called before the first frame update
    void Start()
    {
        damge.from = From.GetComponent<Actor>();
        lastTime = Time.fixedTime;
        Invoke("Explosion", ExplosionTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float curTime = Time.fixedTime;

        float dt = curTime - lastTime+RemainTime;

        RemainTime = 0;

        Vector3 vV =new Vector3(0,(Velocity.y - g*dt)/damping,0);

        //Exp��ʹ�����������Ҳ�����0�����赽�෴����x��С��ʱ��ܴ��Ҽ���仯�����ı仯ƽ��-damping*dt
        Vector3 NewVelocity =vV + (Velocity-new Vector3(0,Velocity.y,0)) * Mathf.Exp(0);

        Vector3 moveDis = (NewVelocity + Velocity) * 0.5f*dt;
        
        //�ж���ײ
        Vector3 newPos = transform.position + moveDis;
        RaycastHit hit;

/*        Debug.Log("1��"+transform.position);
        Debug.Log("2��" + newPos);*/

        if (Physics.Linecast(transform.position, newPos, out hit))
        {
            float TruePath = (hit.point - transform.position).magnitude;
            float PrePath = moveDis.magnitude;

            float ratio = TruePath / PrePath;

            Vector3 trueAvgVelocity = Vector3.zero;
            //���ڼ������
            //��������������ײʱ������Velocity����0��NewVeloctiyС���㣬�ʲ�ֵ������ܴ���0<ratio�ӽ�0>��������õ�һ�����µ��ٶȲ�����
            //�������ٶȻ�������ʱ����Ԥ�⵽��һ֡�������ײ
            //�ٶ�Ϊ0ʱ�н�Ϊ0���ʲ������ٶ�Ϊ0
            if (Vector3.Angle(Velocity,NewVelocity)<90)
                trueAvgVelocity=Vector3.Lerp(Velocity, NewVelocity, ratio);
            //���������򲻽��з���
            if (trueAvgVelocity == Vector3.zero) 
            {
                //�����ٶ�Ϊ0ʱ�н�Ϊ0���ʲ������ٶ�Ϊ0
                //Velocity = Vector3.zero;
                lastTime = curTime;
                return; 
            }
            //���Ż���ʹ������ײ�淨�߷����ƶ�Ͷ����İ뾶��ֵ������ʵλ��
            float TrueTime = (hit.point - transform.position).magnitude / trueAvgVelocity.magnitude;
            RemainTime = dt - TrueTime;

            transform.position = transform.position + TrueTime * trueAvgVelocity;
            Velocity = Vector3.Reflect(trueAvgVelocity, hit.normal)*0.5f* reflectValue;
        }
        else 
        {
/*            Debug.Log("false");*/
            transform.position = newPos;
            Velocity = NewVelocity;
        }
        lastTime = curTime;
    }

    void Explosion() 
    {
        Instantiate(HitActorFX);
    }
}
