using System;
using System.Linq;
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
    public Orientation orientation;

    public positionalData(Vector3 positionIn, Vector3 velocityIn, Orientation orientationIn) {
        position = positionIn;
        velocity = velocityIn;
        orientation = orientationIn;
    }
}

public class PlayerManager : MonoBehaviour
{ 
    //State Variables
    private PlayerState _state;
    private PlayerState nextState;

    //Character Variables
    private float playerPercent = 0f;
    private float maxTimeJuice = 15f;
    private float timeJuice = 0f;

    //Character Management Variables
    private float hitStunTimer = 0f;
    private float attackingTimer = 0f;
    private float parryTimer = 0f;
    private float lastHitByTimer = 0f;
    public PlayerIdentity playerIdentity; // TODO: change this to private
    private Transform lastHitBy = null;

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
    private const float threshold = 0.9f;

    //Echo Related Variables
    public GameObject characterPrefab = null;


    //Variables used inside character echoes 
    private Transform echoParent = null;
    private Queue<TBInput> echoInputRecording;
    private List<positionalData> echoPositionalDataRecording;
    public Material echoMat;
    public int MAXCLONES = 1;
    private int echoLevel = 0;


    //Time Travel Variables
    private int recordingDuration = 1;
    private int recordingLimit;
    private int recordingCount;
    private GameObject characterEcho = null;
    private List<positionalData> positionalDataRecording = new List<positionalData>();
    private Queue<TBInput> echoInputChildRecording = new Queue<TBInput>();



    //Animator
    Animator playerAnimator;

    //Controller
    private ControllerHandler controllerHandler;

    void Start()
    {
        controllerHandler = GameObject.Find("ControllerHandler").GetComponent<ControllerHandler>();

        //Player State machine Vars
        _state = PlayerState.Start;
        nextState = PlayerState.Idle;

        //Time travel vars
        recordingLimit = recordingDuration * 50;

        playerAnimator = GetComponent<Animator>();

        //MAXCLONES = GameModeSelector.PlayerCloneCount;
    }


    void FixedUpdate()
    {
        //---------------  Time Travel Management --------------//

        if (playerIdentity == PlayerIdentity.Echo) {
            updateEchoInputChildRecording();

            MAXCLONES = getEchoRoot().GetComponent<PlayerManager>().MAXCLONES;

            if (echoLevel > MAXCLONES) {
                Die();
            }
            else if(echoLevel < MAXCLONES) //Only clones with echoLevel >= MAXCLONES do not have trail renderers
                transform.GetChild(2).GetComponent<TrailRenderer>().enabled = true;
            else
                transform.GetChild(2).GetComponent<TrailRenderer>().enabled = false;
        }


        //transform.GetComponent<TimeTravelManager>().UpdatePersistentClone();
        if ( echoLevel < MAXCLONES/*playerIdentity != PlayerIdentity.Echo*/) {
            if (_state != PlayerState.Dead && _state != PlayerState.Respawning)
            {

                Queue<TBInput> inputQueue = (playerIdentity == PlayerIdentity.Echo) ? new Queue<TBInput>(echoInputChildRecording) : new Queue<TBInput>(controllerHandler.getRecording(playerIdentity));

                if (characterEcho == null)
                {
                    transform.GetChild(2).transform.GetChild(1).gameObject.GetComponent<Renderer>().enabled = true;
                    createEcho(inputQueue, positionalDataRecording);
                }
                else
                {
                    characterEcho.GetComponent<PlayerManager>().updateEcho(transform, inputQueue, positionalDataRecording);
                }
            }
        }




        //---------------  Updating UI --------------//
        FindObjectOfType<TimeJuiceUI>().updateUI(playerIdentity, timeJuice / maxTimeJuice);
        FindObjectOfType<PercentageUI>().UpdateUI(playerIdentity);


        //---------------  Character/Input Management --------------//
        horizontalInput = transform.GetComponent<PlayerController>().getHorizontalInput();
        horizontalFightInput = transform.GetComponent<PlayerController>().getHorizontalFightInput();

        Orientation oldOrientation = playerOrientation; // SMOOTH TURNING
        //FIXME: FIXES JUMPING ROTATIONS
        if (_state != PlayerState.ArialAttack /*&& _state != PlayerState.Airborne */ && _state != PlayerState.GroundAttack && _state != PlayerState.Parrying && _state != PlayerState.TimeTravelling)
        {
            if (horizontalFightInput > 0.25 && _state != PlayerState.Airborne) playerOrientation = Orientation.Right;
            else if (horizontalFightInput < -0.25 && _state != PlayerState.Airborne) playerOrientation = Orientation.Left;
            else if (horizontalInput > 0) playerOrientation = Orientation.Right;
            else if (horizontalInput < 0) playerOrientation = Orientation.Left;
        }

        isGrounded = (Physics.Raycast(transform.position, Vector3.down, transform.GetComponent<PlayerController>().groundingDistance, LayerMask.GetMask("Stage")) || Physics.Raycast(transform.position, Vector3.down, transform.GetComponent<PlayerController>().groundingDistance, LayerMask.GetMask("Platform")));

        //-------------------Managing Counters ------------------//
        if (hitStunTimer > 0f) hitStunTimer -= Time.deltaTime;
        else hitStunTimer = 0;

        if (attackingTimer > 0f) attackingTimer -= Time.deltaTime;
        else attackingTimer = 0;

        if (parryTimer > 0f) parryTimer -= Time.deltaTime;
        else parryTimer = 0;

        if (lastHitByTimer > 0f) lastHitByTimer -= Time.deltaTime;
        else { lastHitByTimer = 0; lastHitBy = null; }


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
                else if (isGrounded) { nextState = PlayerState.Idle; GetComponent<MoveList>().interruptMove(); StopAttacking(); }
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
            positionalDataRecording.Add(new positionalData(transform.position, transform.GetComponent<Rigidbody>().velocity, playerOrientation));
            recordingCount += 1;
        }
        else
        {
            positionalDataRecording.RemoveAt(0);
            positionalDataRecording.Add(new positionalData(transform.position, transform.GetComponent<Rigidbody>().velocity, playerOrientation));
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
            if (GameObject.FindWithTag(BlastSelect))
                GameObject.FindWithTag(BlastSelect).transform.LookAt(new Vector3(0, 5, -6.7f));

            Die();


            }
                
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
    }

    public void testAddDamage()
    {
        playerPercent += 5f;
    }

    public void StartAttacking(float attackLength) {
        attackingTimer = attackLength;
    }
    public void StopAttacking()
    {
        attackingTimer = 0;
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

        //FIXME: beta
        transform.GetChild(2).GetComponent<TrailRenderer>().Clear();
        transform.GetChild(2).GetComponent<TrailRenderer>().enabled = false;


        //Reward the player who killed you
        if (lastHitBy != null) lastHitBy.GetComponent<PlayerManager>().AddTimeJuice(2);

    }

    public void FinishRespawning()
    {
        canMoveAfterDeath = true;
        //FIXME: this should probably not be here lol FIXME
        resetPositionalData();

        //FIXME: beta
        transform.GetChild(2).GetComponent<TrailRenderer>().enabled = true;

        GameObject.Find("ControllerHandler").GetComponent<ControllerHandler>().resetPlayerInputData(playerIdentity);
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
            timeJuice -= 3; // FIXME: timeJuice -3;
        }

        // FIXME: AFTER ALPHA
    }

    public void StopTimeTravelling()
    {
        // FIXME: 
        resetPositionalData();
        GameObject.Find("ControllerHandler").GetComponent<ControllerHandler>().resetPlayerInputData(playerIdentity);
        isTimeTravelling = false;
    }

    public void SetTimeJuice(float num)
    {
        timeJuice = (num < 15f) ? num : 15f;
    }

    public void AddTimeJuice(float num)
    {
        timeJuice += num;
        timeJuice = (timeJuice < 15f) ? timeJuice : 15f;
    }

    public void updateLastHitBy(Transform Assaulter) {
        lastHitBy = Assaulter;
        lastHitByTimer = 10.0f;
    }

    public void setPlayerOrientation(Orientation orientation)
    {
        playerOrientation = orientation;
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

    public Transform getLastHitBy()
    {
        return lastHitBy;
    }

    public Transform getEchoParent(){
        return echoParent;
    }

    public Transform getEchoRoot(){

        if (playerIdentity != PlayerIdentity.Echo) return null;

        Transform echoParentIterator = echoParent;

        while (echoParentIterator.GetComponent<PlayerManager>().GetWhichPlayer() == PlayerIdentity.Echo) {
            echoParentIterator = echoParentIterator.GetComponent<PlayerManager>().getEchoParent();
        }

        return echoParentIterator;
    }

    public GameObject getCharacterEcho() {
        return characterEcho;
    }

    public int getRecordingLimit() {
        return recordingLimit;
    }

    //-------------  Time Travel Related Functions: Parents --------------//
    // Creates a timeclone of this character
    public void createEcho(Queue<TBInput> inputRecording, List<positionalData> inputPositionalDataRecording)
    {

        characterEcho = Instantiate(characterPrefab, transform.position, transform.rotation);
        characterEcho.GetComponent<PlayerManager>().setupEcho(transform, inputRecording, inputPositionalDataRecording, echoLevel+1);

    }


    //-------------  Time Travel Related Functions: Echos --------------//

    public void setupEcho(Transform inputEchoParent, Queue<TBInput> inputEchoRecording, List<positionalData> inputPositionalData, int inputEchoLevel)
    {
        playerIdentity = PlayerIdentity.Echo;
        echoParent = inputEchoParent;
        echoLevel = inputEchoLevel;
        echoInputRecording = new Queue<TBInput>(inputEchoRecording);
        echoPositionalDataRecording = new List<positionalData>(inputPositionalData);
        playerPercent = 200;

        // *** Set the material to the special one ****//
        if(echoLevel == MAXCLONES)
            transform.GetChild(2).GetComponent<TrailRenderer>().enabled = false;
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

        transform.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.tag = "Clone";
    }

    
    public void updateEcho(Transform inputEchoParent, Queue<TBInput> inputEchoRecording, List<positionalData> inputPositionalData)
    {
        if (inputEchoParent != echoParent) return;
        echoInputRecording = inputEchoRecording;
        echoPositionalDataRecording = inputPositionalData;

}


    //-------------  Time Travel Related Functions: Parents--------------//
    public Vector3 getRecordedPosition() {
         return positionalDataRecording[0].position;
    }

    public Vector3 getRecordedVelocity() {
         return positionalDataRecording[0].position;
    }

    public Orientation getRecordedOrientation()
    {
         return positionalDataRecording[0].orientation;
    }

    public List<positionalData> getRecordPositionList()
    {
        return new List<positionalData>(positionalDataRecording);
    }

    public void resetPositionalData() {
        recordingCount = 0;
        positionalDataRecording.Clear();
    }

    private void updateEchoInputChildRecording() {

        if (echoInputRecording.Count >= recordingLimit)
        {
            if (echoInputChildRecording.Count < recordingLimit)
            {
                echoInputChildRecording.Enqueue(echoInputRecording.ElementAt(0));
            }
            else
            {
                echoInputChildRecording.Dequeue();
                echoInputChildRecording.Enqueue(echoInputRecording.ElementAt(0));
            }
        }
    }


    //-------------  Time Travel Related Functions: Echoes --------------//

    public TBInput getNextEchoRecording()
    {
        if (echoInputRecording.Count == 0) return new TBInput(0f, 0f, 0f, 0f, false, false, false, false, false);
        else return echoInputRecording.Dequeue();
    }

    public Queue<TBInput> getAllInputEchoRecording()
    {
        return echoInputRecording;
    }

    public List<positionalData> getPositionEchoRecording()
    {
        return echoPositionalDataRecording;
    }

}
