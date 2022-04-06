using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrailEffectController : MonoBehaviour
{
    GameObject Player;
    bool isFollowing;
    private void Start()
    {
        isFollowing = true;
        StartCoroutine(Stop());
    }
    void Update()
    {
        if (isFollowing)
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            transform.position = Player.transform.position;
        }
    }
    IEnumerator Stop()
    {
        yield return new WaitForSeconds(0.35f);
        isFollowing = false;
    }
}
