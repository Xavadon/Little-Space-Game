using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    float smooth = 0f;
    public float rotation;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerController>().Win();
        }
    }
    void Update()
    {
        smooth += rotation;
        transform.rotation = Quaternion.Euler(0, 0, smooth);
    }
}
