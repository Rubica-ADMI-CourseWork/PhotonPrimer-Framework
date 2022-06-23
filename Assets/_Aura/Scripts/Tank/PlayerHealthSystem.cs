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
    public void TakeDamage(float damageAmnt, int actorNo)
    {
        var fx = PhotonNetwork.Instantiate(hitFX.name, transform.position, Quaternion.identity);
        Destroy(fx, .2f);
        Debug.Log("Inside Take Damage");
        maxhealth -= damageAmnt;
        if (maxhealth <= 0)
        {
            Die(actorNo);
        }
    }



    private void Die(int no)
    {
        AudioManager.Instance.PlayTankXplosionFX();
        MatchManager.Instance.SendUpdateStatsEvent(PhotonNetwork.LocalPlayer.ActorNumber, StatType.DeatCount, 1);
        SpawnManager.Instance.Die();
        var fx = PhotonNetwork.Instantiate(destructionFX.name, transform.position, Quaternion.identity);
        Destroy(gameObject);
        Destroy(fx, .3f);
    }
}
