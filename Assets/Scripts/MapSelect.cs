using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapSelect : MonoBehaviour
{

    public RawImage originalStage;
    public RawImage dinoStage;
    public RawImage caveStage;
    public RawImage originalBorder;
    public RawImage dinoBorder;
    public RawImage caveBorder;
    private float inputDelay = 0.15f;
    private float nextInput = 0f;
    private int selected = 2; //Index of Selected Stage i.e. 0 for original, 1 for dino, 2 for cave

    // Start is called before the first frame update
    void Start()
    {
        OnSelectChange(selected);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("SpecialButton1"))
        {
            PlayStage(selected);
        }
        if (Time.time > nextInput)
        {
            nextInput = Time.time + inputDelay;
            if (Input.GetAxis("MoveAxisX1") > 0)
            {
                switch (selected)
                {
                    case 0:
                        selected = 1;
                        break;
                    case 1:
                        selected = 2;
                        break;
                    case 2:
                        selected = 0;
                        break;
                }
                OnSelectChange(selected);
            }
            if (Input.GetAxis("MoveAxisX1") < 0)
            {
                switch (selected)
                {
                    case 0:
                        selected = 2;
                        break;
                    case 1:
                        selected = 0;
                        break;
                    case 2:
                        selected = 1;
                        break;
                }
                OnSelectChange(selected);
            }
        }
    }

    void PlayStage(int stageNum)
    {
        switch (stageNum)
        {
            case 0:
                //TODO: Load original stage scene that is up to date
                SceneManager.LoadScene("Beta_v1");
                break;
            case 1:
                //TODO: Load original stage scene that is up to date
                SceneManager.LoadScene("DinoStage");
                break;
            case 2:
                //TODO: Load original stage scene that is up to date
                SceneManager.LoadScene("CaveStage");
                break;
        }
    }

    void OnSelectChange(int index)
    {
        switch (index)
        {
            case 0:
                dinoStage.color = new Color(1f, 1f, 1f, 0.6f);
                caveStage.color = new Color(1f, 1f, 1f, 0.6f);
                originalStage.color = new Color(1f, 1f, 1f, 1f);
                originalBorder.enabled = true;
                dinoBorder.enabled = false;
                caveBorder.enabled = false;
                break;
            case 1:
                originalStage.color = new Color(1f, 1f, 1f, 0.6f);
                caveStage.color = new Color(1f, 1f, 1f, 0.6f);
                dinoStage.color = new Color(1f, 1f, 1f, 1f);
                originalBorder.enabled = false;
                dinoBorder.enabled = true;
                caveBorder.enabled = false;
                break;
            case 2:
                dinoStage.color = new Color(1f, 1f, 1f, 0.6f);
                originalStage.color = new Color(1f, 1f, 1f, 0.6f);
                caveStage.color = new Color(1f, 1f, 1f, 1f);
                originalBorder.enabled = false;
                dinoBorder.enabled = false;
                caveBorder.enabled = true;
                break;
        }
    }

}
