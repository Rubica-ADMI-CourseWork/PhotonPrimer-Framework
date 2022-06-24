using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System;

public enum EventCodes : byte
{
    UpdateStats,
    ListAllPlayers,
    AddNewPlayer
}
public class MatchManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public GameObject leaderBoardPanel;
    public GameObject leaderBoardItemPrefab;

    public static MatchManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }
    #region Member Fields

    private List<GameObject> leaderBoardItemObjects = new List<GameObject>();
    [SerializeField] private List<PlayerInfo> playerInfoList = new List<PlayerInfo>();
    private int index;

    #endregion

    #region Unity Callbacks
    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            SendAddNewPlayerEvent(PhotonNetwork.NickName);
        }
        else if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene(0);
        }
    }

    private void UpdateLeaderBoard()
    {
        Debug.Log("Inside update leader board!");
        foreach (var obj in leaderBoardItemObjects)
        {
            Destroy(obj);
        }

        leaderBoardItemObjects.Clear();

        //create a leaderboard item for each 
        foreach (var info in playerInfoList)
        {
            var item = Instantiate(leaderBoardItemPrefab);
            item.GetComponent<LeaderboardItem>().UpdateItem(info.playerName, info.crewAmount, info.killCount);
            item.transform.SetParent(leaderBoardPanel.transform, false);

            leaderBoardItemObjects.Add(item);
           
        }
    }

    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    #endregion

    #region IOnEventCallback Implementation
    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code < 200)
        {
            EventCodes inComingCode = (EventCodes)photonEvent.Code;
            var inComingData = (object[])photonEvent.CustomData;

            Debug.Log("received event: " + inComingCode);

            switch (inComingCode)
            {
                case EventCodes.UpdateStats:
                    ReceiveUpdateStatsEvent(inComingData);
                    break;
                case EventCodes.ListAllPlayers:
                    ReceiveListAllPlayersEvent(inComingData);
                    break;
                case EventCodes.AddNewPlayer:
                    ReceiveAddNewPlayerEvent(inComingData);
                    break;
            }
        }

    }
    #endregion

    #region Event Management
    public void SendUpdateStatsEvent(int sender, byte statToUpdate, int statAmount)
    {
        //Debug.Log("sending stats of : " + sender + statToUpdate + statAmount);

        object[] statsInfo = new object[] { sender, statToUpdate, statAmount };

        PhotonNetwork.RaiseEvent(
           (byte)EventCodes.UpdateStats,
           statsInfo,
           new RaiseEventOptions { Receivers = ReceiverGroup.All },
           new SendOptions { Reliability = true });
    }
    public void ReceiveUpdateStatsEvent(object[] inComingData)
    {
        //cache incoming values
        int actor = (int)inComingData[0];
        int statType = (byte)inComingData[1];
        int statAmount = (int)inComingData[2];

        Debug.Log("Receiving stats of : " + actor + statType + statAmount);

        //loop through list updating the relevant player based on actor number
        for (int i = 0; i < playerInfoList.Count; i++)
        {
            if (playerInfoList[i].actorNo == actor)
            {
                switch (statType)
                {
                    //crew amount
                    case 0:
                        playerInfoList[i].crewAmount += statAmount;
                        break;

                    case 1://kill count
                        playerInfoList[i].killCount += statAmount;
                        break;

                    case 2://death count
                        playerInfoList[i].deathCount += statAmount;
                        break;
                }
                if(i == index)
                {
                    UpdateLeaderBoard();
                }
                break;
            }
        }

        //SendListAllPlayersEvent();

    }
    public void SendListAllPlayersEvent()
    {
        object[] outgoingPackage = new object[playerInfoList.Count];

        for (int i = 0; i < outgoingPackage.Length; i++)
        {
            object[] dataPiece = new object[5];
            dataPiece[0] = playerInfoList[i].playerName;
            dataPiece[1] = playerInfoList[i].actorNo;
            dataPiece[2] = playerInfoList[i].crewAmount;
            dataPiece[3] = playerInfoList[i].killCount;
            dataPiece[4] = playerInfoList[i].deathCount;

            outgoingPackage[i] = dataPiece;
        }

        PhotonNetwork.RaiseEvent(
           (byte)EventCodes.ListAllPlayers,
           outgoingPackage,
           new RaiseEventOptions { Receivers = ReceiverGroup.All },
           new SendOptions { Reliability = true });
    }
    public void ReceiveListAllPlayersEvent(object[] inComingData)
    {
        //reconstitute
        playerInfoList.Clear();
        for (int i = 0; i < inComingData.Length; i++)
        {
            object[] inComingDataPiece = (object[])inComingData[i];
            PlayerInfo info = new PlayerInfo((string)inComingDataPiece[0],
               (int)inComingDataPiece[1],
               (int)inComingDataPiece[2],
               (int)inComingDataPiece[3],
               (int)inComingDataPiece[4]);

            playerInfoList.Add(info);

            if (PhotonNetwork.LocalPlayer.ActorNumber == info.actorNo)
            {
                index = i;
            }
        }
       
       UpdateLeaderBoard();
    }
    public void SendAddNewPlayerEvent(string playerName)
    {
        object[] package = new object[5];
        package[0] = playerName;
        package[1] = PhotonNetwork.LocalPlayer.ActorNumber;
        package[2] = 0;
        package[3] = 0;
        package[4] = 0;

        PhotonNetwork.RaiseEvent(
            (byte)EventCodes.AddNewPlayer,
            package,
            new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient },
            new SendOptions { Reliability = true });
    }
    public void ReceiveAddNewPlayerEvent(object[] inComingData)
    {
        PlayerInfo info = new PlayerInfo((string)inComingData[0],
            (int)inComingData[1],
            (int)inComingData[2],
            (int)inComingData[3],
            (int)inComingData[4]);

        playerInfoList.Add(info);

        SendListAllPlayersEvent();
    }
    #endregion

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("Switched MasterClients to " + newMasterClient.NickName);
    }
}
