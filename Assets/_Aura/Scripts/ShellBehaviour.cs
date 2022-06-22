using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ShellBehaviour : MonoBehaviourPun //shell data to be scriptable object
{
    public float timeToDestruction;
    public float timeToVfxDestruction;
    public GameObject shellVFX;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            HandleShellDestruction();
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, 10f);
        }
    }

    private void HandleShellDestruction()
    {
        var shellFX =PhotonNetwork.Instantiate(shellVFX.name,transform.position,Quaternion.identity);
        Destroy(gameObject, timeToDestruction);
        Destroy(shellFX,timeToVfxDestruction);
    }
}
