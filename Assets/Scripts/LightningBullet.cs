using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBullet : MonoBehaviour
{
    // 连环闪电最大跳跃次数
    public static int LightningChainLeapMaxTime = 3;

    // 跳跃检测范围
    public static int LightningChainLeapRadius = 8;

    public float Speed = 30;

    private float distanceArriveTarget = 1.2f;

    private Transform target;
    private float damage = 40;
    private float leapDamage = 30;
    private float slowdownTime = 1;

    public GameObject ChainLightningPrefab;

    public GameObject ExplosionEffectPrefab;

    public void InitBullet(Transform target, float damage, float leapDamage, float slowdownTime)
    {
        this.target = target;
        this.damage = damage;
        this.leapDamage = leapDamage;
        this.slowdownTime = slowdownTime;
    }

    void Update()
    {
        if (target == null)
        {
            DestroyBullet();
            return;
        }

        transform.LookAt(target.position);
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);


        Vector3 dir = target.position - transform.position;
        if (dir.magnitude < distanceArriveTarget)
        {
            long chainId = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            target.GetComponent<Enemy>().TakeLightningDamage(damage, leapDamage, slowdownTime, chainId, ChainLightningPrefab);
            DestroyBullet();
        }
    }

    void DestroyBullet()
    {
        var effect = GameObject.Instantiate(ExplosionEffectPrefab, transform.position, transform.rotation);
        Destroy(effect, 1);
        Destroy(this.gameObject);
    }
}
