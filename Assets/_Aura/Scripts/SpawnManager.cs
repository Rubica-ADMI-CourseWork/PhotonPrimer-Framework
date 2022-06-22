
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnManager : MonoBehaviourPun
{
    [SerializeField] GameObject playerPrefab;
    
    [SerializeField]float minXBounds,maxXbounds,minZBounds,maxZBounds,yPos;
  
    private void Start()
    {
        SpawnPlayerAtRandomPos();
    }

    private void SpawnPlayerAtRandomPos()
    {
        Vector3 randomPos = new Vector3(
            Random.Range(minXBounds, maxXbounds), 
            yPos, 
            Random.Range(minZBounds, maxZBounds));

        var newPlayer = PhotonNetwork.Instantiate(playerPrefab.name, randomPos, Quaternion.identity);
    }
}
