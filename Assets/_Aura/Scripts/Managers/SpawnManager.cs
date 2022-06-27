
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnManager : MonoBehaviourPun
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] float minXBounds, maxXbounds, minZBounds, maxZBounds, yPos;


    //store the player brought in.
    private GameObject newPlayer;

    public static SpawnManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            SpawnPlayerAtRandomPos();
        }
    }

    #region Spawn Death and Respawn Handling
    public void SpawnPlayerAtRandomPos()
    {
        Vector3 randomPos = CalculateRandomPos();

        newPlayer = PhotonNetwork.Instantiate(playerPrefab.name,
            randomPos,
            Quaternion.identity);
    }
    public void Die(int playerNo,int stat,int statAmound)
    {
        PhotonNetwork.Destroy(newPlayer);
        MatchManager.Instance.SendUpdateKillStatsEvent(playerNo, stat, statAmound);
        StartCoroutine(HandleDeath(playerNo,stat,statAmound));
    }
    private IEnumerator HandleDeath(int playerNo,int stat,int statAmount)
    {
        
        yield return new WaitForSeconds(.1f);



        SpawnPlayerAtRandomPos();

    }
    #endregion


    #region Utility Methods
    private Vector3 CalculateRandomPos()
    {
        return new Vector3(
            Random.Range(minXBounds, maxXbounds),
            yPos,
            Random.Range(minZBounds, maxZBounds));
    }
    #endregion
}
