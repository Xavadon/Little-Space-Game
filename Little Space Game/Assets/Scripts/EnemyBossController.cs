using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossController : MonoBehaviour
{
    int hp = 5000;
    int damage = 10;
    int attackType;

    float lastFire;
    float fireDelay = 0.1f;

    Vector3 target;
    Vector3 target1;
    Vector3 target2;

    GameObject Player;
    [SerializeField] GameObject SpawnPoint1;
    [SerializeField] GameObject SpawnPoint2;
    [SerializeField] GameObject SpawnEnemy1;
    [SerializeField] GameObject SpawnEnemy2;
    [SerializeField] GameObject BlueEnemyPrefab;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] GameObject rocketPrefab;
    [SerializeField] GameObject bombPrefab;
    [SerializeField] GameObject Walls;

    [SerializeField] GameObject DestroyEffect;
    [SerializeField] GameObject DestroySound;
    [SerializeField] GameObject DestroySpawnPoint1;
    [SerializeField] GameObject DestroySpawnPoint2;
    [SerializeField] GameObject DestroySpawnPoint3;

    public bool state_1;
    bool state_2;
    bool isRecharging;
    bool changeAttackType = true;
    bool isWhite;
    bool isDying;

    SpriteRenderer SpriteRend;
    Material matBlink;
    Material matDefault;
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
    private void Start()
    {
        SpriteRend = GetComponent<SpriteRenderer>();
        matBlink = Resources.Load("BlinkMaterial", typeof(Material)) as Material;
        matDefault = SpriteRend.material;

        Player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {
        Debug.Log(hp);
        if (!isDying)
        {
            if (changeAttackType && state_1)
            {
                StartCoroutine(ChangeAttackType());
                StartCoroutine(SpawnBlueEnemies());
            }
            if (state_1)
            {
                if (!isWhite)
                {
                    target = Player.transform.position - transform.position;
                    float angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(0f, 0f, angle + 90);
                }

                target1 = Player.transform.position - SpawnPoint1.transform.position;
                target2 = Player.transform.position - SpawnPoint2.transform.position;
                if (attackType == 1 && Vector3.Distance(transform.position, Player.transform.position) <= 13f && Time.time > lastFire + fireDelay)
                {
                    GameObject bullet1 = Instantiate(bulletPrefab, SpawnPoint1.transform.position, transform.rotation) as GameObject;
                    target1 = new Vector2(target1.x, target1.y);
                    bullet1.GetComponent<BulletController>().isEnemyBullet = true;
                    bullet1.GetComponent<BulletController>().damage = 5;
                    bullet1.GetComponent<Rigidbody2D>().AddForce(target1.normalized * 35f * 30f * Time.fixedDeltaTime, ForceMode2D.Impulse);
                    bullet1.transform.localScale = new Vector2(2, 2);
                    GameObject bullet2 = Instantiate(bulletPrefab, SpawnPoint2.transform.position, transform.rotation) as GameObject;
                    target2 = new Vector2(target2.x, target2.y);
                    bullet2.GetComponent<BulletController>().isEnemyBullet = true;
                    bullet2.GetComponent<BulletController>().damage = 5;
                    bullet2.transform.localScale = new Vector2(2, 2);
                    bullet2.GetComponent<Rigidbody2D>().AddForce(target2.normalized * 35f * 30f * Time.fixedDeltaTime, ForceMode2D.Impulse);
                    lastFire = Time.time;
                }
                if (attackType == 2 && Vector3.Distance(transform.position, Player.transform.position) <= 13f && !isRecharging)
                {
                    GameObject bomb1 = Instantiate(bombPrefab, SpawnPoint1.transform.position, transform.rotation) as GameObject;
                    bomb1.GetComponent<Rigidbody2D>().AddForce(target1.normalized * 35f * 10f * Time.fixedDeltaTime, ForceMode2D.Impulse);
                    bomb1.GetComponent<BombController>().damage = 25; //original 100
                    bomb1.GetComponent<BombController>().isEnemyBomb = true;
                    GameObject bomb2 = Instantiate(bombPrefab, SpawnPoint2.transform.position, transform.rotation) as GameObject;
                    bomb2.GetComponent<Rigidbody2D>().AddForce(target.normalized * 35f * 10f * Time.fixedDeltaTime, ForceMode2D.Impulse);
                    bomb2.GetComponent<BombController>().damage = 25; // original 100
                    bomb2.GetComponent<BombController>().isEnemyBomb = true;
                    isRecharging = true;
                    StartCoroutine(Recharge(1f));
                }
                if (attackType == 3 && Vector3.Distance(transform.position, Player.transform.position) <= 13f && !isRecharging)
                {
                    GameObject rocket1 = Instantiate(rocketPrefab, SpawnPoint1.transform.position, transform.rotation) as GameObject;
                    rocket1.transform.localScale = new Vector2(2, 2);
                    rocket1.GetComponent<RocketController>().damage = 35; //Original 75
                    GameObject rocket2 = Instantiate(rocketPrefab, SpawnPoint2.transform.position, transform.rotation) as GameObject;
                    rocket2.transform.localScale = new Vector2(2, 2);
                    rocket2.GetComponent<RocketController>().damage = 35;
                    isRecharging = true;
                    StartCoroutine(Recharge(0.7f));
                }
            }
            if (state_2)
            {

            }
        }
    }
    IEnumerator ChangeAttackType()
    {
        changeAttackType = false;
        attackType = 0;
        Blink(4f);
        isWhite = true;
        yield return new WaitForSeconds(4f);
        attackType = Random.Range(1, 4);
        isWhite = false;
        yield return new WaitForSeconds(7);
        changeAttackType = true;
    }
    IEnumerator SpawnBlueEnemies()
    {
        Instantiate(BlueEnemyPrefab, SpawnEnemy1.transform.position, Quaternion.identity);
        Instantiate(BlueEnemyPrefab, SpawnEnemy2.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(1f);
        Instantiate(BlueEnemyPrefab, SpawnEnemy1.transform.position, Quaternion.identity);
        Instantiate(BlueEnemyPrefab, SpawnEnemy2.transform.position, Quaternion.identity);
    }
    IEnumerator Recharge(float Delay)
    {
        yield return new WaitForSeconds(Delay);
        isRecharging = false;
    }
    public void DamageEnemy(int damage)
    {
        if ((state_1 || state_2))
        {
            hp -= damage;
            if (hp <= 0)
            {
                isDying = true;
                StartCoroutine(DestroyBoss());
                Walls.SetActive(false);
            }
            else
            {
                Blink(0.2f);
            }
        }
    }
    bool canTakeDamage = true;
    public void DamageEnemyLaser(int damage)
    {
        if ((state_1 || state_2) && canTakeDamage)
        {
            hp -= damage;
            if (hp <= 0)
            {
                isDying = true;
                StartCoroutine(DestroyBoss());
                Walls.SetActive(false);
            }
            else
            {
                Blink(0.2f);
            }
        }
    }
    void Blink(float Delay)
    {
        canTakeDamage = false;
        SpriteRend.material = matBlink;
        Invoke("ResetMaterial", Delay);
    }
    void ResetMaterial()
    {
        canTakeDamage = true;
        SpriteRend.material = matDefault;
    }
    IEnumerator DestroyBoss()
    {
        Instantiate(DestroyEffect, DestroySpawnPoint1.transform.position, Quaternion.identity);
        Instantiate(DestroySound, DestroySpawnPoint1.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.3f);
        Instantiate(DestroyEffect, DestroySpawnPoint1.transform.position, Quaternion.identity);
        Instantiate(DestroySound, DestroySpawnPoint1.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.3f);
        Instantiate(DestroyEffect, DestroySpawnPoint1.transform.position, Quaternion.identity);
        Instantiate(DestroySound, DestroySpawnPoint1.transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
        GameObject Destr = Instantiate(DestroyEffect, transform.position, Quaternion.identity);
        Destr.transform.localScale = new Vector2(2, 2);
    }
}
