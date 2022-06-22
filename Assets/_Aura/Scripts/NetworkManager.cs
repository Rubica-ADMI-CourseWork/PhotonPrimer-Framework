using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
class NetworkManager : MonoBehaviourPunCallbacks
{
    bool roomAvailable = false;
    [SerializeField] string sceneName;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {  
            PhotonNetwork.JoinRandomRoom(); 
    }

    public override void OnCreatedRoom()
    {
        roomAvailable = true;
        Debug.Log("Room Created called " + PhotonNetwork.CurrentRoom.Name);
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Create room failed! " + message);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(sceneName);
        Debug.Log("Joined Room Called " + PhotonNetwork.CurrentRoom.Name);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("join room failed! " + message);

    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("join room failed! " + message);
        RoomOptions roomOptions = new RoomOptions()
        {
            MaxPlayers = (byte)4
        };
        PhotonNetwork.CreateRoom("RandomRoom", roomOptions);
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if (roomList.Count > 0)
        {

        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Another player just entered!");
       
    }
}