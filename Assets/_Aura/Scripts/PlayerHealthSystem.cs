using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerHealthSystem : MonoBehaviourPun
{
    [SerializeField] float maxhealth;
    [SerializeField] GameObject destructionFX;
    [SerializeField] GameObject hitFX;

    [PunRPC]
    public void TakeDamage(float damageAmnt)
    {
        var fx = PhotonNetwork.Instantiate(hitFX.name, transform.position, Quaternion.identity);
        Destroy(fx, .2f);
        Debug.Log("Inside Take Damage");
        maxhealth -= damageAmnt;
        if (maxhealth <= 0)
        {
            if (!photonView.IsMine)
            {
               photonView.RPC("SpewCrew",RpcTarget.All);
               photonView.RPC("Die",RpcTarget.All);
            } 
        }
    }


    [PunRPC]
    private void Die()
    {
        if(photonView.IsMine)
        FindObjectOfType<SpawnManager>().RespawnPlayer();
        var fx = PhotonNetwork.Instantiate(destructionFX.name, transform.position, Quaternion.identity);
        Destroy(gameObject);
        Destroy(fx, .3f);

        
    }
}
