using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyWave
{
    public GameObject EnemyPrefab;
    public int Count;
    public float Rate;
    public float SpawnDelay;
}

public class EnemySpawner : MonoBehaviour
{
    public static int EnemyAliveCount = 0;
    public EnemyWave[] Waves;
    public Transform StartPoint;
    public float WaveRate = 3;

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
            yield return new WaitForSeconds(WaveRate);
        }
    }
}
