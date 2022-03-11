using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public enum EnemyType
    {
        Slime,
        Turtle,
        Boss
    }

    private float speed = 8;
    private float CurrentHp;
    public int TotalHp = 200;
    private float rotateSpeed = 5;
    private Transform[] positions;
    private int index = 0;

    private Animator anim;
    private Rigidbody rbody;
    public GameObject DieEffect;
    private Slider hpSlider;

    public GameObject SelfBody; // 模型本身，转向时只转动这个部分，避免血条也一起转动了

    public EnemyType Type;

    // Start is called before the first frame update
    void Start()
    {
        positions = Waypoints.Positions;
        rbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        transform.position = positions[0].position;
        hpSlider = GetComponentInChildren<Slider>();
        CurrentHp = TotalHp;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (index < positions.Length - 1)
        {
            // 移动
            var nor = (positions[index + 1].position - positions[index].position).normalized;
            //transform.Translate(nor * Time.deltaTime * speed);
            Vector3 pos = rbody.position;
            pos.x += nor.x * speed * Time.deltaTime;
            pos.y += nor.y * speed * Time.deltaTime;
            pos.z += nor.z * speed * Time.deltaTime;
            rbody.MovePosition(pos);

            if (nor.x > nor.z)
            {
                // 左右移动
                anim.SetFloat("moveZ", 0);
                anim.SetFloat("moveX", nor.x < 0 ? -1 : 1);
            }
            else
            {
                // 前后移动
                anim.SetFloat("moveX", 0);
                anim.SetFloat("moveZ", nor.z < 0 ? -1 : 1);
            }

            // 旋转
            Quaternion q = Quaternion.LookRotation(positions[index + 1].position - positions[index].position);
            SelfBody.transform.rotation = Quaternion.Slerp(SelfBody.transform.rotation, q, rotateSpeed * Time.deltaTime);

            // 到达一个waypoint，下一帧开始前往下一个waypoint
            if (Vector3.Distance(positions[index + 1].position, transform.position) < 1f)
            {
                index++;
                if (index >= positions.Length - 1)
                {
                    //anim.SetTrigger("idle");
                    ReachDestination();
                    return;
                }
            }
        }
    }

    void ReachDestination()
    {
        GameObject.Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        try
        {
            EnemySpawner.EnemyAliveCount--;
        }
        catch { }
    }

    /// <summary>
    /// 受到仙人球的伤害
    /// </summary>
    /// <param name="damage">伤害值</param>
    public void TakeCactusDamage(float damage)
    {
        if (CurrentHp > 0)
        {
            CurrentHp -= damage;
            hpSlider.value = (float)CurrentHp / TotalHp;
            if (CurrentHp <= 0)
            {
                Die();
            }
        }
    }

    /// <summary>
    /// 收到连环闪电的伤害
    /// </summary>
    /// <param name="damage">伤害值</param>
    /// <param name="leapTimes">跳跃次数</param>
    /// <param name="chainId">这条连环闪电的ID，用于标记受到作用的敌人</param>
    public void TakeLightningDamage(float damage, int leapTimes, long chainId)
    {
        if (CurrentHp > 0)
        {
            CurrentHp -= damage;
            hpSlider.value = (float)CurrentHp / TotalHp;
            if (CurrentHp <= 0)
            {
                Die();
            }
        }

        // 寻找下一个跳跃目标，不可重复，当跳跃到这个目标身上后添加一个标记，拥有标记的就不会再跳跃

    }

    private void Die()
    {
        switch (Type)
        {
            case EnemyType.Slime:
                BuildManager.Instance.AddMoney(5);
                break;
            case EnemyType.Turtle:
                BuildManager.Instance.AddMoney(10);
                break;
            case EnemyType.Boss:
                BuildManager.Instance.AddMoney(9999);
                break;
            default:
                break;
        }
        var effect = GameObject.Instantiate(DieEffect, transform.position, transform.rotation);
        Destroy(effect, 1.5f);
        Destroy(this.gameObject);
    }
}
