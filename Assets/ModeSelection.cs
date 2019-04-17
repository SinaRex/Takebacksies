using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject sunSelector;

    private int selected = -1;
    private float inputDelay = 0.15f;
    private float nextInput = 0f;
    private bool deBounce = false;
    // Update is called once per frame
    void Update()
    {
        if (!deBounce)
        {
            if (Input.GetAxisRaw("MoveAxisY1") > 0 || Input.GetAxis("MoveAxisY2") > 0 || Input.GetKeyDown(KeyCode.UpArrow))
            {
                deBounce = true;
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

            if (Input.GetAxisRaw("MoveAxisY1") < 0 || Input.GetAxis("MoveAxisY2") < 0 || Input.GetKeyDown(KeyCode.DownArrow))
            {
                deBounce = true;
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
        }

        OnSelectChange(selected);

    }

    void OnSelectChange(int index)
    {
        if (index != -1)
        {
            actualSun.SetActive(false);
            sunSelector.SetActive(true);
        }

        switch (index)
        {
            case 0:
                SelectText(mode1Text);
                ResetText(mode2Text);
                ResetText(mode3Text);
                ResetText(mode4Text);
                sunSelector.transform.position = Vector3.Lerp();
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
        StartCoroutine(ResetDebounce());
    }

    void ResetText(Text text)
    {
        text.fontSize = (int) Mathf.Lerp(text.fontSize, 24f, 0.8f);
        text.rectTransform.sizeDelta = Vector2.Lerp(text.rectTransform.sizeDelta, 
            new Vector2(text.rectTransform.sizeDelta.x, 50), 0.8f);
    }

    void SelectText(Text text)
    {
        text.fontSize = (int)Mathf.Lerp(text.fontSize, 46f, 0.8f);
        text.rectTransform.sizeDelta = Vector2.Lerp(text.rectTransform.sizeDelta,
        new Vector2(text.rectTransform.sizeDelta.x, 100), 0.8f);

    }

    IEnumerator ResetDebounce()
    {
        yield return new WaitForSeconds(0f);
        if (Input.GetAxisRaw("MoveAxisY1") == 0)
        {
            deBounce = false;
        }
    }
}
