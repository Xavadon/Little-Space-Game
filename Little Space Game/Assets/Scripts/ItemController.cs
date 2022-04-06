using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    [SerializeField] GameObject coinSound;
    [SerializeField] bool coin;
    [SerializeField] bool heal;
    [SerializeField] bool dmg;
    [SerializeField] bool bombChargeItem;
    [SerializeField] bool bombSpeedChargeItem;
    [SerializeField] bool speedUp;
    [SerializeField] bool speedAttck;
    [SerializeField] bool SpecialItemLaser;
    [SerializeField] bool SpecialItemBomb;
    [SerializeField] bool SpecialItemUpgradedBomb;
    bool isFollowingPlayer = false;
    GameObject Player;
    float smooth = 0f;

    void FixedUpdate()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        if (Vector3.Distance(transform.position, Player.transform.position) <= 2f || isFollowingPlayer)
        {
            isFollowingPlayer = true;
            Vector3 target = Player.transform.position - transform.position;
            GetComponent<Rigidbody2D>().AddForce(target.normalized * 1f * Player.GetComponent<PlayerController>().speed / 30, ForceMode2D.Impulse);
        }
        if (SpecialItemUpgradedBomb || SpecialItemBomb || SpecialItemLaser)
        {
            smooth += 2.5f;
            transform.rotation = Quaternion.Euler(0, 0, smooth);
        }
    }    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Instantiate(coinSound, transform.position, Quaternion.identity);
            Destroy(gameObject);
            if (coin)
            {
                Player.GetComponent<PlayerController>().coins += 1;
            }
            if (heal)
            {
                Player.GetComponent<PlayerController>().HealPlayer(20);
            }
            if (dmg)
            {
                Player.GetComponent<PlayerController>().damage += 10;
            }
            if (bombChargeItem)
            {
                Player.GetComponent<PlayerController>().maxBombCharges += 1;
            }
            if (bombSpeedChargeItem)
            {
                float bombDelay = Player.GetComponent<PlayerController>().bombDelay;
                if (bombDelay <= 1f)
                {
                    Player.GetComponent<PlayerController>().bombDelay -= bombDelay * 0.1f;
                }
                else
                {
                    Player.GetComponent<PlayerController>().bombDelay -= 0.5f;
                }
            }
            if (speedUp)
            {
                Player.GetComponent<PlayerController>().speed += 3.5f;
            }
            if (speedAttck)
            {
                Player.GetComponent<PlayerController>().fireDelay -= 0.030f;
            }
            if (SpecialItemLaser)
            {
                Player.GetComponent<PlayerController>().haveLasers = true;
            }
            if (SpecialItemBomb)
            {
                Player.GetComponent<PlayerController>().haveBomb = true;
            }
            if (SpecialItemUpgradedBomb)
            {
                Player.GetComponent<PlayerController>().haveUpgradedBomb = true;
            }
        }
    }
}
