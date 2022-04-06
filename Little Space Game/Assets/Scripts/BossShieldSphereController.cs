using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShieldSphereController : MonoBehaviour
{
    float smooth = 0f;
    void Start()
    {
        
    }

    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Vector2 force = collision.transform.position - transform.position;
        if (collision.GetComponent<Rigidbody2D>() != null)
        {
            collision.GetComponent<Rigidbody2D>().AddForce(force.normalized * 500f * Time.deltaTime, ForceMode2D.Impulse);
        }
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().DamagePlayer(10);
        }
    }
}
