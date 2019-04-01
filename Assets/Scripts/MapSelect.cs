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
    public GameObject p1Model, p2Model;
    private Vector3 p1ModelPosition, p2ModelPosition;
    private float inputDelay = 0.15f;
    private float nextInput = 0f;
    private int selected = 0; //Index of Selected Stage i.e. 0 for original, 1 for dino, 2 for cave
    private bool p1Ready, p2Ready = false;

    // Start is called before the first frame update
    void Start()
    {
        OnSelectChange(selected);
    }

    // Update is called once per frame
    void Update()
    {
        MoveModels();

        if (Input.GetButtonDown("SpecialButton1"))
        {
            p1Ready = true;
        }
        else if (Input.GetButtonDown("Jump1")){
            p1Ready = false;
        }

        if (Input.GetButtonDown("SpecialButton2")){
            p2Ready = true;
        }
        else if (Input.GetButtonDown("Jump2"))
        {
            p2Ready = false;
        }

        if(p1Ready && p2Ready)
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
                //TODO: Load dino stage scene that is up to date
                SceneManager.LoadScene("DinoStage");
                break;
            case 2:
                //TODO: Load cave stage scene that is up to date
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

    void MoveModels()
    {
        if(p1Ready && p1Model.transform.position.y < 10.64f)
        {
            p1Model.transform.position = new Vector3(p1Model.transform.position.x, p1Model.transform.position.y + 0.25f, p1Model.transform.position.z);
        }
        else if(!p1Ready && p1Model.transform.position.y > 5.5f)
        {
            p1Model.transform.position = new Vector3(p1Model.transform.position.x, p1Model.transform.position.y - 0.25f, p1Model.transform.position.z);
        }

        if (p2Ready && p2Model.transform.position.y < 10.64f)
        {
            p2Model.transform.position = new Vector3(p2Model.transform.position.x, p2Model.transform.position.y + 0.25f, p2Model.transform.position.z);
        }
        else if (!p2Ready && p2Model.transform.position.y > 5.5f)
        {
            p2Model.transform.position = new Vector3(p2Model.transform.position.x, p2Model.transform.position.y - 0.25f, p2Model.transform.position.z);
        }

    }
}
