using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIManager : MonoBehaviour
{
    public Image p1life1, p1life2, p1life3, p2life1, p2life2, p2life3;
    private int p1health, p2health;
    private GameManger gameManger;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void updateUI()
    {
        gameManger = FindObjectOfType<GameManger>();
        p1health = gameManger.playersLives[0];
        p2health = gameManger.playersLives[1];

        //Temporary switching to if statements for quick fix
        if(p1health == 3)
        {
            p1life1.gameObject.SetActive(true);
            p1life2.gameObject.SetActive(true);
            p1life3.gameObject.SetActive(true);
        }
        else if(p1health == 2)
        {
            p1life1.gameObject.SetActive(true);
            p1life2.gameObject.SetActive(true);
            p1life3.gameObject.SetActive(false);
        }
        else if(p1health == 1)
        {
            p1life1.gameObject.SetActive(true);
            p1life2.gameObject.SetActive(false);
            p1life3.gameObject.SetActive(false);
        }
        else if(p1health == 0)
        {
            p1life1.gameObject.SetActive(false);
            p1life2.gameObject.SetActive(false);
            p1life3.gameObject.SetActive(false);
        }

        if (p2health == 3)
        {
            p2life1.gameObject.SetActive(true);
            p2life2.gameObject.SetActive(true);
            p2life3.gameObject.SetActive(true);
        }
        else if (p2health == 2)
        {
            p2life1.gameObject.SetActive(true);
            p2life2.gameObject.SetActive(true);
            p2life3.gameObject.SetActive(false);
        }
        else if (p2health == 1)
        {
            p2life1.gameObject.SetActive(true);
            p2life2.gameObject.SetActive(false);
            p2life3.gameObject.SetActive(false);
        }
        else if (p2health == 0)
        {
            p2life1.gameObject.SetActive(false);
            p2life2.gameObject.SetActive(false);
            p2life3.gameObject.SetActive(false);
        }
    }


}
