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
            Debug.Log("from shell Behaviour " + GetComponent<ShellInfo>().playerNo);
            collision.gameObject.GetPhotonView().RPC("TakeDamage", RpcTarget.All, 10f,GetComponent<ShellInfo>().playerNo,GetComponent<ShellInfo>().ShooterName);
        }
    }

    private void HandleShellDestruction()
    {
        var shellFX =PhotonNetwork.Instantiate(shellVFX.name,transform.position,Quaternion.identity);
        Destroy(gameObject);
        Destroy(shellFX);
    }
}
