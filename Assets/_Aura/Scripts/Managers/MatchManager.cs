using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public enum EventCodes : byte
{
    UpdateStats,
    ListAllPlayers,
    AddNewPlayer
}
public class MatchManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    #region Member Fields
    [SerializeField] private List<PlayerInfo> playerInfoList = new List<PlayerInfo>();
    private int index;
    #endregion

    #region Unity Callbacks
    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            SendAddNewPlayerEvent("Tony");
        }
    }
    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    private void OnDisable()
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
    public void SendUpdateStatsEvent()
    {

    }
    public void ReceiveUpdateStatsEvent(object[] inComingData)
    {

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

            if(PhotonNetwork.LocalPlayer.ActorNumber == info.actorNo)
            {
                index = i;
            }
        }
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

}
