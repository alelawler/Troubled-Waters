using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RepairBoatHelper : MonoBehaviour
{
    public Button repairButton;
    //public Button repairBoat;
    public void RepairBoat()
    {
        if (PlayerStatsManager.currentLife <5 && PlayerStatsManager.totalGold >300)
        {
            PlayerStatsManager.currentLife++;
            PlayerStatsManager.totalGold -= 300;
            if (PlayerStatsManager.currentLife == 5 || PlayerStatsManager.totalGold < 300)
                repairButton.interactable = false;
        }
        else
            repairButton.interactable = false;

        GetComponentInParent<TavernTalkManager>().UpdateTavernIcons();
    }
}
