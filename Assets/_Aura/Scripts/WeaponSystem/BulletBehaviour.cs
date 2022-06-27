using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class BulletBehaviour : MonoBehaviourPun
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Bullet landed.");
            PhotonNetwork.Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            
            PhotonNetwork.Destroy(gameObject);

        }
    }
}
