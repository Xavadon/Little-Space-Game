using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    bool EnemiesAlive;
    [SerializeField] GameObject Walls;
    [SerializeField] Transform SpawnPos;
    [SerializeField] bool onlyRange;
    [SerializeField] bool onlyBomb;
    [SerializeField] bool onlyRocket;
    [SerializeField] bool RandomSpawn;
    [SerializeField] bool spawnBoss;
    [SerializeField] float rangeX;
    [SerializeField] float rangeY;
    [SerializeField] int EnemyCount;
    [SerializeField] GameObject[] Enemies = new GameObject[3];
    [SerializeField] GameObject EnemyPrefabRange;
    [SerializeField] GameObject EnemyPrefabBomb;
    [SerializeField] GameObject EnemyPrefabRocket;
    [SerializeField] GameObject EnemyBossPrefab;
    [SerializeField] GameObject WaitForSpawnEffect;
    [SerializeField] GameObject SpawnEffect;
    [SerializeField] GameObject SpawnSound;
    private void Start()
    {
        Enemies[0] = EnemyPrefabRange;
        Enemies[1] = EnemyPrefabBomb;
        Enemies[2] = EnemyPrefabRocket;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !EnemiesAlive)
        {
            Walls.SetActive(true);
            for (int i = 0; i < EnemyCount; i++)
            {
                StartCoroutine(SpawnEnemy());
            }
            EnemiesAlive = true;
        }
    }
    IEnumerator SpawnEnemy()
    {
        Vector3 range = SpawnPos.position + new Vector3(Random.Range(-rangeX, rangeX), Random.Range(-rangeY, rangeY));
        GameObject waitSpawnEffect = Instantiate(WaitForSpawnEffect, range, Quaternion.identity);
        yield return new WaitForSeconds(3f);
        Destroy(waitSpawnEffect);
        Instantiate(SpawnEffect, range, Quaternion.identity);
        Instantiate(SpawnSound, range, Quaternion.identity);

        if (onlyRange)
        {
            Instantiate(EnemyPrefabRange, range, Quaternion.identity);
        }
        if (onlyBomb)
        {
            Instantiate(EnemyPrefabBomb, range, Quaternion.identity);
        }
        if (onlyRocket)
        {
            Instantiate(EnemyPrefabRocket, range, Quaternion.identity);
        }
        if (RandomSpawn)
        {
            int enemyNumber = Random.Range(0, 2);
            Instantiate(Enemies[enemyNumber], range, Quaternion.identity);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            EnemyCount--;
        }
    }
    private void Update()
    {
        bool changed = false;
        if (EnemiesAlive && EnemyCount <= 0)
        {
            if (spawnBoss && !changed && EnemyBossPrefab != null)
            {
                changed = true;
                EnemyBossPrefab.GetComponent<EnemyBossController>().state_1 = true;
            }
            else
            {
                Walls.SetActive(false);
            }
        }
    }

}
