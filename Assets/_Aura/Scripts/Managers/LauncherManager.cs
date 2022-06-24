using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class LauncherManager : MonoBehaviourPunCallbacks
{
    #region Singleton Setup
    public static LauncherManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    #endregion

    [Header("Launcher Menu objects")]
    public GameObject roomBrowserPanel;
    public GameObject roomButtonParent;
    public GameObject roomButtonPrefab;
    public GameObject errorPanel;
    public TMP_Text errorText;
    public GameObject roomPanel;
    public TMP_Text roomTitleText;
    public GameObject playerListPrefab;
    public Transform playerListItemParent;
    public GameObject CreateRoomPanel;
    public TMP_InputField createRoomNameInput;
    public GameObject loadingScreen;
    public TMP_Text loadingText;
    public GameObject menuButtonsPanel;

    private List<GameObject> allRoomButtons = new List<GameObject>();
    private List<GameObject> allPlayerTexts = new List<GameObject> ();
    private void Start()
    {
        CloseAllMenus();
        loadingScreen.SetActive(true);
        loadingText.text = "Connecting to Network....";

        PhotonNetwork.ConnectUsingSettings();
    }
    private void CloseAllMenus()
    {
        roomBrowserPanel.SetActive(false);
        errorPanel.SetActive(false);
        roomPanel.SetActive(false);
        CreateRoomPanel.SetActive(false);
        loadingScreen.SetActive(false);
        menuButtonsPanel.SetActive(false);
    }

    #region PUN Callbacks
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        loadingText.text = "joining Lobby....";
    }

    public override void OnJoinedLobby()
    {
        CloseAllMenus();
        menuButtonsPanel.SetActive(true);

        PhotonNetwork.NickName = Random.Range(0, 1000).ToString();
    }
    public override void OnJoinedRoom()
    {
        CloseAllMenus();
        roomPanel.SetActive(true);
        roomTitleText.text = PhotonNetwork.CurrentRoom.Name;
        ListAllPlayers();

        Player[] players = PhotonNetwork.PlayerList;//cache so as not to loop over the network
        for(int i = 0; i < players.Length; i++)
        {
            var newPlayerLabel = Instantiate(playerListPrefab);
            newPlayerLabel.transform.SetParent(playerListItemParent, false);
            newPlayerLabel.GetComponent<TMP_Text>().text = players[i].NickName;

            allPlayerTexts.Add(newPlayerLabel);
        }
    }
    private void ListAllPlayers()
    {
        foreach (var listitem in allPlayerTexts)
        {
            Destroy(listitem);
        }
        allPlayerTexts.Clear();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (var roomButton in allRoomButtons)
        {
            Destroy(roomButton.gameObject);
        }
        allRoomButtons.Clear();

        for(int i = 0; i < roomList.Count; i++)
        {
            //filter out
            if (roomList[i].PlayerCount != roomList[i].MaxPlayers && !roomList[i].RemovedFromList)
            {
                var newButton = Instantiate(roomButtonPrefab);
                newButton.transform.SetParent(roomButtonParent.transform,false);
                newButton.GetComponent<RoomButton>().SetButtonDetails(roomList[i]);

                allRoomButtons.Add(newButton);
            }
        }
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        CloseAllMenus();
        errorPanel.SetActive(true);
        errorText.text = "Creating Room Failed: " + message;
    }

    public override void OnLeftRoom()
    {
        CloseAllMenus();
        menuButtonsPanel.SetActive(true);
    } 
    #endregion
    #region Button Callbacks
    public void OpenRoomCreate()
    {
        CloseAllMenus();
        CreateRoomPanel.SetActive(true);
    }

    public void CreateRoom()
    {
        if (!string.IsNullOrEmpty(createRoomNameInput.text))
        {
            //limit players to 8
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 8;
            PhotonNetwork.CreateRoom(createRoomNameInput.text,options);

            CloseAllMenus();
            loadingText.text = "Creating Room....";
            loadingScreen.SetActive(true);
        }
    }

    public void CloseErrorScreen()
    {
        CloseAllMenus();
        menuButtonsPanel.SetActive(true);
    }

    public void FindRoom()
    {
        CloseAllMenus();
        roomBrowserPanel.SetActive(true);
    }

    public void joinRoom(RoomInfo inputInfo)
    {
        PhotonNetwork.JoinRoom(inputInfo.Name);

        CloseAllMenus();
        loadingScreen.SetActive(true);
        loadingText.text = "Joining room....";
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        CloseAllMenus();
        loadingScreen.SetActive(true);
        loadingText.text = "Leaving Room...."; ;
    }

    public void LeaveGame()
    {
        Application.Quit();
    }
    #endregion
}
