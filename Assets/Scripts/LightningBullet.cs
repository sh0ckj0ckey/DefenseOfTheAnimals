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

    public int Damage = 40;
    public int LeapDamage = 30;
    public float Speed = 30;

    private float distanceArriveTarget = 1.2f;

    private Transform target;

    public GameObject ChainLightningPrefab;

    public GameObject ExplosionEffectPrefab;

    public void SetTarget(Transform tar)
    {
        target = tar;
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
            target.GetComponent<Enemy>().TakeLightningDamage(Damage, LeapDamage, chainId, ChainLightningPrefab);
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
