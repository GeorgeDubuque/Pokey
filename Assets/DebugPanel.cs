using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugPanel : MonoBehaviour
{
    public TMP_Text launchPercentText;
    public TMP_Text launchPowerText;

    // displays amount user is pulling back
    public void DisplayLaunchPercentage(float launchValue)
    {
        string launchMessage = "Launch Percentage: {0}%";
        string launchPercent = (launchValue * 100).ToString("0.00");
        launchPercentText.text = string.Format(launchMessage, launchPercent);
    }

    public void DisplayLaunchForce(float launchPower)
    {
        string launchMessage = "Launch Power: {0}";
        string launchPercent = (launchPower).ToString("0.00");
        launchPowerText.text = string.Format(launchMessage, launchPercent);
    }
}

