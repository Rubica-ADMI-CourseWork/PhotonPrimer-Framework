using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardItem : MonoBehaviour
{
    public TMP_Text playerNameTxt;
    public TMP_Text crewAmountTxt;
    public TMP_Text killCountTxt;

    public void UpdateItem(string name,int crewAmnt,int killCnt)
    {
        playerNameTxt.text = name;
        crewAmountTxt.text = crewAmnt.ToString();
        killCountTxt.text = killCnt.ToString();
    }
}
