using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using TMPro;

public class RoomButton : MonoBehaviour
{
    public TMP_Text roomNameText;

    RoomInfo info;

    public void SetButtonDetails(RoomInfo incomingInfo)
    {
        info = incomingInfo;

        roomNameText.text = info.Name;
    }

    public void OpenRoom()
    {
        LauncherManager.Instance.joinRoom(info);
    }
}
