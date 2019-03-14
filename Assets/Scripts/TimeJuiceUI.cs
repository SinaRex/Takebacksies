using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeJuiceUI : MonoBehaviour
{

    public Image p1TimeBar;
    public Image p2TimeBar;

    /*
     * bool p1 -> if function is being called on p1
     * ratio -> timeJuice / maxTimeJuice   
     */
    public void updateUI(PlayerIdentity player, float ratio)
    {
        if (player == PlayerIdentity.Player1)
        {
            p1TimeBar.rectTransform.localScale = new Vector3(ratio, 1, 1);
        }
        else
        {
            p2TimeBar.rectTransform.localScale = new Vector3(ratio, 1, 1);
        }
    }
}
