using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SetLeaderboardItemInfo : MonoBehaviour
{
    public TextMeshProUGUI positionText;

    public TextMeshProUGUI driverNameText;

    public void SetPositionText(string newPosition)
    {
        positionText.text = newPosition;
    }
    public void SetDriverNameText(string newDriverName)
    {
        driverNameText.text = newDriverName;
    }
}