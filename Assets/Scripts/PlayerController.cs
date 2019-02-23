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


    //Movement Variables
    private bool isGrounded;
    private bool isRewinding;
    private float horizontalDirection = 0;
    private float verticalDirection = 0;
    private int extraJumpsLeft = 2;


    //Player Character Variables
    private TBInput playerInput;

    private Rigidbody playerBody;

    private PlayerManager playerManager;

    Animator playerAnimator;


    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
        canvas.enabled = false;
        m_rewinders = FindObjectsOfType<Rewind3DObject>();
        playerBody = gameObject.GetComponent<Rigidbody>();

        isRewinding = false;
        //Control the fall speed
        Physics.gravity = new Vector3(0, -15.0F, 0);

        playerAnimator = GetComponent<Animator>();
    }


    private void FixedUpdate()
    {

        playerManager = transform.GetComponent<PlayerManager>();

        // Managing Movement Inputs
        if (playerManager.whichPlayer == PlayerIdentity.Player1) playerInput = GameObject.Find("ControllerHandler").GetComponent<ControllerHandler>().input1;
        else playerInput = GameObject.Find("ControllerHandler").GetComponent<ControllerHandler>().input2;

        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundingDistance, LayerMask.GetMask("Stage"));
        if (isGrounded) extraJumpsLeft = 2;

        //FIXME Can customize movemenet sensetivity here
        horizontalDirection = 500 * playerInput.MoveAxisX;
        verticalDirection = -500 * playerInput.MoveAxisY;

        if ((playerInput.jumpButton ) && (extraJumpsLeft > 0))
        {
            //Limit extra jumps in the air
            if (!isGrounded) extraJumpsLeft--;

            //FIXME: Make paramterizable
            playerBody.velocity = (Vector3.up * jumpSpeed * 1.3f);
        }

        if (playerInput.NormalButton && (Mathf.Abs(horizontalDirection) < 0.5) && (verticalDirection < 0.5)){
            playerAnimator.SetTrigger("Jab");
            transform.GetComponent<MoveList>().jab();
        }

        else if (playerInput.NormalButton && (Mathf.Abs(horizontalDirection) > 0.5)) {
            playerAnimator.SetTrigger("Smash");
            transform.GetComponent<MoveList>().Forward_Normal();
        }

        else if (playerInput.NormalButton && (verticalDirection > 0.5)){
            transform.GetComponent<MoveList>().Up_Normal();
        }
        if (playerInput.SpecialButton && (Mathf.Abs(horizontalDirection) > 0.5)) {
            transform.GetComponent<MoveList>().Forward_Special();
        }

        


        //Time travel Inputs
        if (Input.GetKeyDown(KeyCode.X))
        {
            StartRewind();
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            StopRewind();
        }

        //Different Movemetn options depending on player state
        if (playerManager.GetState() == PlayerState.InHitStun) playerBody.AddForce(new Vector3(horizontalDirection / 5, 0f, 0f), ForceMode.Force); //DI
        else if (playerManager.GetState() == PlayerState.GroundAttack) playerBody.velocity = Vector3.zero; // Can't move while attacking
        else if (playerManager.GetState() == PlayerState.Airborne || playerManager.GetState() == PlayerState.ArialAttack) playerBody.AddForce(new Vector3(horizontalDirection / 3, 0f, 0f), ForceMode.Impulse);
        else playerBody.velocity = new Vector3(horizontalDirection * movementSpeed, playerBody.velocity.y, 0f); // Move normally

    }

    // Starts the rewind for all RewindObject in scene.
    void StartRewind()
    {
        if (!isRewinding)
        {
            isRewinding = true;
            canvas.enabled = true;

            //send start rewind for all rewind object in the current scene
            foreach (Rewind3DObject rewinder in m_rewinders)
            {
                rewinder.StartRewind();
            }
        }
    }

    // Stops the rewind for all RewindObject in scene.
    void StopRewind()
    {
        if (isRewinding)
        {
            canvas.enabled = false;
            isRewinding = false;

            //send stop rewind for all rewind object in the current scene
            foreach (Rewind3DObject rewinder in m_rewinders)
            {
                rewinder.StopRewind();
            }
        }
    }


    //Getters and Setters
    public float getHorizontalInput() {
        return horizontalDirection;
    }
}
