using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PercentageUI : MonoBehaviour
{

    public GameObject p1, p2;
    public Text p1percentageText, p2percentageText;
    private PlayerManager p1Manager, p2Manager;

    // Start is called before the first frame update
    void Start()
    {
        p1Manager = p1.GetComponent<PlayerManager>();
        p2Manager = p2.GetComponent<PlayerManager>();
    }

    public void UpdateUI(bool p1)
    {
        if (p1)
        {
            p1percentageText.text = Mathf.Round(p1Manager.getPercent()).ToString() + "%";
        }
        else
        {
            p2percentageText.text = Mathf.Round(p2Manager.getPercent()).ToString() + "%";
        }
    }

}
