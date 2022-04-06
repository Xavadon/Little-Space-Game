using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    GameObject Player;
    Rigidbody2D rb;
    public int damage;
    [SerializeField] GameObject RocketStartEffect;
    [SerializeField] GameObject BulletEXPL;
    [SerializeField] GameObject ShootSound;
    [SerializeField] GameObject EXPLSound;
    void Start()
    {
        Instantiate(RocketStartEffect, transform.position, Quaternion.identity);
        Instantiate(ShootSound, transform.position, Quaternion.identity);
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(DestroyRocket());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        Vector3 target = Player.transform.position - transform.position;
        float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
        GetComponent<Rigidbody2D>().AddForce(target.normalized * 1f, ForceMode2D.Impulse);
        rb.AddForce(target.normalized * 1.2f, ForceMode2D.Impulse);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag != "Bullet" && collision.tag != "Enemy" && collision.tag != "EnemyTurrel" && collision.tag != "Room" && collision.tag != "Item")
        {
            DestroyBullet();
        }
        if(collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().DamagePlayer(15 + damage);
        }
    }
    void DestroyBullet()
    {
        Instantiate(BulletEXPL, transform.position, Quaternion.identity);
        Instantiate(EXPLSound, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    IEnumerator DestroyRocket()
    {
        yield return new WaitForSeconds(1f);
        DestroyBullet();
    }
}
