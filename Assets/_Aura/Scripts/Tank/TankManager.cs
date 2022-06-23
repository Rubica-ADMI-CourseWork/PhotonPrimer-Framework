using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TankManager : MonoBehaviourPun
{
    [SerializeField] PlayerMovement movementComponent;
    [SerializeField] PlayerShoot shootComponent;

    private void Awake()
    {
        if (!photonView.IsMine)
        {
            movementComponent.enabled = false;
            shootComponent.enabled = false;
        }
        else
        {
            return;
        }
    }
}
