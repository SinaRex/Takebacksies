using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PauseMenuController : MonoBehaviour
{

    public GameObject PauseScreen = null;
    public GameManger gameManager = null;

    public GameObject MainPause;
    public GameObject ControlPause;
    public GameObject QuitPause;


    public List<Text> pauseOptions;
    private int highlightIndex = 0; //FIXME This will be hardcoded to 3 for now
 
    private bool AButton = false; 
    private bool BButton = false;
    private float debouncedInputVert = 0f;
    private bool stickReset = true;
    private PlayerIdentity pausingPlayer;

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Pause1") && (!gameManager.isPaused || pausingPlayer == PlayerIdentity.Player1)) {
            gameManager.isPaused = !gameManager.isPaused;
            pausingPlayer = PlayerIdentity.Player1;
        }
        else if (Input.GetButtonDown("Pause2") && (!gameManager.isPaused || pausingPlayer == PlayerIdentity.Player2))
        {
            gameManager.isPaused = !gameManager.isPaused;
            pausingPlayer = PlayerIdentity.Player2;
        }

        if (gameManager.isPaused) {
            PauseScreen.SetActive(true);
            debounceInput();

            for (int i = 0; i < pauseOptions.Count; i++) {
                if (i == highlightIndex) pauseOptions[i].color = Color.yellow;
                else pauseOptions[i].color = Color.white;
            }


            if (debouncedInputVert < 0 && stickReset) {
                if (highlightIndex == 0) highlightIndex = pauseOptions.Count - 1;
                else highlightIndex--;
                stickReset = false;
            }
            else if (debouncedInputVert > 0 && stickReset) { 
                if (highlightIndex >= pauseOptions.Count - 1) highlightIndex = 0;
                else highlightIndex++;
                stickReset = false;
            }

            //What happens when you press the A button
            if (AButton) {
                AButton = false;

                if (MainPause.activeSelf)
                {
                    switch (highlightIndex)
                    {

                        case 0:
                            gameManager.isPaused = !gameManager.isPaused;
                            break;

                        case 1:
                            MainPause.SetActive(false);
                            ControlPause.SetActive(true);
                            break;

                        case 2:
                            MainPause.SetActive(false);
                            QuitPause.SetActive(true);
                            break;
                    }
                }
                else if (ControlPause.activeSelf);
                else if (QuitPause.activeSelf) {
                    gameManager.isPaused = !gameManager.isPaused;
                    SceneManager.LoadScene("StartScene");
                }

            }

            if (BButton)
            {
                AButton = false;

                if (MainPause.activeSelf)
                {
                    gameManager.isPaused = !gameManager.isPaused;
                }
                else if (ControlPause.activeSelf)
                {
                    ControlPause.SetActive(false);
                    MainPause.SetActive(true);
                }
                else if (QuitPause.activeSelf)
                {
                    QuitPause.SetActive(false);
                    MainPause.SetActive(true);
                }

            }


        }

        else PauseScreen.SetActive(false);

    }

    void debounceInput() {

        if (pausingPlayer == PlayerIdentity.Player1)
        {
            debouncedInputVert = Input.GetAxis("MoveAxisY1");
            AButton = Input.GetButtonDown("Jump1");
            BButton = Input.GetButtonDown("SpecialButton1");
        }
        else if (pausingPlayer == PlayerIdentity.Player2)
        {
            debouncedInputVert = Input.GetAxis("MoveAxisY2");
            AButton = Input.GetButtonDown("Jump2");
            BButton = Input.GetButtonDown("SpecialButton2");
        }

        if (debouncedInputVert == 0f) stickReset = true;
    }
}
