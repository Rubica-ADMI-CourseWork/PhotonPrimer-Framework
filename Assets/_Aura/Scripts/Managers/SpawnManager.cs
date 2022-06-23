
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnManager : MonoBehaviourPun
{
    [SerializeField] GameObject playerPrefab;
    
    [SerializeField]float minXBounds,maxXbounds,minZBounds,maxZBounds,yPos;

    public static SpawnManager Instance;

    private GameObject newPlayer;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if(PhotonNetwork.IsConnected)
        SpawnPlayerAtRandomPos();
    }

    public void Die()
    {
        Destroy(gameObject);
    }
    public void SpawnPlayerAtRandomPos()
    {
        Vector3 randomPos = new Vector3(
            Random.Range(minXBounds, maxXbounds), 
            yPos, 
            Random.Range(minZBounds, maxZBounds));

        newPlayer = PhotonNetwork.Instantiate(playerPrefab.name, 
            randomPos, 
            Quaternion.identity);
    }

    public void RespawnPlayer()
    {
        StartCoroutine(HandleRespawnWithDelay());
    }

    private IEnumerator HandleRespawnWithDelay()
    {
        yield return new WaitForSeconds(2f);
        SpawnPlayerAtRandomPos();
    }
}
