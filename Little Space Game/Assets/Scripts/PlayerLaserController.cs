using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaserController : MonoBehaviour
{
    GameObject Player;
    float smooth = 0f;
    void Start()
    {
         Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Player.transform.position;
        smooth += 187.5f;
        transform.rotation = Quaternion.Euler(0, 0, smooth);
    }
}
