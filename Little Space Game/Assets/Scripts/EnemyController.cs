using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    int hp;
    int damage;
    int coins;

    [SerializeField] GameObject DestroyEffect;
    [SerializeField] GameObject DeathSound;

    [SerializeField] bool Range;
    [SerializeField] bool Bomb;
    [SerializeField] bool Rocket;

    GameObject Player;
    [SerializeField] GameObject coinPrefab;

    Vector2 target;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject bombPrefab;
    [SerializeField] GameObject bombSpawnPoint;
    [SerializeField] GameObject rocketPrefab;
    int bombCharges;
    bool isBombCooldown;
    float lastFire;
    float fireDelay = 0.3f;

    SpriteRenderer SpriteRend;
    Material matBlink;
    Material matDefault;
    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }
    void Start()
    {
        bombCharges = 1;

        //Для блинка
        SpriteRend = GetComponent<SpriteRenderer>();
        matBlink = Resources.Load("BlinkMaterial", typeof(Material)) as Material;
        matDefault = SpriteRend.material;

        if (Range)
        {
            hp = 100;
            damage = 20;
            coins = 5;
        }
        if (Bomb)
        {
            hp = 200;
            damage = 70;
            coins = 10;
        }
        if (Rocket)
        {
            hp = 120;
            damage = 20;
            coins = 15;
        }
        
    }
    void Update()
    {
        target = Player.transform.position - transform.position;
        if (Range && Vector3.Distance(transform.position, Player.transform.position) <= 7f && Time.time > lastFire + fireDelay)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation) as GameObject;
            target = new Vector2(target.x, target.y);
            bullet.GetComponent<BulletController>().isEnemyBullet = true;
            bullet.GetComponent<BulletController>().damage = 5;
            bullet.GetComponent<Rigidbody2D>().AddForce(target.normalized * 35f * 20f * Time.fixedDeltaTime, ForceMode2D.Impulse);
            lastFire = Time.time;
        }
        if (Bomb && Vector3.Distance(transform.position, Player.transform.position) <= 5f && !isBombCooldown)
        {
            GameObject bomb = Instantiate(bombPrefab, bombSpawnPoint.transform.position, transform.rotation) as GameObject;
            bomb.GetComponent<Rigidbody2D>().AddForce(target.normalized * 35f * 20f * Time.fixedDeltaTime, ForceMode2D.Impulse);
            bomb.GetComponent<BombController>().damage = 10; //Original damage / 2
            bomb.GetComponent<BombController>().isEnemyBomb = true;
            isBombCooldown = true;
            StartCoroutine(BombRecharge(4f));
        }
        if (Rocket && Vector3.Distance(transform.position, Player.transform.position) <= 13f && !isBombCooldown)
        {
            GameObject rocket = Instantiate(rocketPrefab, bombSpawnPoint.transform.position, transform.rotation) as GameObject;
            rocket.GetComponent<RocketController>().damage = 10; //Original damage / 2
            isBombCooldown = true;
            StartCoroutine(BombRecharge(2f));
        }
    }
    IEnumerator BombRecharge(float Delay)
    {
        yield return new WaitForSeconds(Delay);
        isBombCooldown = false;
        bombCharges++;
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

    bool isEnemyDead;
    private void EnemyDestroy()
    {
        if (!isEnemyDead)
        {
            isEnemyDead = true;
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
}
