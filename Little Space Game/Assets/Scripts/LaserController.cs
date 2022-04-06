using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    int damage;
    void FixedUpdate()
    {
        damage = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().damage;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            if (collision.GetComponent<EnemyController>() != null)
            {
                collision.GetComponent<EnemyController>().DamageEnemyLaser(damage);
            }
            if (collision.GetComponent<EnemyTurrelController>() != null)
            {
                collision.GetComponent<EnemyTurrelController>().DamageEnemyLaser(damage);
            }
            if (collision.GetComponent<EnemyBossController>() != null)
            {
                collision.GetComponent<EnemyBossController>().DamageEnemyLaser(damage);
            }
        }
        if (collision.tag == "Bullet")
        {
            Destroy(collision.gameObject);
        }
    }
}
