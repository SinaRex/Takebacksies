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
    private Canvas canvas;


    //Control Variables
    private bool isGrounded;
    private bool isRewinding;
    private float horizontalDirection = 0;
    private float verticalDirection = 0;
    private int extraJumpsLeft = 3;
    private float echoTimer = 0f;

    //Player Character Variables
    private TBInput playerInput;

    private Rigidbody playerBody;

    private PlayerManager playerManager;

    private ControllerHandler controllerHandler;

    // Echo related variables
    public GameObject characterPrefab = null;
    private GameObject characterEcho = null;
    public Material echoMat;

    // Start is called before the first frame update
    void Start()
    {
        controllerHandler = GameObject.Find("ControllerHandler").GetComponent<ControllerHandler>();

        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
        canvas.enabled = false;
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
            playerInput = playerManager.getNextRecording();
        }

        //-----------Update control paramters----------------
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundingDistance, LayerMask.GetMask("Stage"));
        if (isGrounded) extraJumpsLeft = 3;

        if (echoTimer > 0) echoTimer-=Time.fixedDeltaTime;
        else echoTimer = 0;

        //FIXME Can customize movemenet sensetivity here
        horizontalDirection = 500 * playerInput.MoveAxisX;
        verticalDirection = -500 * playerInput.MoveAxisY;


        //----------- Process Player Inputs --------------
        if ((playerInput.jumpButton ) && (extraJumpsLeft > 0))
        {
            //Limit extra jumps in the air
            if (!isGrounded) extraJumpsLeft--;

            //FIXME: Make paramterizable
            playerBody.velocity = (Vector3.up * jumpSpeed * 1.3f);
        }

        if (playerInput.NormalButton && (Mathf.Abs(horizontalDirection) < 0.5) && (verticalDirection < 0.5))
            transform.GetComponent<MoveList>().jab();  

        else if (playerInput.NormalButton && (Mathf.Abs(horizontalDirection) > 0.5)) 
            transform.GetComponent<MoveList>().Forward_Normal();   

        else if (playerInput.NormalButton && (verticalDirection > 0.5))
            transform.GetComponent<MoveList>().Up_Normal();
        
        if (playerInput.SpecialButton) 
            transform.GetComponent<MoveList>().Neutral_Special();
        


        //Time travel
        if (playerInput.RewindButton && (playerManager.GetWhichPlayer() != PlayerIdentity.Echo)) {
            createEcho();
        }


        //Different Movemetn options depending on player state
        if (playerManager.GetState() == PlayerState.InHitStun) playerBody.AddForce(new Vector3(horizontalDirection / 5, 0f, 0f), ForceMode.Force); //DI
        else if (playerManager.GetState() == PlayerState.GroundAttack) playerBody.velocity = Vector3.zero; // Can't move while attacking
        else if (playerManager.GetState() == PlayerState.Airborne || playerManager.GetState() == PlayerState.ArialAttack) playerBody.AddForce(new Vector3(horizontalDirection / 3, 0f, 0f), ForceMode.Impulse);
        else playerBody.velocity = new Vector3(horizontalDirection * movementSpeed, playerBody.velocity.y, 0f); // Move normally

    }

    // Creates a timeclone of this character
    private void createEcho() {

        if (echoTimer > 0) return;

        characterEcho = Instantiate(characterPrefab, transform.position, transform.rotation);

        // *** Set the material to the special one ****//
        GameObject echoModel = characterEcho.transform.GetChild(2).gameObject;// get CharacterModel gameobject
        echoModel.transform.GetChild(1).gameObject.GetComponent<Renderer>().sharedMaterial = echoMat;// set the caveman.001's material to echoMat

        characterEcho.GetComponent<PlayerManager>().setupEcho(gameObject, controllerHandler.getRecording(playerManager.GetWhichPlayer()));

        echoTimer = 3f;
    }

    //Getters and Setters
    public float getHorizontalInput() {
        return horizontalDirection;
    }
}
