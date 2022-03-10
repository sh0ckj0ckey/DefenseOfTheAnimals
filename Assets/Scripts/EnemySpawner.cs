using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static int EnemyAliveCount = 0;
    public Wave[] Waves;
    public Transform StartPoint;
    public float WaveRate = 3;

    void Start()
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
