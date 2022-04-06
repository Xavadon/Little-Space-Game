using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashHitColliderController : MonoBehaviour
{
    public int damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            if (collision.GetComponent<EnemyBossController>() != null)
            {
                collision.GetComponent<EnemyBossController>().DamageEnemy(100 + damage);
            }
            else
            {
                collision.GetComponent<EnemyController>().DamageEnemy(100 + damage);
            }
        }
        if (collision.tag == "EnemyTurrel")
        {
            collision.GetComponent<EnemyTurrelController>().DamageEnemy(100 + damage);
        }
    }
}
