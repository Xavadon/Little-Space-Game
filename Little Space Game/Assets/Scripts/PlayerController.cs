using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    int hp = 100;
    int maxHp = 100;
    public int damage = 20;
    public int maxBombCharges;
    public int bombCharges;
    public int coins;
    [SerializeField] GameObject HaveDmgSound;
    [SerializeField] GameObject bulletSpawnPoint;

    //Items
    public bool haveBomb;
    public bool haveUpgradedBomb;
    public bool haveLasers;

    Rigidbody2D rb;
    public float speed = 35f;
    Vector2 direction;
    bool isDashing = false;
    bool dashCooldown = false;

    bool isRotating = false;
    bool rotateCooldown = false;
    [SerializeField] GameObject Lasers;
    [SerializeField] GameObject LasersSound;
    [SerializeField] GameObject LasersEffect;

    bool isBombCooldown = false;

    [SerializeField] GameObject DashHitCollider;
    [SerializeField] GameObject DashStartEffect;
    [SerializeField] GameObject DashSoundEffect;

    [SerializeField] Camera cam;
    Vector3 difference;
    public GameObject bulletPrefab;
    public GameObject bombPrefab;
    public GameObject upgradedBombPrefab;
    public float fireDelay = 0.3f;
    float lastFire;
    public float bombDelay;

    SpriteRenderer SpriteRend;
    Material matBlink;
    Material matDefault;

    [SerializeField] Text TextUI;

    [SerializeField] GameObject DeathEffect;
    [SerializeField] GameObject DeathSound;

    void Start()
    {
        maxBombCharges = 1;

        SpriteRend = GetComponent<SpriteRenderer>();
        matBlink = Resources.Load("BlinkMaterial", typeof(Material)) as Material;
        matDefault = SpriteRend.material;

        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        //Движение в fixedupdate
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        direction = moveInput.normalized;

        //Слежение за курсором
        difference = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference = new Vector2(difference.x, difference.y);
        float rotation = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotation - 90f); //-90 Потому что спрайт смотрит вверх а не вправо

        //Выстрел
        if (Input.GetKey(KeyCode.Mouse0) && Time.time > lastFire + fireDelay && !isRotating)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.transform.position, transform.rotation) as GameObject;
            bullet.GetComponent<BulletController>().isPlayerBullet = true;
            bullet.GetComponent<BulletController>().damage = damage;
            bullet.GetComponent<Rigidbody2D>().AddForce(difference.normalized * speed * 20 * Time.fixedDeltaTime, ForceMode2D.Impulse);
            lastFire = Time.time;
        }
        //Бомба
        if (Input.GetKey(KeyCode.Mouse1) && bombCharges > 0 && Time.time > lastFire + fireDelay && haveBomb && !isRotating)
        {
            lastFire = Time.time;
            bombCharges--;
            GameObject bomb;
            if (haveUpgradedBomb)
            {
                bomb = Instantiate(upgradedBombPrefab, bulletSpawnPoint.transform.position, transform.rotation) as GameObject;
            }
            else
            {
                bomb = Instantiate(bombPrefab, bulletSpawnPoint.transform.position, transform.rotation) as GameObject;
            }
            bomb.GetComponent<Rigidbody2D>().AddForce(difference.normalized * speed * 20 * Time.fixedDeltaTime, ForceMode2D.Impulse);
            bomb.GetComponent<BombController>().damage = damage / 2;
            bomb.GetComponent<BombController>().isPlayerBomb = true;
        }
        if(bombCharges < maxBombCharges && !isBombCooldown)
        {
            isBombCooldown = true;
            Debug.Log(bombDelay);
            StartCoroutine(BombRecharge(bombDelay));
        }
        //Интерфейс
        UpdateUI();
    }
    void FixedUpdate()
    {
        if (!isDashing)
        {
            rb.AddForce(direction * speed * 0.7f, ForceMode2D.Force);
            //rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
        }
        if (Input.GetKey(KeyCode.Space) && !isDashing && !dashCooldown)
        {
            //Удар в рывке
            if (!isRotating)
            {
                DashHitCollider.SetActive(true);
            }
            DashHitCollider.GetComponent<DashHitColliderController>().damage = damage / 2;

            isDashing = true;
            dashCooldown = true;

            rb.drag = 0;
            rb.velocity = Vector3.zero;

            Instantiate(DashSoundEffect, transform.position, Quaternion.identity);
            Instantiate(DashStartEffect, transform.position, Quaternion.identity);
            rb.AddForce(difference.normalized * speed * 0.5f, ForceMode2D.Impulse); //Рывок получается? 

            StartCoroutine(DashCooldown());
        }
        if (Input.GetKey(KeyCode.Q) && !isRotating && !rotateCooldown && haveLasers)
        {
            isRotating = true;
            rotateCooldown = true;

            DashHitCollider.SetActive(true);
            Lasers.SetActive(true);

            StartCoroutine(RotationCooldown());
        }
    }
    public void reduceCoins(int red)
    {
        coins -= red;
        if (coins <= 0)
        {
            coins = 0;
        }
    }
    void UpdateUI()
    {
        TextUI.text = "HP: " + hp + " dmg: " + damage +"\r\nBomb charges: " + bombCharges + "\r\nCoins: " + coins;
    }
    public void HealPlayer(int heal)
    {
        hp += heal;
        if (hp > maxHp)
        {
            maxHp = hp;
        }
    }
    public void DamagePlayer(int damage)
    {
        if (!isDashing && !isRotating)
        {
            Instantiate(HaveDmgSound, transform.position, Quaternion.identity);
            hp -= damage;
            if (hp <= 0)
            {
                StartCoroutine(Death());
            }
            else
            {
                Blink();
            }
        } 
    }
    public void KillPlayer()
    {
        Instantiate(DeathEffect, transform.position, Quaternion.identity);
        Instantiate(DeathSound, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    IEnumerator Death()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene("GameOver");
        transform.position = new Vector2(0, 0);
        yield return new WaitForSeconds(3f);
        Instantiate(DeathEffect, transform.position, Quaternion.identity);
        Instantiate(DeathSound, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    public void Win()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene("GameWin");
        transform.position = new Vector2(0, 0);
    }
    void Blink()
    {
        SpriteRend.material = matBlink;
        Invoke("ResetMaterial", 0.2f);
    }
    void ResetMaterial()
    {
        SpriteRend.material = matDefault;
    }
    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(0.35f);
        isDashing = false;
        if (!isRotating)
        {
            DashHitCollider.SetActive(false);
        }
        rb.drag = 3;
        yield return new WaitForSeconds(1f);
        dashCooldown = false;
    }
    IEnumerator RotationCooldown()
    {
        yield return new WaitForSeconds(7f);
        isRotating = false;
        DashHitCollider.SetActive(false);
        Lasers.SetActive(false);
        Instantiate(LasersSound, transform.position, Quaternion.identity);
        Instantiate(LasersEffect, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(4f);
        rotateCooldown = false;
    }
    IEnumerator BombRecharge(float Delay)
    {
        yield return new WaitForSeconds(Delay);
        isBombCooldown = false;
        bombCharges++;
    }
}
