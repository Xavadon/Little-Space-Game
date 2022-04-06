using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPointSpawnerController : MonoBehaviour
{
    public GameObject EnemyPrefab;
    [SerializeField] GameObject SpawnEffect;
    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }
    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(Random.Range(1f, 1.5f));
        Instantiate(SpawnEffect, transform.position, Quaternion.identity);
        Instantiate(EnemyPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
