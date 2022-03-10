using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guardian : MonoBehaviour
{
    public enum AttackType
    {
        Cactus,     // 仙人尖刺
        SoulStream, // 灵魂激流
        ChaosMeteor // 混沌陨石
    }

    public AttackType GuardianType;

    public List<GameObject> enemiesList = new List<GameObject>();

    Animator animator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemiesList.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            enemiesList.Remove(other.gameObject);
        }
    }

    private float cactusAttackRate = 0.7f;      // 攻击频率(x秒/次)
    private float cactusAttackTimer = 0;

    private float soulStreamDamageRate = 40f;    // 伤害值(伤害/秒)

    public GameObject BulletPrefab;
    public Transform FirePosition;

    public LineRenderer SoulStreamLineRenderer;
    public GameObject SoulStreamHitEffect;

    void Start()
    {
        cactusAttackTimer = cactusAttackRate;
        animator = GetComponent<Animator>();
        //SoulStreamLineRenderer.transform.position += SoulStreamHitEffect.transform.up * (-10);
        SoulStreamLineRenderer.enabled = false;
    }

    void Update()
    {
        // 转身面向敌人
        if (enemiesList.Count > 0 && enemiesList[0] != null)
        {
            Vector3 targetPos = enemiesList[0].transform.position;
            targetPos.y = transform.position.y; // 纵轴先对齐，只需要横向的转向
            transform.LookAt(targetPos);
        }

        // 攻击处理
        if (this.GuardianType == AttackType.Cactus)
        {
            if (enemiesList.Count > 0)
            {
                cactusAttackTimer += Time.deltaTime;
                if (cactusAttackTimer >= cactusAttackRate)
                {
                    cactusAttackTimer -= cactusAttackRate;
                    animator.SetBool("IsAttacking", true);
                    StartCoroutine(DelayInvoker.DelayToInvoke(() =>
                    {
                        Attack();
                    }, 0.3f));
                }
            }
            else
            {
                animator.SetBool("IsAttacking", false);
            }
        }
        else if (this.GuardianType == AttackType.SoulStream)
        {
            if (enemiesList.Count > 0)
            {
                if (enemiesList[0] == null)
                {
                    UpdateEnemies();
                }
                if (enemiesList.Count > 0)
                {
                    animator.SetBool("IsSpellcasting", true);

                    StartCoroutine(DelayInvoker.DelayToInvoke(() =>
                    {
                        if (enemiesList.Count > 0)
                        {
                            // 绘制射线
                            if (SoulStreamLineRenderer.enabled == false)
                            {
                                SoulStreamLineRenderer.enabled = true;
                            }
                            var enemyPos = enemiesList[0].transform.position;
                            enemyPos.y = FirePosition.position.y;
                            SoulStreamLineRenderer.SetPositions(new Vector3[] { FirePosition.position, enemyPos });

                            // 造成伤害
                            enemiesList[0].GetComponent<Enemy>().TakeDamage(soulStreamDamageRate * Time.deltaTime);

                            // 添加特效
                            SoulStreamHitEffect.transform.position = enemiesList[0].transform.position;

                            // 将特效朝向施法者
                            Vector3 pos = transform.position;
                            pos.y = enemiesList[0].transform.position.y;
                            SoulStreamHitEffect.transform.LookAt(pos);

                            //将特效朝施法者移动一点距离，避免被敌人模型挡住
                            SoulStreamHitEffect.transform.position += SoulStreamHitEffect.transform.forward * 1;
                            SoulStreamHitEffect.transform.position += SoulStreamHitEffect.transform.up * 1;
                        }
                        else
                        {
                            animator.SetBool("IsSpellcasting", false);

                            // 没有敌人就把特效藏到地下去
                            HideSoulStream();
                        }
                    }, 1.4f));
                }
            }
            else
            {
                animator.SetBool("IsSpellcasting", false);

                // 没有敌人就把特效藏到地下去
                HideSoulStream();
            }
        }
    }

    void Attack()
    {
        if (enemiesList.Count <= 0 || enemiesList[0] == null)
        {
            UpdateEnemies();
        }

        if (enemiesList.Count > 0)
        {
            var bullet = GameObject.Instantiate(BulletPrefab, FirePosition.position, FirePosition.rotation);
            bullet.GetComponent<Bullet>().SetTarget(enemiesList[0].transform);
        }
        else
        {
            cactusAttackTimer = cactusAttackRate;
        }

    }

    void UpdateEnemies()
    {
        for (int i = enemiesList.Count - 1; i >= 0; i--)
        {
            if (enemiesList[i] == null)
            {
                enemiesList.RemoveAt(i);
            }
        }
    }

    void HideSoulStream()
    {
        //SoulStreamLineRenderer.SetPositions(new Vector3[] { FirePosition.position, FirePosition.position });
        //SoulStreamLineRenderer.transform.position += SoulStreamHitEffect.transform.up * (-10);
        SoulStreamLineRenderer.enabled = false;

        SoulStreamHitEffect.transform.position = FirePosition.position;
        SoulStreamHitEffect.transform.position += SoulStreamHitEffect.transform.up * (-10);
    }
}
