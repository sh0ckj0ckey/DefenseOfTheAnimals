using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyWave
{
    public GameObject EnemyPrefab;
    public int Count;               // 这一波敌人数量
    public float Rate;              // 这一波敌人的产生间隔
    public float SpawnDelay = 3;    // 延迟x秒后产生这一批敌人
}

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    public static int EnemyAliveCount = 0;
    public EnemyWave[] Waves;
    public Transform StartPoint;

    private void Awake()
    {
        Instance = this;    //把当前的对象赋给Instance便于外部访问
    }

    void Start()
    {

    }

    public void StartSpawningEnemy()
    {
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy()
    {
        foreach (var wave in Waves)
        {
            for (int i = 0; i < wave.Count; i++)
            {
                GameObject.Instantiate(wave.EnemyPrefab, StartPoint.position, Quaternion.identity);
                EnemyAliveCount++;
                if (i < wave.Count)
                {
                    yield return new WaitForSeconds(wave.Rate);
                }
            }
            while (EnemyAliveCount > 0)
            {
                yield return 0;
            }
            yield return new WaitForSeconds(wave.SpawnDelay);
        }
    }
}
