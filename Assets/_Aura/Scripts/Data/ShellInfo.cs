using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;

/// <summary>
/// Contains info of which player fired this shell
/// to control for the scenario where multiple players are shooting
/// at the tank, each time a shell strikes the shooter name is updated on teh 
/// target player.
/// </summary>
public class ShellInfo : MonoBehaviourPun,IOnEventCallback
{
    public string ShooterName;
    public int playerNo;

    private void OnEnable()
    {
        playerNo = photonView.ControllerActorNr;
    }
    private void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
       
    }

    internal void HandleKilling()
    {
        
    }
}
