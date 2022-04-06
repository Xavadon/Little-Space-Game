using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public int damage;
    public bool isPlayerBullet;
    public bool isEnemyBullet;
    public GameObject BulletEXPL;
    [SerializeField] GameObject ShootSound;
    [SerializeField] GameObject EXPLSound;
    private void Start()
    {
        Instantiate(ShootSound, transform.position, Quaternion.identity);
        //ShootSound.Play();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPlayerBullet && collision.tag == "Enemy")
        {
            DestroyBullet();
            if (collision.GetComponent<EnemyBossController>() != null)
            {
                collision.GetComponent<EnemyBossController>().DamageEnemy(damage);
            }
            else
            {
                collision.GetComponent<EnemyController>().DamageEnemy(damage);
            }
            //Hit enemy
        }
        if (isPlayerBullet && collision.tag == "EnemyTurrel")
        {
            collision.GetComponent<EnemyTurrelController>().DamageEnemy(damage);
            DestroyBullet();
            //Hit enemy
        }
        if (isEnemyBullet && collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().DamagePlayer(damage);
            DestroyBullet();
            //Hit Player
        }
        if (collision.tag == "Wall")
        {
            DestroyBullet();
        }
    }
    void DestroyBullet()
    {
        Instantiate(BulletEXPL, transform.position, Quaternion.identity);
        Instantiate(EXPLSound, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
