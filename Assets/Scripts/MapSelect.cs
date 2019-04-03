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
    public GameObject circleTransition;
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

        if (Input.GetButtonDown("SpecialButton1"))
        {
            p1Ready = false;
            p1Model.GetComponent<Animator>().SetTrigger("Cancel");
        }
        else if (Input.GetButtonDown("Jump1")){
            p1Ready = true;
            p1Model.GetComponent<Animator>().SetTrigger("ShowUp");
        }

        if (Input.GetButtonDown("SpecialButton2")){
            p2Model.GetComponent<Animator>().SetTrigger("Cancel");
            p2Ready = false;
        }
        else if (Input.GetButtonDown("Jump2"))
        {
            p2Model.GetComponent<Animator>().SetTrigger("ShowUp");
            p2Ready = true;
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
        StartCoroutine(StartLevel(stageNum));
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

    private IEnumerator StartLevel(int stageNum)
    {
        string sceneToLoad = "";
        switch (stageNum)
        {
            case 0:
                //TODO: Load original stage scene that is up to date
                p1Model.GetComponent<Animator>().SetBool("Beta_v1", true);
                p2Model.GetComponent<Animator>().SetBool("Beta_v1", true);
                sceneToLoad = "Beta_v1";
                break;
            case 1:
                //TODO: Load dino stage scene that is up to date
                p1Model.GetComponent<Animator>().SetBool("Dino", true);
                p2Model.GetComponent<Animator>().SetBool("Dino", true);
                sceneToLoad = "DinoStage";
                break;
            case 2:
                //TODO: Load cave stage scene that is up to date
                p1Model.GetComponent<Animator>().SetBool("Cave", true);
                p2Model.GetComponent<Animator>().SetBool("Cave", true);
                sceneToLoad = "CaveStage";
                break;
        }
        yield return new WaitForSeconds(3.5f);
        circleTransition.SetActive(true);
        circleTransition.GetComponent<Animator>().SetTrigger("Transit");
        yield return new WaitForSeconds(1f);
        if (GameObject.FindGameObjectsWithTag("MusicSeamless").Length > 0)
            Destroy(GameObject.FindGameObjectsWithTag("MusicSeamless")[0]);
        SceneManager.LoadScene(sceneToLoad);

    }

}
