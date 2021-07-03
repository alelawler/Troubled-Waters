using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthIconsManager : MonoBehaviour
{
    public Image[] healthIcons;

    public void UpdateIcons(int currentHealth)
    {
        for (int i = 0; i < healthIcons.Length; i++)
        {
            if (i < currentHealth)
                healthIcons[i].enabled = true;
            else
                healthIcons[i].enabled = false;
        }
    }
}
