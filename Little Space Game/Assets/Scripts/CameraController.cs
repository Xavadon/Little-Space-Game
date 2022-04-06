using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Transform Player;
    [SerializeField] float threshold;
    private void Update()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetPos = (Player.position + mousePos) / 2f;

        targetPos.x = Mathf.Clamp(targetPos.x, -threshold + Player.position.x, threshold + Player.position.x);
        targetPos.y = Mathf.Clamp(targetPos.y, -threshold + Player.position.y, threshold + Player.position.y);

        this.transform.position = targetPos;
    }
}
