using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ModeSelection : MonoBehaviour
{
    //public GameObject mode1;
    //public GameObject mode2;
    //public GameObject mode3;

    public Text mode1Text;
    public Text mode2Text;
    public Text mode3Text;
    public Text mode4Text;


    public GameObject actualSun;
    public Image sunSelector;

    public GameObject p1;
    public GameObject p2;

    private int selected = -1;
    private float inputDelay = 0.15f;
    private float nextInput = 0f;
    private bool deBounce1 = false;
    private bool deBounce2 = false;
    // Update is called once per frame
    void Update()
    {
        // Preserve the Scene

        if (Input.GetButtonDown("Jump1") || Input.GetButtonDown("Jump2")) {

            SceneManager.LoadScene("MapSelectScene");
        }



        if (!deBounce1)
        {
            // Player 1
            if (Input.GetAxis("MoveAxisY1") < 0 || Input.GetKeyDown(KeyCode.UpArrow))
            {
                p1.GetComponent<Animator>().SetTrigger("ChangeMode");
                deBounce1 = true;
                ChangeSelectUp();
            }

            if (Input.GetAxis("MoveAxisY1") > 0 || Input.GetKeyDown(KeyCode.DownArrow))
            {
                p1.GetComponent<Animator>().SetTrigger("ChangeMode");
                deBounce1 = true;
                ChangeSelectDown();
            }

        }

        if (!deBounce2)
        {
            // player 2
            if (Input.GetAxis("MoveAxisY2") < 0 || Input.GetKeyDown(KeyCode.UpArrow))
            {
                p2.GetComponent<Animator>().SetTrigger("ChangeMode");
                deBounce2 = true;
                ChangeSelectUp();
            }

            if (Input.GetAxis("MoveAxisY2") > 0 || Input.GetKeyDown(KeyCode.DownArrow))
            {
                p2.GetComponent<Animator>().SetTrigger("ChangeMode");
                deBounce2 = true;
                ChangeSelectDown();
            }

        }

        OnSelectChange(selected);

    }

    void ChangeSelectUp()
    {
        switch (selected)
        {
            case -1:
                selected = 3;
                break;
            case 0:
                selected = 3;
                break;
            case 1:
                selected = 0;
                break;
            case 2:
                selected = 1;
                break;
            case 3:
                selected = 2;
                break;
            default:
                selected = -1;
                break;
        }

    }

    void ChangeSelectDown()
    {
        switch (selected)
        {
            case -1:
                selected = 0;
                break;
            case 0:
                selected = 1;
                break;
            case 1:
                selected = 2;
                break;
            case 2:
                selected = 3;
                break;
            case 3:
                selected = 0;
                break;
            default:
                selected = -1;
                break;
        }
    }

    void OnSelectChange(int index)
    {
        if (index != -1)
        {
            Debug.Log("Hello");
            actualSun.SetActive(false);
            sunSelector.enabled = true;
        }

        switch (index)
        {
            case 0:
                SelectText(mode1Text);
                ResetText(mode2Text);
                ResetText(mode3Text);
                ResetText(mode4Text);
                break;
            case 1:
                ResetText(mode1Text);
                SelectText(mode2Text);
                ResetText(mode3Text);
                ResetText(mode4Text);
                break;
            case 2:
                ResetText(mode1Text);
                ResetText(mode2Text);
                SelectText(mode3Text);
                ResetText(mode4Text);

                break;
            case 3:
                ResetText(mode1Text);
                ResetText(mode2Text);
                ResetText(mode3Text);
                SelectText(mode4Text);

                break;
        }

        if (Input.GetAxisRaw("MoveAxisY1") == 0)
        {
            deBounce1 = false;
        }

        if (Input.GetAxisRaw("MoveAxisY2") == 0)
        {
            deBounce2 = false;
        }
    }

    void ResetText(Text text)
    {
        text.fontSize = (int) Mathf.Lerp(text.fontSize, 24f, 0.8f);
        text.rectTransform.sizeDelta = Vector2.Lerp(text.rectTransform.sizeDelta, 
            new Vector2(text.rectTransform.sizeDelta.x, 50), 0.8f);
    }

    void SelectText(Text text)
    {
        RectTransform rt = text.transform.GetComponent<RectTransform>();
        float width = rt.sizeDelta.x * rt.localScale.x;
        float height = rt.sizeDelta.y * rt.localScale.y;
        Vector3 toLerp = text.transform.position - new Vector3(text.rectTransform.sizeDelta.x / 2, -6, 0);
        sunSelector.rectTransform.position = Vector3.Lerp(sunSelector.rectTransform.position, toLerp, 0.4f);
        text.fontSize = (int)Mathf.Lerp(text.fontSize, 46f, 0.8f);
        text.rectTransform.sizeDelta = Vector2.Lerp(text.rectTransform.sizeDelta,
        new Vector2(text.rectTransform.sizeDelta.x, 100), 0.8f);

    }

}
