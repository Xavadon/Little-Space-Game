using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShieldController : MonoBehaviour
{
    float smooth = 0f;
    public float amount = 0f;

    void FixedUpdate()
    {
        smooth += amount;
        transform.rotation = Quaternion.Euler(0, 0, smooth);
        if (transform.parent.gameObject.GetComponent<EnemyBossController>().state_1 == true)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Vector2 force = collision.transform.position - transform.position;
        if (collision.tag != "Enemy" && collision.tag != "Room")
        {
            collision.GetComponent<Rigidbody2D>().AddForce(force.normalized * 500f * Time.deltaTime, ForceMode2D.Impulse);
        }
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().DamagePlayer(10);
        }
    }
}