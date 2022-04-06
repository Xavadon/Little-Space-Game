using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossHairController : MonoBehaviour
{
    [SerializeField] Camera MainCamera;
    private void Start()
    {
        Cursor.visible = false;
    }
    private void Update()
    {
        Vector3 CrossHairPos = new Vector3(MainCamera.ScreenToWorldPoint(Input.mousePosition).x, MainCamera.ScreenToWorldPoint(Input.mousePosition).y);
        transform.position = CrossHairPos;
    }
}
