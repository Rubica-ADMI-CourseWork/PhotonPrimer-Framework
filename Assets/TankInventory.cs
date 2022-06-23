using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankInventory : MonoBehaviour
{
   [field:SerializeField]public int crewAmount { get;private set; }
   public void DepositCrew()
    {
        crewAmount++;
    }

    public void LooseCrew()
    {
        crewAmount = 0;
    }
}
