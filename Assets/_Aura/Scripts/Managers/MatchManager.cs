using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using System;
using System.Text;

public enum EventCodes : byte
{
    UpdateStats,
    ListAllPlayers,
    AddNewPlayer
}
public class MatchManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    [SerializeField]private List<PlayerInfo> allPlayersList = new List<PlayerInfo>();
    [SerializeField] Transform leaderBoardParent;
    [SerializeField] GameObject leaderBoardPrefab;
    private List<GameObject> leaderBoardItems = new List<GameObject>();

    private int myIndex;

    public static MatchManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    public override void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    private void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            SendNewPlayerEvent(PhotonNetwork.LocalPlayer.NickName);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code < 200)
        {
            var theEvent = (EventCodes)photonEvent.Code;
            var incomingData = (object[])photonEvent.CustomData;

            switch (theEvent)
            {
                case EventCodes.AddNewPlayer:
                    ReceiveNewPlayerEvent(incomingData);
                    break;
                case EventCodes.ListAllPlayers:
                    ReceiveListAllPlayersEvent(incomingData);
                    break;
                case EventCodes.UpdateStats:
                    ReceiveUpdateKillStatsEvent(incomingData);
                    break;
            }
        }
    }


    public void SendNewPlayerEvent(string playerName)
    {
        var info = new object[3];
        info[0] = playerName;
        info[1] = PhotonNetwork.LocalPlayer.ActorNumber;
        info[2] = 0;

        PhotonNetwork.RaiseEvent(
            (byte)EventCodes.AddNewPlayer,
            info,
            new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient },
            new SendOptions { Reliability = true }); ;
    }

    public void ReceiveNewPlayerEvent(object[] incomingData)//adds new player to a list of allPlayers.
    {
        string newPlayerName = (string)incomingData[0];
        int newPlayerNumber = (int)incomingData[1];
        int newPlayerKills = (int)incomingData[2];

        PlayerInfo info = new PlayerInfo(newPlayerName, newPlayerNumber, newPlayerKills);

        allPlayersList.Add(info);

        SendListAllPlayersEvent();
    }

    public void SendListAllPlayersEvent()
    {
        object[] package = new object[allPlayersList.Count];

        for (int i = 0; i < allPlayersList.Count; i++)
        {
            object[] piece = new object[3];
            piece[0] = allPlayersList[i].playerName;
            piece[1] = allPlayersList[i].actorNo;
            piece[2] = allPlayersList[i].killCount;

            package[i] = piece;
        }

        PhotonNetwork.RaiseEvent(
            (byte)EventCodes.ListAllPlayers,
            package,
            new RaiseEventOptions { Receivers = ReceiverGroup.All },
            new SendOptions { Reliability = true });
    }

    public void ReceiveListAllPlayersEvent(object[] incomingData)
    {
        //refresh player list
        allPlayersList.Clear();

        for (int i = 0; i < incomingData.Length; i++)
        {
            object[] piece = (object[])incomingData[i];

            string name = (string)piece[0];
            int number = (int)piece[1];
            int killCount = (int)piece[2];

            PlayerInfo info = new PlayerInfo(name, number, killCount);

            allPlayersList.Add(info);

            //who am i in the list?
            if(PhotonNetwork.LocalPlayer.ActorNumber == info.actorNo)
            {
                myIndex = i;
            }
        }
        UpdateLeaderBoard();
    }

    public void SendUpdateKillStatsEvent(int killerActor, int stat,int statAmount)
    {
        object[] package = new object[]
        {
            killerActor, stat, statAmount
        };

        PhotonNetwork.RaiseEvent(
           (byte)EventCodes.UpdateStats,
           package,
           new RaiseEventOptions { Receivers = ReceiverGroup.All },
           new SendOptions { Reliability = true });
    }

    public void ReceiveUpdateKillStatsEvent(object[] incomingData)
    {
        //pull out the info
        int actorNo = (int)incomingData[0];
        int stat = (int)incomingData[1];
        int statAmount = (int)incomingData[2];

        for (int i = 0; i < allPlayersList.Count; i++)
        {
            if (allPlayersList[i].actorNo == actorNo)
            {
                switch (stat)
                {
                    case 1: //kill stats
                        allPlayersList[i].killCount += statAmount;
                        break;
                }

                UpdateLeaderBoard();
                break;
            }
        }
    }

    public void UpdateLeaderBoard()
    {
        DebugController.Instance.debugInfoText.text = "Updating Leaderboard";
        RefreshLeaderBoard();

        var sortedPlayers = SortPlayers(allPlayersList);

        foreach(var p in sortedPlayers)
        {
            var item = Instantiate(leaderBoardPrefab);
            item.transform.SetParent(leaderBoardParent, false);
            item.GetComponent<LeaderboardItem>().UpdateItem(p.playerName, 0, p.killCount);

            leaderBoardItems.Add(item);
        }

    }

    private void RefreshLeaderBoard()
    {
        DebugController.Instance.debugInfoText.text = "Refreshing Leaderboard";

        foreach (var item in leaderBoardItems)
        {
            Destroy(item);
        }
        leaderBoardItems.Clear();
    }

    private List<PlayerInfo> SortPlayers(List<PlayerInfo> players)
    {
        List<PlayerInfo> result = new List<PlayerInfo>();

        while(result.Count < players.Count)
        {
            //go through all and see who has highest value
            int highestVal = -1;
            PlayerInfo selection = players[0];

            foreach (var item in players)
            {
                if (!result.Contains(item))
                {
                    if (item.killCount > highestVal)
                    {
                        selection = item;
                        highestVal = item.killCount;
                    }
                }
               
            }
            result.Add(selection);
        }

        return result;
    }
}
