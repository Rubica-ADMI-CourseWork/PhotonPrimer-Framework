using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInfo 
{
    public string playerName;
    public int actorNo;
    public int killCount;
 

    public PlayerInfo(string playerName, int actorNo, int killCount)
    {
        this.playerName = playerName;
        this.actorNo = actorNo;
        this.killCount = killCount;
    }
}

public enum StatType
{
    CrewAmount,
    KillCount,
    DeatCount
}

