using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerAf1Sec : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyAf1Sec());
    }

    IEnumerator DestroyAf1Sec()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
