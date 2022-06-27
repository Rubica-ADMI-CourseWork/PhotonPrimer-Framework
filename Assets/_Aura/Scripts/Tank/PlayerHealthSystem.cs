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

    private void Start()
    {
        maxhealth = 50;
    }



    private void OnCollisionEnter(Collision collision)
    {
        //check if it is shell
        if (collision.gameObject.CompareTag("Shell"))
        {

            if (photonView.IsMine)
            {
               TakeDamage(collision.gameObject.GetComponent<ShellInfo>().playerNo, 1, 1);
               
            }
        }
    }



    public void TakeDamage( int actorNo, int stat,int statAmout)
    {
        var fx = PhotonNetwork.Instantiate(hitFX.name, transform.position, Quaternion.identity);
        maxhealth -= 10;
        if (maxhealth <= 0)
        {
            PlayKillFX();
            SpawnManager.Instance.Die(actorNo,stat,statAmout);
        }

    }
    private void PlayKillFX()
    {
        AudioManager.Instance.PlayTankXplosionFX();
        var fx = PhotonNetwork.Instantiate(destructionFX.name, transform.position, Quaternion.identity);
    }
}
