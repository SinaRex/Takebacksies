using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LunarCatsStudio.SuperRewinder;

public class PlayerController : MonoBehaviour
{
    private Rewind3DObject[] m_rewinders;
    public float jumpSpeed = 4;
    public float movementSpeed = 10;
    public float groundingDistance = 0.5f;
    public float timeTravelWaitTime = 2f;
    public float parryCooldown = 1f;
    public float parryDuration = 0.5f;

    //Control Variables
    private bool isGrounded;
    private bool isRewinding;
    private float horizontalDirection = 0;
    private float verticalDirection = 0;
    private float horizontalFightDirection = 0;
    private float verticalFightDirection = 0;
    private float directionSign = 1;
    private float directionMagnitude = 0;
    private int extraJumpsLeft = 3;
    private float echoCooldownTimer = 0f;
    private float parryCooldownTimer = 0f;


    //Player Character Variables
    private TBInput playerInput;

    private Rigidbody playerBody;

    private PlayerManager playerManager;

    private ControllerHandler controllerHandler;

    // Echo related variables
    public GameObject characterPrefab = null;
    private GameObject characterEcho = null;

    // FIXME: DELETE THIS AFTER ALPHA
    private bool makeSmoke1 = false;

    // Start is called before the first frame update
    void Start()
    {
        controllerHandler = GameObject.Find("ControllerHandler").GetComponent<ControllerHandler>();

        m_rewinders = FindObjectsOfType<Rewind3DObject>();
        playerBody = gameObject.GetComponent<Rigidbody>();

        isRewinding = false;
        //Control the fall speed
        Physics.gravity = new Vector3(0, -15.0F, 0);
    }

    private void FixedUpdate()
    {
        //----------Get Player manager related information ---------------
        playerManager = transform.GetComponent<PlayerManager>();

        if (playerManager.playerIdentity != PlayerIdentity.Echo)
        {
            // Managing Movement Inputs
            if (playerManager.playerIdentity == PlayerIdentity.Player1) playerInput = controllerHandler.input1;
            else playerInput = controllerHandler.input2;
        }
        else {
            playerInput = playerManager.getNextEchoRecording();
        }

        //-----------Update control paramters----------------
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundingDistance, LayerMask.GetMask("Stage"));
        if (isGrounded) extraJumpsLeft = 3;

        if (echoCooldownTimer > 0) echoCooldownTimer -= Time.fixedDeltaTime;
        else echoCooldownTimer = 0;

        if (parryCooldownTimer > 0) parryCooldownTimer -= Time.fixedDeltaTime;
        else parryCooldownTimer = 0;
        
        //FIXME Can customize movemenet sensetivity here
        horizontalDirection = 500 * playerInput.MoveAxisX;
        verticalDirection = -500 * playerInput.MoveAxisY;
        horizontalFightDirection = 500 * playerInput.FightAxisX;
        verticalFightDirection = -500 * playerInput.FightAxisY;


        if (playerManager.GetState() == PlayerState.Dead || playerManager.GetState() == PlayerState.Respawning || playerManager.GetState() == PlayerState.TimeTravelling) return;


        //----------- Process Player Inputs --------------
        if ((playerInput.jumpButton ) && (extraJumpsLeft > 0))
        {
            //Limit extra jumps in the air
            if (!isGrounded) extraJumpsLeft--;

            //FIXME: Make paramterizable
            //playerBody.velocity = (Vector3.up * jumpSpeed * 1.3f);
            playerBody.velocity = new Vector3(playerBody.velocity.x,  jumpSpeed * 1.3f, 0f); //FIXME UPDATED

        }

        //Parry 
        if (playerInput.ParryButton && parryCooldownTimer <= 0) {
            parryCooldownTimer = parryCooldown;
            playerManager.StartParrying(parryDuration);
            GetComponent<Animator>().SetTrigger("Parry");
        }

        else if (playerInput.NormalButton && (Mathf.Abs(horizontalDirection) < 0.5) && (Mathf.Abs(verticalDirection) < 0.5))
            transform.GetComponent<MoveList>().jab();

        else if ((playerInput.NormalButton && (Mathf.Abs(horizontalDirection) > 0.5)) || (Mathf.Abs(horizontalFightDirection) > 0.5))
            transform.GetComponent<MoveList>().Forward_Normal();

        else if ((playerInput.NormalButton && (verticalDirection > 0.5)) || verticalFightDirection > 0.5)
            transform.GetComponent<MoveList>().Up_Normal();

        else if ((playerInput.NormalButton && (verticalDirection < -0.5)) || verticalFightDirection < -0.5)
            transform.GetComponent<MoveList>().Down_Normal();

        if (playerInput.SpecialButton) 
            transform.GetComponent<MoveList>().Neutral_Special();
        


        //Time travel
        if (playerInput.RewindButton && (playerManager.GetWhichPlayer() != PlayerIdentity.Echo) && playerManager.getTimeJuice() > 3) {

            Debug.Log("PRessing Y");
            // ---------- FIXME: DELETE THIS AFTER ALPHA--------//
            if (!makeSmoke1)
            {
                if (playerManager.GetWhichPlayer() == PlayerIdentity.Player1)
                {
                    GameObject.Find("CloudEffect1").transform.position = transform.position;
                    GameObject.Find("CloudEffect1").GetComponentInChildren<ParticleSystem>().Play();
                }
                else
                {
                    GameObject.Find("CloudEffect2").transform.position = transform.position;
                    GameObject.Find("CloudEffect2").GetComponentInChildren<ParticleSystem>().Play();
                }

                makeSmoke1 = true;
            }
            // ---------- FIXME: DELETE THIS AFTER ALPHA--------//

            StartCoroutine(TravelBackInTime());
        }


        //Different Movemetn options depending on player state
        if (playerManager.GetState() == PlayerState.InHitStun) playerBody.AddForce(new Vector3(horizontalDirection / 5, 0f, 0f), ForceMode.Force); //DI
        else if (playerManager.GetState() == PlayerState.GroundAttack || playerManager.GetState() == PlayerState.Parrying /*FIXME THIS PREVENTS MOVEMENT DURING ARIAL PARRY*/) playerBody.velocity = Vector3.zero; // Can't move while attacking
        else if (playerManager.GetState() == PlayerState.TimeTravelling) playerBody.velocity = Vector3.zero; // Can't move while in timetravel
        else if ((playerManager.GetState() == PlayerState.Airborne || playerManager.GetState() == PlayerState.ArialAttack)  
            && Mathf.Abs(playerBody.velocity.x) <= movementSpeed)
            playerBody.AddForce(new Vector3(horizontalDirection * movementSpeed / 25, 0f, 0f), ForceMode.VelocityChange);
        else 
        {
            //FIXME: UPDATE THE ANIMATIONS FOR DASHING AND WALKING HERE
            //Define Direction
            if (horizontalDirection > 0) directionSign = 1;
            else directionSign = -1;

            //Defining step function speed, either dashing or walking, no continuous spectrum
            if (Mathf.Abs(horizontalDirection) > 0.9) directionMagnitude = movementSpeed;
            else if (Mathf.Abs(horizontalDirection) > 0) directionMagnitude = movementSpeed / 3;
            else directionMagnitude = 0;


            //playerBody.velocity = new Vector3(horizontalDirection * movementSpeed, playerBody.velocity.y, 0f); // Move normally
            playerBody.velocity = new Vector3(directionSign*directionMagnitude, playerBody.velocity.y, 0f); // Move normally
        }
    }


    //-------------  Time Travel Related Functions: All --------------//

    private IEnumerator TravelBackInTime() {

        //Enable TimeTravel State
        playerManager.StartTimeTravelling();

        //Fetcth the recording up until you pressed the reqind button
        Queue<TBInput> inputRecording = new Queue<TBInput>(controllerHandler.getRecording(playerManager.GetWhichPlayer()));

        //Set you up in proper position
        //transform.position = playerManager.getRecordedPosition();
        List<positionalData> positionalDatas = playerManager.getRecordPositionList();
        for (int i = positionalDatas.Count - 1; i >= 0; i -= 5)
        {
            transform.position = positionalDatas[i].position;
            //Debug.Log(positionalDatas[i].position);
            yield return new WaitForSeconds(0.0000001f);
        }

        // ---------- FIXME: DELETE THIS AFTER ALPHA--------//
        if (playerManager.GetWhichPlayer() == PlayerIdentity.Player1)
        {
            GameObject.Find("CloudEffect1_2").transform.position = transform.position;
            GameObject.Find("CloudEffect1_2").GetComponentInChildren<ParticleSystem>().Play();
        }
        else
        {
            GameObject.Find("CloudEffect2_2").transform.position = transform.position;
            GameObject.Find("CloudEffect2_2").GetComponentInChildren<ParticleSystem>().Play();
        }

        // ---------- FIXME: DELETE THIS AFTER ALPHA--------//

        yield return new WaitForSeconds(timeTravelWaitTime);


        playerManager.StopTimeTravelling();

        //playerBody.AddForce(playerManager.getRecordedVelocity(), ForceMode.VelocityChange);

        createEcho(inputRecording);

    }

    // Creates a timeclone of this character
    private void createEcho(Queue<TBInput> inputRecording) {

        if (echoCooldownTimer > 0) return;

        characterEcho = Instantiate(characterPrefab, transform.position, transform.rotation);
        characterEcho.GetComponent<PlayerManager>().setupEcho(gameObject, inputRecording);

        // FIXME: DELE THIS AFTER ALPHA
        makeSmoke1 = false;

        echoCooldownTimer = 3f;
    }

    //Getters and Setters
    public float getHorizontalInput() {
        return horizontalDirection;
    }

    public float getHorizontalFightInput()
    {
        return horizontalFightDirection;
    }
}
