using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    /* Can be changed in Unity Editor */
    public float gameTimerRemaining = 600f;

    /* Maximum lives that each player can have*/
    public int maxLives = 3;

    /* Respawn Duration*/
    public int respawnDuration = 2;

    /* List of players GameObject*/
    private List<GameObject> players = new List<GameObject>();

    /* List of lives that correspond to the players in the list players.*/
    private List<int> playersLives = new List<int>();

    /* Respawn Platform (DRAG AND DROP)*/
    public GameObject respawnPlatform;

    /* Enum */
    private enum whichPlayer {Player1, Player2, Echo1, Echo2}

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

        // TODO: Initialize the map related stuff (e.g. respawnPlatforms) 
    }


    /**
     * Update is called for each frame.
     */
    private void Update()
    {
        for (int i = 0; i < players.Count; i++)
        {
            switch (players[i].GetComponent<PlayerManager>().GetState())
            {
                case PlayerState.Dead:
                    playersLives[i] -= 1;
                    if (playersLives[i] <= 0)

                    {
                        GameOver();
                    }
                    else
                    {
                        players[i].GetComponent<PlayerManager>().Respawn();
                        StartCoroutine(RespawnPlayer(players[i]));
                    }
                    break;
                default:
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

        if (gameTimerRemaining < 0)
        {
            GameOver();
        }
    }


    /**
     * when the game is over, show who the winner is
     * or if it's a tie. Then probably have a button to go 
     * back to the menu.
     */
    private void GameOver()
    {
        // TODO: have a nice UI implementation to do this.
        Debug.Log("GameOver");
    }


    /**
     * Respawn animation where the player is respawned on the top of a platform
     */
    private IEnumerator RespawnPlayer(GameObject player)
    {
        // Freeze/stop the player.
        player.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        // Play the blast animation.
        player.GetComponentInChildren<ParticleSystem>().Play();
        StartCoroutine(MakePlayerInvincible());

        // Wait two (or customized) second before respawning.
        yield return new WaitForSeconds(respawnDuration);

        GameObject platform = MakeRespawnPlatform(player); 

        platform.GetComponent<Rigidbody>().isKinematic = false;


        if (platform)
        {
            for (int i = 0; i < 30; i++)
            {
                yield return new WaitForFixedUpdate();
                platform.GetComponent<Rigidbody>().AddForce(new Vector3(0, -4.8f, 0));
                GetComponent<Rigidbody>().AddForce(new Vector3(0, -4.8f, 0));
            }
            platform.GetComponent<Rigidbody>().isKinematic = true;

            yield return new WaitForSeconds(0.5f);
            platform.GetComponent<BoxCollider>().enabled = false;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None |
                RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
            Destroy(platform);


        }
        else
        {
            // TODO: deal with the echo players
            Debug.LogError("UnImplemented For echo Players");
        }

    }


    private GameObject MakeRespawnPlatform(GameObject player)
    {

        if (player.GetComponent<PlayerManager>().GetWhichPlayer() == whichPlayer.Player1)
        {
           return Instantiate(respawnPlatform,
                            GameObject.Find("RespawnPoint1").transform.position,
                            Quaternion.identity);
        }

        else if (player.GetComponent<PlayerManager>().GetWhichPlayer() == whichPlayer.Player2)
        {
            return Instantiate(respawnPlatform,
                            GameObject.Find("RespawnPoint1").transform.position,
                            Quaternion.identity);
        }

        else
        {
            return null;
        }
    }

    //private IEnumerator MakePlayerInvincible()
    //{

    //}

}
