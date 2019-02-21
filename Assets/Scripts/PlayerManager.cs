using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerState
{
    Start,
    Idle, Walking, Dashing, Airborne,
    InHitStun, GroundAttack, ArialAttack,
    Dead, Respawning, Invincible
}


public enum PlayerIdentity
{
    Player1, Player2, Echo1, Echo2
}


public enum Orientation
{
    Right = 0,
    Left = 180
}

public class PlayerManager : MonoBehaviour
{ 
    //State Variables
    private PlayerState _state;
    private PlayerState nextState;

    //Character Variables
    private float playerPercent = 0f;
    private float hitStunTimer = 0f;
    private float attackingTimer = 0f;
    public PlayerIdentity whichPlayer; // TODO: change this to private

    //Movement Variables
    private bool isGrounded;
    private float horizontalInput;
    private Orientation playerOrientation;

    private bool canRespawn = false;
    private bool isDead = false;
    private bool canBeInvinvible = false;

    //Static Variables
    private static float threshold = 0.5f;


    void Start()
    {
        _state = PlayerState.Start;
        nextState = PlayerState.Idle;
    }

    void FixedUpdate()
    {

        //-------  Character/Input Management ------//

        horizontalInput = transform.GetComponent<Player>().getHorizontalInput();

        //FIXME: This is jank
        if (horizontalInput > 0) playerOrientation = Orientation.Right;
        else if (horizontalInput < 0) playerOrientation = Orientation.Left;

        isGrounded = Physics.Raycast(transform.position, Vector3.down, transform.GetComponent<Player>().groundingDistance, LayerMask.GetMask("Stage"));

        //Decrementing Timers
        if (hitStunTimer > 0f) hitStunTimer -= Time.deltaTime;
        else hitStunTimer = 0;

        if (attackingTimer > 0f) attackingTimer -= Time.deltaTime;
        else attackingTimer = 0;


        //Other Player Managings
        transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, (float)playerOrientation, 0));

        //--------- Player State Machine-----------//
        //Update Next State
        _state = nextState;

        //Update States
        switch (_state) {
            case PlayerState.Idle:
                if (isDead) nextState = PlayerState.Dead;
                else if (hitStunTimer > 0) nextState = PlayerState.InHitStun;
                else if (attackingTimer > 0) nextState = PlayerState.GroundAttack;
                else if (!isGrounded) nextState = PlayerState.Airborne;
                else if (horizontalInput > 0 && horizontalInput < threshold) nextState = PlayerState.Walking;
                else if (horizontalInput > threshold) nextState = PlayerState.Dashing;
                else nextState = PlayerState.Idle;
                break;

            case PlayerState.Walking:
                if (isDead) nextState = PlayerState.Dead;
                else if (hitStunTimer > 0) nextState = PlayerState.InHitStun;
                else if (attackingTimer > 0) nextState = PlayerState.GroundAttack;
                else if (!isGrounded) nextState = PlayerState.Airborne;
                else if (horizontalInput > 0 && horizontalInput < threshold) nextState = PlayerState.Walking;
                else if (horizontalInput > threshold) nextState = PlayerState.Dashing;
                else nextState = PlayerState.Idle;
                break;

            case PlayerState.Dashing:
                //FIXME May need a transition state for turnaround lag
                if (isDead) nextState = PlayerState.Dead;
                else if (hitStunTimer > 0) nextState = PlayerState.InHitStun;
                else if (attackingTimer > 0) nextState = PlayerState.GroundAttack;
                else if (!isGrounded) nextState = PlayerState.Airborne;
                else if (horizontalInput > 0 && horizontalInput < threshold) nextState = PlayerState.Walking;
                else if (horizontalInput > threshold) nextState = PlayerState.Dashing;
                else nextState = PlayerState.Idle;
                break;

            case PlayerState.Airborne:
                //FIXME Add Landing lag transition Stage
                if (isDead) nextState = PlayerState.Dead;
                else if (hitStunTimer > 0) nextState = PlayerState.InHitStun;
                else if (attackingTimer > 0) nextState = PlayerState.ArialAttack;
                else if (!isGrounded) nextState = PlayerState.Airborne;
                else nextState = PlayerState.Idle;
                break;

            case PlayerState.InHitStun:
                //FIXME Let characters Attack out of here
                if (isDead) nextState = PlayerState.Dead;
                else if (hitStunTimer > 0) nextState = PlayerState.InHitStun;
                else if (!isGrounded) nextState = PlayerState.Airborne;
                else nextState = PlayerState.Idle;
                break;

            case PlayerState.GroundAttack:
                //FIXME: Add Move Lag Transition state
                if (isDead) nextState = PlayerState.Dead;
                else if (hitStunTimer > 0) { nextState = PlayerState.InHitStun; GetComponent<MoveList>().interruptMove(); }
                else if (attackingTimer > 0) nextState = PlayerState.GroundAttack;
                else nextState = PlayerState.Idle;
                break;

            case PlayerState.ArialAttack:
                //FIXME: Add landing lag transition State
                if (isDead) nextState = PlayerState.Dead;
                else if (hitStunTimer > 0) { nextState = PlayerState.InHitStun; GetComponent<MoveList>().interruptMove(); }
                else if (isGrounded) { nextState = PlayerState.Idle; GetComponent<MoveList>().interruptMove(); }
                else if (attackingTimer > 0) nextState = PlayerState.ArialAttack;
                else nextState = PlayerState.Airborne;
                break;

            case PlayerState.Dead:
                //Debug.Log("he is Dead");
                if (canRespawn) { nextState = PlayerState.Respawning; canRespawn = false; isDead = false;}
                else nextState = PlayerState.Dead;
                break;

            case PlayerState.Respawning:
                //Debug.Log("he is Respawning");
                if (canBeInvinvible) nextState = PlayerState.Invincible;
                else nextState = PlayerState.Respawning;
                break;

            case PlayerState.Invincible:
                //Debug.Log("he is Invincible");
                if (isDead) nextState = PlayerState.Dead;
                nextState = PlayerState.Invincible;
                break;

        }

        //Debug.Log(_state);

    }


    /*-------------------START: PlayZone & BlastZone Logic---------------------*/
    private void OnTriggerExit(Collider other)
    {
        // The reaosn we check if the current state is not respawning state
        // is because when respawning on the plaform at the top, a partial
        // part of the player's body might be outside of the playBox.
        if (other.gameObject.CompareTag("PlayZone") &&
                 _state != PlayerState.Respawning) { 
            Die();

            }
                
    }
    /*-------------------END: PlayZone & BlastZone Logic-----------------------*/


    //-------External Functions------//
    // State Getters and Setters

    public PlayerState GetState()
    {
        return _state;
    }


    //Getters, Setters, and incrementers for character variables and inputs
    public void setHitStun(float inputStun)
    {
        hitStunTimer = inputStun;
    }


    public void addDamage(float inputDamage)
    {
        playerPercent += inputDamage;
    }


    public void StartAttacking(float attackLength) {
        attackingTimer = attackLength;
    }


    public void Respawn() {
        isDead = false;
        canRespawn = true;
    }

    public void Die()
    {
        isDead = true;
    }

    public void GoInvincible()
    {
        canBeInvinvible = true;
    }


    public PlayerIdentity GetWhichPlayer()
    {
        return whichPlayer;
    }

    public Orientation getPlayerOrientation(){
        return playerOrientation;
    }
}
