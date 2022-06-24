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
    public void TakeDamage(float damageAmnt, int actorNo,string shooterName)
    {
      HandleDamage(damageAmnt, actorNo,shooterName);
    }

    public void HandleDamage(float damageAmnt, int actorNo,string shooter)
    {
        if (photonView.IsMine)
        {
            var fx = PhotonNetwork.Instantiate(hitFX.name, transform.position, Quaternion.identity);    
            maxhealth -= damageAmnt;
            if (maxhealth <= 0)
            {
                PlayKillFX();
                MatchManager.Instance.SendUpdateStatsEvent(actorNo, (byte)StatType.KillCount, 1);
                SpawnManager.Instance.Die();
            }
        }
    }

    private void PlayKillFX()
    {
        AudioManager.Instance.PlayTankXplosionFX();     
        var fx = PhotonNetwork.Instantiate(destructionFX.name, transform.position, Quaternion.identity);    
    }
}
