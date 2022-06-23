using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerInfo 
{
    public string playerName;
    public int actorNo;
    public int crewAmount;
    public int killCount;
    public int deathCount;

    public PlayerInfo(string playerName, int actorNo, int crewAmount, int killCount, int deathCount)
    {
        this.playerName = playerName;
        this.actorNo = actorNo;
        this.crewAmount = crewAmount;
        this.killCount = killCount;
        this.deathCount = deathCount;
    }
}

