using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField] float fieldOfImpact;
    [SerializeField] float force;
    [SerializeField] LayerMask layerToHit;
    [SerializeField] GameObject bombExplotion;
    [SerializeField] GameObject ShootSound;
    [SerializeField] GameObject ExplodeSound;
    [SerializeField] GameObject bombPrefab;
    public bool isEnemyBomb;
    public bool isPlayerBomb;
    public bool isShooted;
    public int damage;
    private void Start()
    {
        Instantiate(ShootSound, transform.position, Quaternion.identity);
        StartCoroutine(WaitForExplode());
    }
    IEnumerator WaitForExplode()
    {
        yield return new WaitForSeconds(1f);
        Explode();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("triggered");
        if (((collision.tag == "Enemy" || collision.tag == "EnemyTurrel") && isPlayerBomb) || (collision.tag == "Player" && isEnemyBomb))
        {
            Explode();
        }

    }
    void Explode()
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position, fieldOfImpact, layerToHit);
        Instantiate(bombExplotion, transform.position, Quaternion.identity);
        Instantiate(ExplodeSound, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
        foreach (Collider2D obj in objects)
        {
            /*Vector2 direction = obj.transform.position - transform.position;
            if (obj.tag != "Player" && obj.tag != "EnemyTurrel" && obj.tag !="Bullet")
            {
                obj.GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
            }*/
            if (obj.tag == "Bullet")
            {
                Destroy(obj);
            }
            if (isPlayerBomb)
            {
                if (obj.tag == "Enemy")
                {
                    if (obj.GetComponent<EnemyController>() != null)
                    {
                        obj.GetComponent<EnemyController>().DamageEnemy(100 + damage);
                    }
                    if (obj.GetComponent<EnemyBossController>() != null)
                    {
                        obj.GetComponent<EnemyBossController>().DamageEnemy(100 + damage);
                    }
                }
                if (obj.tag == "EnemyTurrel")
                {
                    obj.GetComponent<EnemyTurrelController>().DamageEnemy(20 + damage); //Original 100 + damage;
                }
            }
            if (obj.tag == "Player" && isEnemyBomb)
            {
                if (obj.GetComponent<PlayerController>() != null)
                {
                    obj.GetComponent<PlayerController>().DamagePlayer(20 + damage); //Original 100 + damage;
                }
            }

        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fieldOfImpact);
    }
}
