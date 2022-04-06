using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurrelController : MonoBehaviour
{
    int hp;
    int damage;
    int coins = 5;
    float lastFire;
    float fireDelay = 0.3f;
    [SerializeField] bool rocket;
    bool isRocketCooldown;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject DestroyEffect;
    [SerializeField] GameObject DeathSound;
    [SerializeField] GameObject RocketSpawnPont;

    GameObject Player;
    Vector2 target;

    SpriteRenderer SpriteRend;
    Material matBlink;
    Material matDefault;
    void Start()
    {
        hp = 150;
        damage = 20;

        SpriteRend = GetComponent<SpriteRenderer>();
        matBlink = Resources.Load("BlinkMaterial", typeof(Material)) as Material;
        matDefault = SpriteRend.material;

        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        target = Player.transform.position - transform.position;
        if ( Vector3.Distance(transform.position, Player.transform.position) <= 7f)
        {
            float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
            if (Time.time > lastFire + fireDelay && !rocket)
            {
                GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
                bullet.GetComponent<BulletController>().damage = 5; //Original damage;
                target = new Vector2(target.x, target.y);
                bullet.GetComponent<BulletController>().isEnemyBullet = true;
                bullet.GetComponent<Rigidbody2D>().AddForce(target.normalized * 35f * 20f * Time.fixedDeltaTime, ForceMode2D.Impulse);
                lastFire = Time.time;
            }
            if (!isRocketCooldown && rocket)
            {
                isRocketCooldown = true;
                GameObject rocket = Instantiate(bulletPrefab, RocketSpawnPont.transform.position, transform.rotation) as GameObject;
                rocket.GetComponent<RocketController>().damage = 10; //Original damage / 2;
                StartCoroutine(RocketCooldown());
            }
        }
    }
    public void DamageEnemy(int damage)
    {
        hp -= damage;
        if (hp <= 0)
        {
            EnemyDestroy();
        }
        else
        {
            Blink();
        }
    }
    bool canTakeDamage = true;
    public void DamageEnemyLaser(int damage)
    {
        if (canTakeDamage)
        {
            hp -= damage;
            if (hp <= 0)
            {
                EnemyDestroy();
            }
            else
            {
                Blink();
            }
        }
    }
    void Blink()
    {
        canTakeDamage = false;
        SpriteRend.material = matBlink;
        Invoke("ResetMaterial", 0.2f);
    }
    void ResetMaterial()
    {
        canTakeDamage = true;
        SpriteRend.material = matDefault;
    }
    IEnumerator RocketCooldown()
    {
        yield return new WaitForSeconds(2f);
        isRocketCooldown = false;
    }
    private void EnemyDestroy()
    {
        for (int i = 0; i < coins; i++)
        {
            GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            coin.GetComponent<Rigidbody2D>().AddForce(randomDir * Random.Range(2.5f, 4f), ForceMode2D.Impulse);
        }
        Instantiate(DestroyEffect, transform.position, Quaternion.identity);
        Instantiate(DeathSound, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
