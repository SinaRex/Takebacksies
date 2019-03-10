using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerState
{
    Start,
    Idle, Walking, Dashing, Airborne,
    InHitStun, GroundAttack, ArialAttack, Parrying,
    Dead, Respawning, Invincible, TimeTravelling
}


public enum PlayerIdentity
{
    Player1, Player2, Echo
}

public enum Orientation
{
    Right = 0,
    Left = 180
}

public struct positionalData {

    public Vector3 position;
    public Vector3 velocity;

    public positionalData(Vector3 positionIn, Vector3 velocityIn) {
        position = positionIn;
        velocity = velocityIn;
    }
}

public class PlayerManager : MonoBehaviour
{ 
    //State Variables
    private PlayerState _state;
    private PlayerState nextState;

    //Character Variables
    private float playerPercent = 0f;
    private float timeJuice = 15f;
    private float maxTimeJuice = 15f;

    //Character Management Variables
    private float hitStunTimer = 0f;
    private float attackingTimer = 0f;
    private float parryTimer = 0f;
    public PlayerIdentity playerIdentity; // TODO: change this to private

    //Movement Variables
    private bool isGrounded;
    private float horizontalInput;
    private float horizontalFightInput;
    private Orientation playerOrientation;


    //State Management flags
    private bool canRespawn = false;
    private bool isDead = false;
    private bool canMoveAfterDeath = false;

    // Current state flags for GameManager so that it is a trigger and not a state
    private bool isDying = false;
    private bool isRespawning = false;
    private bool isInvincible = false;
    private bool isTimeTravelling = false;

    //Constant Variables
    private const float threshold = 0.5f;

    //Echo Character related varibles
    private GameObject echoParent = null;
    private Queue<TBInput> echoRecording;
    public Material echoMat;


    //Time Travel Variables
    public int recordingDuration = 3;
    private int recordingLimit;
    private int recordingCount;
    //private Queue<positionalData> positionalDataRecording = new Queue<positionalData>();
    private List<positionalData> positionalDataRecording = new List<positionalData>();



    //Animator
    Animator playerAnimator;


    void Start()
    {
        //Player State machine Vars
        _state = PlayerState.Start;
        nextState = PlayerState.Idle;

        //Time travel vars
        recordingLimit = recordingDuration * 50;

        playerAnimator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {

        //---------------  Character/Input Management --------------//

        horizontalInput = transform.GetComponent<PlayerController>().getHorizontalInput();
        horizontalFightInput = transform.GetComponent<PlayerController>().getHorizontalFightInput();

        Orientation oldOrientation = playerOrientation; // SMOOTH TURNING
        //FIXME: This is jank
        if (_state != PlayerState.ArialAttack && _state != PlayerState.GroundAttack && _state != PlayerState.Parrying)
        {
            if (horizontalFightInput > 0.25) playerOrientation = Orientation.Right;
            else if (horizontalFightInput < -0.25) playerOrientation = Orientation.Left;
            else if (horizontalInput > 0) playerOrientation = Orientation.Right;
            else if (horizontalInput < 0) playerOrientation = Orientation.Left;
        }

        isGrounded = Physics.Raycast(transform.position, Vector3.down, transform.GetComponent<PlayerController>().groundingDistance, LayerMask.GetMask("Stage"));

        //-------------------Managing Counters ------------------//
        if (hitStunTimer > 0f) hitStunTimer -= Time.deltaTime;
        else hitStunTimer = 0;

        if (attackingTimer > 0f) attackingTimer -= Time.deltaTime;
        else attackingTimer = 0;

        if (parryTimer > 0f) parryTimer -= Time.deltaTime;
        else parryTimer = 0;

        if (_state == PlayerState.Dead) playerPercent = 0f;

        //Time Travel Managings
        trackPositionalData();

        //Other Player Managings
        transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, (float)playerOrientation, 0));

        //----------------------- Player State Machine --------------------------//
        //Update Next State
        _state = nextState;

        //Update States
        switch (_state) {
            case PlayerState.Idle:
                if (isDead) nextState = PlayerState.Dead;
                else if (hitStunTimer > 0) nextState = PlayerState.InHitStun;
                else if (attackingTimer > 0) nextState = PlayerState.GroundAttack;
                else if (parryTimer > 0) nextState = PlayerState.Parrying;
                else if (isTimeTravelling) nextState = PlayerState.TimeTravelling;
                else if (!isGrounded) nextState = PlayerState.Airborne;
                else if (Mathf.Abs(horizontalInput) > 0 && Mathf.Abs(horizontalInput) < threshold) nextState = PlayerState.Walking;
                else if (Mathf.Abs(horizontalInput) > threshold) nextState = PlayerState.Dashing;
                else nextState = PlayerState.Idle;
                break;

            case PlayerState.Walking:
                if (isDead) nextState = PlayerState.Dead;
                else if (hitStunTimer > 0) nextState = PlayerState.InHitStun;
                else if (attackingTimer > 0) nextState = PlayerState.GroundAttack;
                else if (parryTimer > 0) nextState = PlayerState.Parrying;
                else if (isTimeTravelling) nextState = PlayerState.TimeTravelling;
                else if (!isGrounded) nextState = PlayerState.Airborne;
                else if (Mathf.Abs(horizontalInput) > 0 && Mathf.Abs(horizontalInput) < threshold) nextState = PlayerState.Walking;
                else if (Mathf.Abs(horizontalInput) > threshold) nextState = PlayerState.Dashing;
                else nextState = PlayerState.Idle;
                break;

            case PlayerState.Dashing:
                //FIXME May need a transition state for turnaround lag
                if (isDead) nextState = PlayerState.Dead;
                else if (hitStunTimer > 0) nextState = PlayerState.InHitStun;
                else if (attackingTimer > 0) nextState = PlayerState.GroundAttack;
                else if (parryTimer > 0) nextState = PlayerState.Parrying;
                else if (isTimeTravelling) nextState = PlayerState.TimeTravelling;
                else if (!isGrounded) nextState = PlayerState.Airborne;
                else if (Mathf.Abs(horizontalInput) > 0 && Mathf.Abs(horizontalInput) < threshold) nextState = PlayerState.Walking;
                else if (Mathf.Abs(horizontalInput) > threshold) nextState = PlayerState.Dashing;
                else nextState = PlayerState.Idle;
                break;

            case PlayerState.Airborne:
                //FIXME Add Landing lag transition Stage
                if (isDead) nextState = PlayerState.Dead;
                else if (hitStunTimer > 0) nextState = PlayerState.InHitStun;
                else if (attackingTimer > 0) nextState = PlayerState.ArialAttack;
                else if (parryTimer > 0) nextState = PlayerState.Parrying;
                else if (isTimeTravelling) nextState = PlayerState.TimeTravelling;
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


            case PlayerState.Parrying:
                //FIXME: Add Move Lag Transition state
                if (isDead) nextState = PlayerState.Dead;
                else if (hitStunTimer > 0) nextState = PlayerState.InHitStun;
                else if (parryTimer > 0) nextState = PlayerState.Parrying;
                else nextState = PlayerState.Idle;
                break;

            case PlayerState.Dead:
                //Debug.Log("he is Dead");
                if (canRespawn) { nextState = PlayerState.Respawning; canRespawn = false; isDead = false; }
                else nextState = PlayerState.Dead;
                break;

            case PlayerState.Respawning:
                //Debug.Log("he is Respawning");
                //FIXME: Add invincible state
                if (canMoveAfterDeath) {nextState = PlayerState.Idle; canMoveAfterDeath = false; }
                else nextState = PlayerState.Respawning;
                break;

            case PlayerState.Invincible:
                if (isDead) nextState = PlayerState.Dead;
                nextState = PlayerState.Invincible;
                break;

            case PlayerState.TimeTravelling:
                if (isTimeTravelling) nextState = PlayerState.TimeTravelling;
                else nextState = PlayerState.Idle;
                break;

        }

        //Debug.Log(_state);


        //--------------------------ANIMATIONS-------------------------//
        if (_state == PlayerState.Dashing) playerAnimator.SetBool("isRunning", true);
        else playerAnimator.SetBool("isRunning", false);

        if (_state == PlayerState.Walking) playerAnimator.SetBool("isWalking", true);
        else playerAnimator.SetBool("isWalking", false);
    }


    //-------------------------- Helper Functions -----------------------------//
    /* private void smoothTurn(Orientation oldOrientation)
     {

         Vector3 from = transform.rotation.eulerAngles;
         if (oldOrientation != playerOrientation)
         {

             timeSinceRotation = 0.0f;
             rotationGoalAngle = new Vector3(0f, (float)playerOrientation, 0f);
             isRotating = true;

         }
         if (isRotating)
         {
             from = Vector3.Lerp(from, rotationGoalAngle, timeSinceRotation / 0.25f);
             transform.rotation = Quaternion.Euler(from);
             timeSinceRotation += Time.deltaTime;
             if (timeSinceRotation > 0.25f)
             {
                 isRotating = false;
             }
         }


     }*/

    private void trackPositionalData() {

        //Used to track the postion an trajectory of the player over the most recent 3 seconds
        if (recordingCount < recordingLimit)
        {
            //positionalDataRecording.Enqueue(new positionalData(transform.position, transform.GetComponent<Rigidbody>().velocity));
            positionalDataRecording.Add(new positionalData(transform.position, transform.GetComponent<Rigidbody>().velocity));
            recordingCount += 1;
        }
        else
        {
            //positionalDataRecording.Dequeue();
            positionalDataRecording.RemoveAt(0);
            //positionalDataRecording.Enqueue(new positionalData(transform.position, transform.GetComponent<Rigidbody>().velocity));
            positionalDataRecording.Add(new positionalData(transform.position, transform.GetComponent<Rigidbody>().velocity));
        }
    }


    private IEnumerator FlashOnce(float duration)
    {
        Debug.Log("Parrying!");

        int i = 0;
        GameObject playerModel = transform.GetChild(2).gameObject;// get CharacterModel gameobject
        Color preset = playerModel.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color;
        while (i < duration) {
            yield return new WaitForSeconds(0.125f);
            playerModel.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color = Color.blue;
            yield return new WaitForSeconds(0.125f);
            playerModel.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color = preset;
            i++;
        }


    }

    //-------------------PlayZone & BlastZone Logic--------------------//
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

    private void OnTriggerEnter(Collider other)
    {
        string BlastSelect;
        if (playerIdentity == PlayerIdentity.Player1)
        {
            BlastSelect = "blast1";
        }
        else if (playerIdentity == PlayerIdentity.Player2)
        {
            BlastSelect = "blast2";
        }
        else
        {
            return;
        }
        GameObject.FindWithTag(BlastSelect).transform.LookAt(new Vector3(0, 1, -5.45f));
    }

    //-------------------------External Functions-----------------------------//
    //Getters, Setters, and incrementers for character variables and inputs

    //-------------- Setters --------------
    public void setHitStun(float inputStun)
    {
        hitStunTimer = inputStun;
    }

    public void addDamage(float inputDamage)
    {
        playerPercent += inputDamage;
        if (playerIdentity == PlayerIdentity.Player1)
        {
           // FindObjectOfType<PercentageUI>().UpdateUI(true);
        }
        else if (playerIdentity == PlayerIdentity.Player2)
        {
           // FindObjectOfType<PercentageUI>().UpdateUI(false);
        }
    }

    public void StartAttacking(float attackLength) {
        attackingTimer = attackLength;
    }

    public void StartParrying(float parryLength)
    {
        parryTimer = parryLength;
        StartCoroutine(FlashOnce(parryLength));
    }

    public void Respawn() {
        //isDead = false;
        canRespawn = true;
    }

    public void Die()
    {
        if (playerIdentity == PlayerIdentity.Echo) Destroy(gameObject);
        else isDead = true;
    }

    public void FinishRespawning()
    {
        canMoveAfterDeath = true;
        //FIXME: this should probably not be here lol FIXME
        resetPositionalData();
        GameObject.Find("ControllerHandler").GetComponent<ControllerHandler>().resetPositionalData(playerIdentity);
    }

    public void SetIsDying(bool flag)
    { 
        isDying = flag;
    }

    public void SetIsRespawning(bool flag)
    {
        isRespawning = flag;
    }

    public void SetIsInvincible(bool flag)
    {
        isInvincible = flag;
    }

    public void StartTimeTravelling() {

        // FIXME: AFTER ALPHA
        if (_state == PlayerState.Dead || _state == PlayerState.Respawning || _state == PlayerState.Invincible)
            isTimeTravelling = false;

        else
        {
            isTimeTravelling = true;
            timeJuice -= 1; // FIXME: timeJuice -3;

            //Updating time juice UI
            if (playerIdentity == PlayerIdentity.Player1)
            {
                FindObjectOfType<TimeJuiceUI>().updateUI(true, timeJuice / maxTimeJuice);
            }
            else if (playerIdentity == PlayerIdentity.Player2)
            {
                FindObjectOfType<TimeJuiceUI>().updateUI(false, timeJuice / maxTimeJuice);
            }

        }

        // FIXME: AFTER ALPHA
    }

    public void StopTimeTravelling()
    {
        // FIXME: 
        resetPositionalData();
        GameObject.Find("ControllerHandler").GetComponent<ControllerHandler>().resetPositionalData(playerIdentity);
        isTimeTravelling = false;
    }

    public void SetTimeJuice(int num)
    {
        timeJuice = num;
    }


    //-------------- Getters --------------

    public float getPercent() {
        return playerPercent;
    }

    public float getTimeJuice() {
        return timeJuice;
    }

    public bool IsDying()
    {
        return isDying;
    }

    public bool IsRespawning()
    {
        return isRespawning;
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }

    public PlayerState GetState()
    {
        return _state;
    }

    public PlayerIdentity GetWhichPlayer()
    {
        return playerIdentity;
    }

    public Orientation getPlayerOrientation(){
        return playerOrientation;
    }


    //-------------  Time Travel Related Functions: Echos --------------//

    public void setupEcho(GameObject inputEchoParent, Queue<TBInput> inputEchoRecording)
    {
        playerIdentity = PlayerIdentity.Echo;
        echoParent = inputEchoParent;
        echoRecording = inputEchoRecording;
        playerPercent = 200;

        // *** Set the material to the special one ****//
        GameObject echoModel = transform.GetChild(2).gameObject;// get CharacterModel gameobject
        Renderer echoRenderer = echoModel.transform.GetChild(1).gameObject.GetComponent<Renderer>();

        Material[] newMats = new Material[echoRenderer.materials.Length];
        for (int i = 0; i < newMats.Length; i++)
        {
            newMats[i] = echoMat;
        }
        echoRenderer.materials = newMats;

        // Enable the Halo effect from the prefab
        Behaviour halo = (Behaviour)echoModel.GetComponent("Halo");
        halo.enabled = true;

    }

    public TBInput getNextEchoRecording()
    {
        if (echoRecording.Count == 0) return new TBInput(0f, 0f, 0f, 0f, false, false, false, false, false);
        else return echoRecording.Dequeue();
    }

    //-------------  Time Travel Related Functions: Echos --------------//
    public Vector3 getRecordedPosition() {
        //return positionalDataRecording.Peek().position;
        return positionalDataRecording[0].position;
    }

    public Vector3 getRecordedVelocity() {
        //return positionalDataRecording.Peek().velocity;
        return positionalDataRecording[0].position;
    }

    public List<positionalData> getRecordPositionList()
    {
        return new List<positionalData>(positionalDataRecording);
    }

    public void resetPositionalData() {
        recordingCount = 0;
        positionalDataRecording.Clear();
    }


}
