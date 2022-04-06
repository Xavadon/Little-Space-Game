using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    [SerializeField] int cost;
    [SerializeField] Sprite chestUnlocked;
    private SpriteRenderer spriteRend;
    bool spawned;

    [SerializeField] GameObject ItemPrefab;
    [SerializeField] GameObject HealItemPrefab;

    [SerializeField] GameObject DestroyEffect;
    [SerializeField] GameObject DeathSound;
    private void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
    }
    private void OnTriggerEnter2D(Collider2D colision)
    {
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        int coins = Player.GetComponent<PlayerController>().coins;
        if (colision.GetComponent<BulletController>().isPlayerBullet && coins >= cost && !spawned)
        {
            spawned = true;
            Player.GetComponent<PlayerController>().reduceCoins(cost);
            spriteRend.sprite = chestUnlocked;

            GameObject item = Instantiate(ItemPrefab, transform.position, Quaternion.identity);
            Vector3 randomDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            item.GetComponent<Rigidbody2D>().AddForce(randomDir * 2f, ForceMode2D.Impulse);

            GameObject itemHeal = Instantiate(HealItemPrefab, transform.position, Quaternion.identity);
            randomDir = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
            itemHeal.GetComponent<Rigidbody2D>().AddForce(randomDir * 2f, ForceMode2D.Impulse);

            StartCoroutine(DestroyChest());
        }
    }
    IEnumerator DestroyChest()
    {
        yield return new WaitForSeconds(3f);
        Instantiate(DestroyEffect, transform.position, Quaternion.identity);
        Instantiate(DeathSound, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
