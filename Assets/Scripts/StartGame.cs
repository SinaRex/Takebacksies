using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public GameObject foreGround;
    public GameObject backGround;
    public GameObject mainCam;

    /*Canvas stuff*/
    public GameObject logo;
    public GameObject startButton;
    public GameObject canvas1;
    public GameObject canvas2;

    private bool lerpCamera = false;

    void Start() {
        Time.timeScale = 1f;
    }

    void Update() {
        Time.timeScale = 1f;
        if (Input.anyKey) {
            ChangeScene();
        }
        if (Input.GetButton("Jump1") || Input.GetButton("Jump2"))
        {
            ChangeScene();
        }

        // Pan the camera up to the mode selection screen
        if (lerpCamera)
        {
            Vector3 toLerpPos = new Vector3(mainCam.transform.position.x, 42, mainCam.transform.position.z);
            mainCam.transform.position = Vector3.Lerp(mainCam.transform.position, toLerpPos, 0.1f);

        }


    }

    public void ChangeScene()
    {
        StartCoroutine(CharacterSelector());
    }

   private IEnumerator CharacterSelector()
    {
        backGround.GetComponent<Animator>().SetTrigger("backStart");
        yield return new WaitForSeconds(0.2f);
        foreGround.GetComponent<Animator>().SetTrigger("ForeStart");
        logo.GetComponent<Animator>().SetTrigger("GoMode");
        startButton.GetComponent<Animator>().SetTrigger("GoMode");
        lerpCamera = true;
        yield return new WaitForSeconds(1f);
        canvas1.SetActive(false);
        canvas2.SetActive(true);
           
        // FOR THE REST OF THE CODE LOOK AT THE SCRIPT ATTACHED TO Canvas2



        yield return new WaitForSeconds(0f);



    }
}
