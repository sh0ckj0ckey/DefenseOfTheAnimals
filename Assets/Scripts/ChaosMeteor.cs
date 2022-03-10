using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosMeteor : MonoBehaviour
{
    // 这个火球处于自己的一个单独Layer叫做ChaosMeteor，结果一片漆黑没有光照
    // 需要到MainScene的DirectionalLight里面调整属性CullingMask，把这个Layer也添加进去

    //private Vector3 spawnOffset = new Vector3(5, 5, 5);

    private Vector3 targetPosition = new Vector3(0, 0, 0);

    // 火球在地面滚动的最远距离
    public float TravelDistance = 200;
    // 为了防止火球滚动到地图边缘等地方被卡住位置，所以加一个最大滚动时间，超过了这个时间也要销毁火球
    public float TravelMaxDuration = 8;
    private float travelTimer = 0;

    public float flySpeed, groundSpeed, meteorSize, spinSpeed, pulseTime, pulseRadius;
    public LayerMask GroundLayer;

    // 地面的Y坐标，火球低于这个值就销毁掉
    public float GroundYAxis = 20;

    private GameObject impactEffect;
    private float impactEffectDestroyDelay = 3;

    private Vector3 moveDirection, landLocation;
    private float pulseTimer;

    private bool isMeteorFlying = true;

    public void SetTarget(Vector3 tar)
    {
        targetPosition = tar;
        moveDirection = transform.position - targetPosition;

        Vector3 relativePos = moveDirection - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = rotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        impactEffect = transform.Find("ImpactEffect").gameObject;

        //moveDirection = transform.position - (transform.position + spawnOffset);
        //transform.position = transform.position + targetPosition;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (isMeteorFlying)
        {
            if (this.transform.position.y < GroundYAxis)
            {
                Destroy(gameObject);
                return;
            }

            transform.position = transform.position + moveDirection * flySpeed * Time.deltaTime;

            if (Physics.Raycast(transform.position, Vector3.down, out hit, meteorSize, GroundLayer))
            {
                isMeteorFlying = false;
                moveDirection.y = 0;
                landLocation = hit.point;

                impactEffect.SetActive(true);
                impactEffect.transform.SetParent(null);
                Destroy(impactEffect.gameObject, impactEffectDestroyDelay);
            }
        }
        else
        {
            if (Physics.Raycast(transform.position, Vector3.down, out hit, meteorSize, GroundLayer))
            {
                Vector3 nextPos = new Vector3(transform.position.x, hit.point.y + meteorSize, transform.position.z);
                transform.position = nextPos + moveDirection * groundSpeed * Time.deltaTime;
            }

            travelTimer += Time.deltaTime;

            float dist = Vector3.Distance(landLocation, transform.position);
            if (dist > TravelDistance || travelTimer > TravelMaxDuration)
            {
                Destroy(gameObject);
                return;
            }
            float spin = spinSpeed * Time.deltaTime;
            transform.Rotate(spin, 0, 0);
        }
    }

    void DamagePulses()
    {
        if (pulseTimer > 0)
        {
            pulseTimer -= Time.deltaTime;
        }
        else
        {
            Collider[] gameObjsInRange = Physics.OverlapSphere(transform.position, pulseRadius);
            foreach (var item in gameObjsInRange)
            {
                if (item.tag != "Enemy")
                {
                    continue;
                }

                Rigidbody enemy = item.GetComponent<Rigidbody>();
                if (enemy != null)
                {
                    //enemy.Damage();
                }
            }

            pulseTimer = pulseTime;
        }
    }
}
