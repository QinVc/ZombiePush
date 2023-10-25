using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrown : Projectile
{
    public float Speed=1.0f;
    public float reflectValue = 1f;
    //阻力系数
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

        //Exp会使函数收敛，且不等于0不会阻到相反，在x很小的时候很大且急剧变化，最后的变化平缓-damping*dt
        Vector3 NewVelocity =vV + (Velocity-new Vector3(0,Velocity.y,0)) * Mathf.Exp(0);

        Vector3 moveDis = (NewVelocity + Velocity) * 0.5f*dt;
        
        //判断碰撞
        Vector3 newPos = transform.position + moveDis;
        RaycastHit hit;

/*        Debug.Log("1："+transform.position);
        Debug.Log("2：" + newPos);*/

        if (Physics.Linecast(transform.position, newPos, out hit))
        {
            float TruePath = (hit.point - transform.position).magnitude;
            float PrePath = moveDis.magnitude;

            float ratio = TruePath / PrePath;

            Vector3 trueAvgVelocity = Vector3.zero;
            //存在极限情况
            //当超快速连续碰撞时，存在Velocity大于0，NewVeloctiy小于零，故插值结果可能大于0<ratio接近0>，反射后会得到一下向下的速度不合理
            //当物体速度还在向上时，就预测到下一帧向地面碰撞
            //速度为0时夹角为0，故不能让速度为0
            if (Vector3.Angle(Velocity,NewVelocity)<90)
                trueAvgVelocity=Vector3.Lerp(Velocity, NewVelocity, ratio);
            //若发生了则不进行反射
            if (trueAvgVelocity == Vector3.zero) 
            {
                //错误，速度为0时夹角为0，故不能让速度为0
                //Velocity = Vector3.zero;
                lastTime = curTime;
                return; 
            }
            //待优化，使用沿碰撞面法线法线移动投掷物的半径的值才是真实位置
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
