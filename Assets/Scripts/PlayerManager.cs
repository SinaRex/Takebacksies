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

public class PlayerManager : MonoBehaviour
{ 
    //State Variables
    private PlayerState _state;
    private PlayerState nextState;


    //Character Variables
    private float playerPercent = 0f;
    private float hitStunTimer = 0f;
    private float attackingTimer = 0f;

    private bool isGrounded;
    private float horizontalInput;

    private bool canRespawn = false;
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

        isGrounded = Physics.Raycast(transform.position, Vector3.down, transform.GetComponent<Player>().groundingDistance, LayerMask.GetMask("Stage"));

        //Decrementing Timers
        if (hitStunTimer > 0f) hitStunTimer -= Time.deltaTime;
        else hitStunTimer = 0;

        if (attackingTimer > 0f) attackingTimer -= Time.deltaTime;
        else attackingTimer = 0;

        //--------- Player State Machine-----------//
        //Update Next State
        _state = nextState;

        //Update States
        switch (_state) {
            case PlayerState.Idle:
                if (hitStunTimer > 0) nextState = PlayerState.InHitStun;
                else if (attackingTimer > 0) nextState = PlayerState.GroundAttack;
                else if (!isGrounded) nextState = PlayerState.Airborne;
                else if (horizontalInput > 0 && horizontalInput < threshold) nextState = PlayerState.Walking;
                else if (horizontalInput > threshold) nextState = PlayerState.Dashing;
                else nextState = PlayerState.Idle;
                break;

            case PlayerState.Walking:
                if (hitStunTimer > 0) nextState = PlayerState.InHitStun;
                else if (attackingTimer > 0) nextState = PlayerState.GroundAttack;
                else if (!isGrounded) nextState = PlayerState.Airborne;
                else if (horizontalInput > 0 && horizontalInput < threshold) nextState = PlayerState.Walking;
                else if (horizontalInput > threshold) nextState = PlayerState.Dashing;
                else nextState = PlayerState.Idle;
                break;

            case PlayerState.Dashing:
                //FIXME May need a transition state for turnaround lag
                if (hitStunTimer > 0) nextState = PlayerState.InHitStun;
                else if (attackingTimer > 0) nextState = PlayerState.GroundAttack;
                else if (!isGrounded) nextState = PlayerState.Airborne;
                else if (horizontalInput > 0 && horizontalInput < threshold) nextState = PlayerState.Walking;
                else if (horizontalInput > threshold) nextState = PlayerState.Dashing;
                else nextState = PlayerState.Idle;
                break;

            case PlayerState.Airborne:
                //FIXME Add Landing lag transition Stage
                if (hitStunTimer > 0) nextState = PlayerState.InHitStun;
                else if (attackingTimer > 0) nextState = PlayerState.ArialAttack;
                else if (!isGrounded) nextState = PlayerState.Airborne;
                else nextState = PlayerState.Idle;
                break;

            case PlayerState.InHitStun:
                //FIXME Let characters Attack out of here
                if (hitStunTimer > 0) nextState = PlayerState.InHitStun;
                else if (!isGrounded) nextState = PlayerState.Airborne;
                else nextState = PlayerState.Idle;
                break;

            case PlayerState.GroundAttack:
                //FIXME: Add Move Lag Transition state
                if (hitStunTimer > 0) { nextState = PlayerState.InHitStun; GetComponent<MoveList>().interruptMove(); }
                else if (attackingTimer > 0) nextState = PlayerState.GroundAttack;
                else nextState = PlayerState.Idle;
                break;

            case PlayerState.ArialAttack:
                //FIXME: Add landing lag transition State
                if (hitStunTimer > 0) { nextState = PlayerState.InHitStun; GetComponent<MoveList>().interruptMove(); }
                else if (isGrounded) { nextState = PlayerState.Idle; GetComponent<MoveList>().interruptMove(); }
                else if (attackingTimer > 0) nextState = PlayerState.ArialAttack;
                else nextState = PlayerState.Airborne;
                break;

            case PlayerState.Dead:
                if (canRespawn) { nextState = PlayerState.Respawning; canRespawn = false; }
                else nextState = PlayerState.Dead;
                break;

            case PlayerState.Respawning:
                nextState = PlayerState.Invincible;
                break;

            case PlayerState.Invincible:
                nextState = PlayerState.Invincible;
                break;

        }

        //Debug.Log(_state);

    }
    

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
        canRespawn = true;
    }

}
