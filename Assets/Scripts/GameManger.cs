using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManger : MonoBehaviour
{

    public bool isPaused = false;    

    /* Can be changed in Unity Editor */
    public float gameTimerRemaining = 1200f;

    /* Maximum lives that each player can have*/
    public int maxLives = 3;

    /* Count the win times for player1 and player2 */
    public int wincount1 = 0;
    public int wincount2 = 0;

    public int MaxWins = 2;

    /* Respawn Duration*/
    public int respawnDuration = 2;

    /* Invinciblity Duration*/
    public int invincibeDuration = 2;

    /* Deathplosions */
    public GameObject p1Deathsplosion, p2Deathsplosion;

    /* List of players GameObject*/
    private List<GameObject> players = new List<GameObject>();

    /* List of time clone gameobjects*/
    private GameObject[] clones;

    /* List of lives that correspond to the players in the list players.*/
    public List<int> playersLives = new List<int>();

    /* Respawn Platform (DRAG AND DROP)*/
    public GameObject respawnPlatform;


    /* Enum */
    private enum whichPlayer {Player1, Player2, Echo1, Echo2}

    /* UI */
    //public Text timeLabel;
    public Text textCount1;
    public Text textCount2;
    public Text gameOverText;
    public Text RoundWinner;
    public Text GameWinner;
    public GameObject circleTransition;

    public GameObject winface1;
    public GameObject winface2;

    public GameObject rematchtButton;
    public GameObject rageQuitbutton;


    public Image count1;
    public Image count2;
    public Image count3;
    public Image count4;
    public Image count5;
    public Image count6;
    public Image count7;
    public Image count8;
    public Image count9;
    public Image count10;
    /* GameOver Tracking */
    private bool isRoundOver = false;
    private bool isGameOver = false;
    private bool restartButtonsEnabled = false;



    //FIXME: Used for syncing player inputs to FixedUpdate
    void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 50;
    }

    /**
     * Start is called before the first frame update
     */
    void Start()
    {
        // Initialize the spawn of the player and live stocks for 
        players.Add(GameObject.Find("Player1"));
        playersLives.Add(maxLives);
        players.Add(GameObject.Find("Player2"));
        playersLives.Add(maxLives);

        //FIXME LEVELUP
        MaxWins = GameModeSelector.MaxWinCount;


        // TODO: Initialize the map related stuff (e.g. respawnPlatforms) 
        StartCoroutine(TransitOut());
    }

    /**
     * Update is called for each frame.
     */
    private void Update()
    {
        // This slows down the time for debugging
        if (isPaused || restartButtonsEnabled) Time.timeScale = 0f;
        else Time.timeScale = 1f;


        //If the game is over, process these inputs:
        if (restartButtonsEnabled) {
            
            if(Input.GetButtonDown("Jump1") || Input.GetButtonDown("Jump2")) // Restart this scene
                SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
            else if (Input.GetButtonDown("SpecialButton1") || Input.GetButtonDown("SpecialButton2")) //Quit to main menu
                SceneManager.LoadScene("StartScene");



        }


        for (int i = 0; i < players.Count; i++)
        {

            switch (players[i].GetComponent<PlayerManager>().GetState())
            {
                case PlayerState.Dead:
                    if (players[i].GetComponent<PlayerManager>().GetWhichPlayer() != PlayerIdentity.Echo) {
                        if (!players[i].GetComponent<PlayerManager>().IsDying())
                        {
                            if (!isRoundOver) playersLives[i] -= 1;

                            //FIXME LEVELUP
                            players[i].GetComponent<PlayerManager>().Respawn();

                            if (playersLives[i] <= 0)
                            {
                                if (i == 0)
                                {
                                    wincount2 += 1;
                                }
                                else
                                {
                                    wincount1 += 1;
                                }

                                switch (wincount1)
                                {
                                    case 0:
                                        count1.color = new Color(count1.color.r, count1.color.g, count1.color.b, 0.0f);
                                        count2.color = new Color(count2.color.r, count2.color.g, count2.color.b, 0.0f);
                                        count3.color = new Color(count3.color.r, count3.color.g, count3.color.b, 0.0f);
                                        count4.color = new Color(count4.color.r, count4.color.g, count4.color.b, 0.0f);
                                        count5.color = new Color(count5.color.r, count5.color.g, count5.color.b, 0.0f);
                                        break;
                                    case 1:
                                        count1.color = new Color(count1.color.r, count1.color.g, count1.color.b, 1.0f);
                                        break;
                                    case 2:
                                        count1.color = new Color(count1.color.r, count1.color.g, count1.color.b, 1.0f);
                                        count2.color = new Color(count2.color.r, count2.color.g, count2.color.b, 1.0f);
                                        break;
                                    case 3:
                                        count1.color = new Color(count1.color.r, count1.color.g, count1.color.b, 1.0f);
                                        count2.color = new Color(count2.color.r, count2.color.g, count2.color.b, 1.0f);
                                        count3.color = new Color(count3.color.r, count3.color.g, count3.color.b, 1.0f);
                                        break;
                                    case 4:
                                        count1.color = new Color(count1.color.r, count1.color.g, count1.color.b, 1.0f);
                                        count2.color = new Color(count2.color.r, count2.color.g, count2.color.b, 1.0f);
                                        count3.color = new Color(count3.color.r, count3.color.g, count3.color.b, 1.0f);
                                        count4.color = new Color(count4.color.r, count4.color.g, count4.color.b, 1.0f);
                                        break;
                                    case 5:
                                        count1.color = new Color(count1.color.r, count1.color.g, count1.color.b, 1.0f);
                                        count2.color = new Color(count2.color.r, count2.color.g, count2.color.b, 1.0f);
                                        count3.color = new Color(count3.color.r, count3.color.g, count3.color.b, 1.0f);
                                        count4.color = new Color(count4.color.r, count4.color.g, count4.color.b, 1.0f);
                                        count5.color = new Color(count5.color.r, count5.color.g, count5.color.b, 1.0f);
                                        break;
                                    default:
                                        count1.color = new Color(count1.color.r, count1.color.g, count1.color.b, 0.0f);
                                        count2.color = new Color(count2.color.r, count2.color.g, count2.color.b, 0.0f);
                                        count3.color = new Color(count3.color.r, count3.color.g, count3.color.b, 0.0f);
                                        count4.color = new Color(count4.color.r, count4.color.g, count4.color.b, 0.0f);
                                        count5.color = new Color(count5.color.r, count5.color.g, count5.color.b, 0.0f);
                                        textCount1.text = string.Format("{0}", wincount1);
                                        break;
                                }

                                switch (wincount2)
                                {
                                    case 0:
                                        count6.color = new Color(count6.color.r, count6.color.g, count6.color.b, 0.0f);
                                        count7.color = new Color(count7.color.r, count7.color.g, count7.color.b, 0.0f);
                                        count8.color = new Color(count8.color.r, count8.color.g, count8.color.b, 0.0f);
                                        count9.color = new Color(count9.color.r, count9.color.g, count9.color.b, 0.0f);
                                        count10.color = new Color(count10.color.r, count10.color.g, count10.color.b, 0.0f);
                                        break;
                                    case 1:
                                        count6.color = new Color(count6.color.r, count6.color.g, count6.color.b, 1.0f);
                                        break;
                                    case 2:
                                        count6.color = new Color(count6.color.r, count6.color.g, count6.color.b, 1.0f);
                                        count7.color = new Color(count7.color.r, count7.color.g, count7.color.b, 1.0f);
                                        break;
                                    case 3:
                                        count6.color = new Color(count6.color.r, count6.color.g, count6.color.b, 1.0f);
                                        count7.color = new Color(count7.color.r, count7.color.g, count7.color.b, 1.0f);
                                        count8.color = new Color(count8.color.r, count8.color.g, count8.color.b, 1.0f);
                                        break;
                                    case 4:
                                        count6.color = new Color(count6.color.r, count6.color.g, count6.color.b, 1.0f);
                                        count7.color = new Color(count7.color.r, count7.color.g, count7.color.b, 1.0f);
                                        count8.color = new Color(count8.color.r, count8.color.g, count8.color.b, 1.0f);
                                        count9.color = new Color(count9.color.r, count9.color.g, count9.color.b, 1.0f);
                                        break;
                                    case 5:
                                        count6.color = new Color(count6.color.r, count6.color.g, count6.color.b, 1.0f);
                                        count7.color = new Color(count7.color.r, count7.color.g, count7.color.b, 1.0f);
                                        count8.color = new Color(count8.color.r, count8.color.g, count8.color.b, 1.0f);
                                        count9.color = new Color(count9.color.r, count9.color.g, count9.color.b, 1.0f);
                                        count10.color = new Color(count10.color.r, count10.color.g, count10.color.b, 1.0f);
                                        break;
                                    default:
                                        count6.color = new Color(count6.color.r, count6.color.g, count6.color.b, 0.0f);
                                        count7.color = new Color(count7.color.r, count7.color.g, count7.color.b, 0.0f);
                                        count8.color = new Color(count8.color.r, count8.color.g, count8.color.b, 0.0f);
                                        count9.color = new Color(count9.color.r, count9.color.g, count9.color.b, 0.0f);
                                        count10.color = new Color(count10.color.r, count10.color.g, count10.color.b, 0.0f);
                                        textCount2.text = string.Format("{0}", wincount2);
                                        break;
                                }

                                //Decide Whether to end the round or the Game
                                if(wincount1 < MaxWins && wincount2 < MaxWins) StartCoroutine(RoundOver());
                                else StartCoroutine(GameOver());
                            }


                            FindObjectOfType<HealthUIManager>().updateUI();
                            players[i].GetComponent<PlayerManager>().SetIsDying(true);
                        }
                    }
                    break;

                case PlayerState.Respawning:
                    if (!players[i].GetComponent<PlayerManager>().IsRespawning())
                    {
                        StartCoroutine(RespawnPlayer(players[i])); // Respawn only once and make it goinvincible there
                        StartCoroutine(StartFlashing(players[i]));// Also start flashing

                        players[i].GetComponent<PlayerManager>().SetIsRespawning(true);
                        //players[i].GetComponent<PlayerManager>().SetIsDying(false);
                    }
                    break;

                default:
                    players[i].GetComponent<PlayerManager>().SetIsDying(false);
                    players[i].GetComponent<PlayerManager>().SetIsRespawning(false);
                    break;
            }
        }
    }



    /**
     * FixedUpdate is usually 1/50 seconds per frame (or 50 FPS)
     */
    private void FixedUpdate()
    {
        // Comment this out for testing
        gameTimerRemaining -= Time.deltaTime;

        //Updating UI
        //var minutes = gameTimerRemaining / 60 - 1; //Divide the guiTime by sixty to get the minutes.
        //var seconds = gameTimerRemaining % 60 - 1;//Use the euclidean division for the seconds.
        //timerLabel.text = string.Format("{0:00} : {1:00}", minutes, seconds);

        //Game over when time is done
        if (gameTimerRemaining < 0)
        {
            StartCoroutine(RoundOver());
            //GameOver();
        }
    }


    /**
     * when the game is over, show who the winner is
     * or if it's a tie. Then probably have a button to go 
     * back to the menu.
     */
    private IEnumerator RoundOver()
    {

        //gameOverText.gameObject.SetActive(true);
        isRoundOver = true;

        if (playersLives[0] < playersLives[1])
        {
            p2Deathsplosion.SetActive(false);
            RoundWinner.text = String.Format("Round {0}: <color=#18005C>Kragg!</color>", (wincount1+wincount2));//Kragg Wins!";
            //winface2.SetActive(true);
        }
        else if (playersLives[0] > playersLives[1])
        {
            p1Deathsplosion.SetActive(false);
            RoundWinner.text = String.Format("Round {0}: <color=#D4AA2F>Fred!</color>", (wincount1 + wincount2)); // Fred Wins!";
            //winface1.SetActive(true);
        }
        else
        {
            p1Deathsplosion.SetActive(false);
            p2Deathsplosion.SetActive(false);
            RoundWinner.text = "Draw!";
        }


        //yield return new WaitForSeconds(2f);

        //Destroy all remaining clones after a gameover
        clones = GameObject.FindGameObjectsWithTag("Clone");
        foreach (GameObject clone in clones)
        {
            clone.GetComponent<PlayerManager>().Die();
        }


        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].GetComponent<PlayerManager>().GetState() != PlayerState.Dead
                && players[i].GetComponent<PlayerManager>().GetState() != PlayerState.Respawning) players[i].GetComponent<PlayerManager>().Die();
            players[i].GetComponent<PlayerManager>().Respawn();
            players[i].GetComponent<PlayerManager>().SetTimeJuice(0);
            players[i].GetComponent<PlayerManager>().resetCloneCount();
            playersLives[i] = 3;
        }



        //Destroy all remaining clones after a gameover
        clones = GameObject.FindGameObjectsWithTag("Clone");
        foreach (GameObject clone in clones)
        {
            clone.GetComponent<PlayerManager>().Die();
        }

        yield return new WaitForSeconds(2);
        p1Deathsplosion.SetActive(true);
        p2Deathsplosion.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        winface1.SetActive(false);
        winface2.SetActive(false);

        gameTimerRemaining = 3000f;
        isRoundOver = false;
        gameOverText.gameObject.SetActive(false);
        RoundWinner.text = "";



    }


    private IEnumerator GameOver()
    {
        isGameOver = true;
        isPaused = false;

        if (wincount1 < wincount2)
        {
            p2Deathsplosion.SetActive(false);
            GameWinner.text = "<color=#18005C>Kragg</color> Wins!";
            GameWinner.GetComponent<Outline>().enabled = false;
            winface2.SetActive(true);
        }
        else if (wincount1 > wincount2)
        {
            p1Deathsplosion.SetActive(false);
            GameWinner.text = "<color=#D4AA2F>Fred</color> Wins!";
            GameWinner.GetComponent<Outline>().enabled = true; ;
            winface1.SetActive(true);
        }

        yield return new WaitForSecondsRealtime(2.5f);

        restartButtonsEnabled = true;

        rematchtButton.SetActive(true);
        rageQuitbutton.SetActive(true);

        //isGameOver = false;

    }

    /**
     * Respawn animation where the player is respawned on the top of a platform
     */
    private IEnumerator RespawnPlayer(GameObject player)
    {
        // Freeze/stop the player.
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        // Play the blast animation.
        //player.GetComponentInChildren<ParticleSystem>().Play();
        //StartCoroutine(MakePlayerInvincible());

        // Wait two (or customized) seconds before respawning.
        yield return new WaitForSeconds(respawnDuration - 0.5f);

        GameObject platform = MakeRespawnPlatform(player); 

        if (platform) // if it's not the echo player
        {
            player.transform.position = platform.transform.position + new Vector3(0, player.transform.localScale.y, 0);
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None |
            RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX;
            platform.GetComponent<Rigidbody>().isKinematic = false;

            // Move the player and platform down!
            for (int i = 0; i < 25; i++)
            {
                yield return new WaitForFixedUpdate();
                platform.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0f, 0));
                player.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0f, 0));
            }
            platform.GetComponent<Rigidbody>().isKinematic = true;

            yield return new WaitForSeconds(0.5f);

            platform.GetComponent<BoxCollider>().enabled = false;
            player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None |
                RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
            Destroy(platform);

            // Go invincible.
            player.GetComponent<PlayerManager>().FinishRespawning();

        }
        else
        {
            // TODO: deal with the echo players
            Debug.LogError("UnImplemented For echo Players");
        }

    }


    private GameObject MakeRespawnPlatform(GameObject player)
    {

        if (player.GetComponent<PlayerManager>().GetWhichPlayer() == PlayerIdentity.Player1)
        {
            Debug.Log(GameObject.Find("RespawnPoint1"));
           return Instantiate(respawnPlatform,
                            GameObject.Find("RespawnPoint1").transform.position,
                            respawnPlatform.transform.rotation);
        }

        else if (player.GetComponent<PlayerManager>().GetWhichPlayer() == PlayerIdentity.Player2)
        {
            return Instantiate(respawnPlatform,
                            GameObject.Find("RespawnPoint2").transform.position,
                            respawnPlatform.transform.rotation);
        }

        else
        {
            return null;
        }
    }

    private IEnumerator StartFlashing(GameObject player)
    {
        float totalDuration = Mathf.Ceil((respawnDuration + invincibeDuration) / 0.25f);
        int i = 0;
        GameObject playerModel = player.transform.GetChild(2).gameObject;// get CharacterModel gameobject
        while (i < totalDuration && 
            (player.GetComponent<PlayerManager>().GetState() == PlayerState.Invincible || 
            player.GetComponent<PlayerManager>().GetState() == PlayerState.Respawning))
        {
            yield return new WaitForSeconds(0.125f);
            //player.GetComponentInChildren<Renderer>().enabled = false; // FIXME: replace the rendered with tag

            playerModel.transform.GetChild(1).gameObject.GetComponent<Renderer>().enabled = false;
            yield return new WaitForSeconds(0.125f);
            //player.GetComponentInChildren<Renderer>().enabled = true; // FIXME: replace it with tag
            playerModel.transform.GetChild(1).gameObject.GetComponent<Renderer>().enabled = true;
            i++;
        }


    }
    IEnumerator TransitOut()
    {
        if (circleTransition)
        {
            circleTransition.SetActive(true);
            circleTransition.GetComponent<Animator>().SetTrigger("TransitOut");
            yield return new WaitForSeconds(0.5f);
            circleTransition.SetActive(false);
        }

    }

    /** =============== START: PlayZone & BlastZone Logic =================*/

    /** =============== END: PlayZone & BlastZone Logic ======================*/


    /** =============== Public Functions ======================*/

    public bool isitGameOver() {
        return isGameOver;
    }


    public void MuteSound() {

        for (int i = 0; i < players.Count; i++)
        {
            players[i].GetComponent<AudioManager>().PauseAudio();
        }

        clones = GameObject.FindGameObjectsWithTag("Clone");

        foreach (GameObject clone in clones)
        {
            clone.GetComponent<AudioManager>().PauseAudio();
        }
    }

    public void UnmuteSound()
    {

        for (int i = 0; i < players.Count; i++)
        {
            players[i].GetComponent<AudioManager>().UnpauseAudio();
        }

        clones = GameObject.FindGameObjectsWithTag("Clone");

        foreach (GameObject clone in clones)
        {
            clone.GetComponent<AudioManager>().UnpauseAudio();
        }

    }

}
