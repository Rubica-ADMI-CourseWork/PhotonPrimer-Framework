using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class VfxDestroyer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(HandleVFXDestruction());
    }

    IEnumerator HandleVFXDestruction()
    {
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }

}
