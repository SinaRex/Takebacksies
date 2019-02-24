using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PercentageUI : MonoBehaviour
{

    public GameObject p1, p2;
    private PlayerManager p1Manager, p2Manager;
    private bool isP1;
    private Text percentageText;

    // Start is called before the first frame update
    void Start()
    {
        p1Manager = p1.GetComponent<PlayerManager>();
        p2Manager = p2.GetComponent<PlayerManager>();
        if(gameObject.name == "P1PercentageUI")
        {
            isP1 = true;
        }
        else
        {
            isP1 = false;
        }
        percentageText = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isP1)
        {
            percentageText.text = p1Manager.getPercent().ToString() + "%";
        }
        else
        {
            percentageText.text = p2Manager.getPercent().ToString() + "%";
        }
    }
}
