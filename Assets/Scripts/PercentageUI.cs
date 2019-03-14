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

    public void UpdateUI(PlayerIdentity player)
    {
        if (player == PlayerIdentity.Player1)
        {
            p1percentageText.text = Mathf.Round(p1Manager.getPercent()).ToString() + "%";
            if (p1Manager.getPercent() < 50)
            {
                p1percentageText.color = Color.Lerp(Color.white, new Color(1f, 0.8f, 0f), p1Manager.getPercent() / 50);
            }
            else if (p1Manager.getPercent() <= 100)
            {
                p1percentageText.color = Color.Lerp(new Color(1f, 0.8f, 0f), new Color(1f, 0f, 0f), p1Manager.getPercent() / 100);
            }
            else
            {
                p1percentageText.color = Color.Lerp(new Color(1f, 0f, 0f), new Color(0.5f, 0f, 0f), p1Manager.getPercent() / 200);
            }
        }
        else
        {
            p2percentageText.text = Mathf.Round(p2Manager.getPercent()).ToString() + "%";
            if (p2Manager.getPercent() < 50)
            {
                p2percentageText.color = Color.Lerp(Color.white, new Color(1f, 1f, 0f), p2Manager.getPercent() / 50);
            }
            else if (p2Manager.getPercent() <= 100)
            {
                p2percentageText.color = Color.Lerp(new Color(1f, 1f, 0f), new Color(1f, 0f, 0f), p2Manager.getPercent() / 100);
            }
            else
            {
                p2percentageText.color = Color.Lerp(new Color(1f, 0f, 0f), new Color(0.5f, 0f, 0f), p2Manager.getPercent() / 200);
            }
        }
    }

  


}
