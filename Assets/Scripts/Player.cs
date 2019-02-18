using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LunarCatsStudio.SuperRewinder;

public class Player : MonoBehaviour
{
    private Rewind3DObject[] m_rewinders;
    public float jumpSpeed = 4;
    public float movementSpeed = 10;
    public float groundingDistance = 1f;
    public bool isPlayer1 = false;
    private Canvas canvas;


    //Movement Variables
    private bool isGrounded;
    private bool isRewinding;
    private float horizontalDirection = 0;
    private int extraJumpsLeft = 2;

    private Rigidbody playerBody;
    private PlayerManager playerManager;

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
    }

    private void FixedUpdate()
    {

        playerManager = transform.GetComponent<PlayerManager>();

        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundingDistance, LayerMask.GetMask("Stage"));
        if (isGrounded) extraJumpsLeft = 2;

        // Movement Inputs
        if (isPlayer1)
        {

            horizontalDirection = Input.GetAxis("Horizontal1");
            if (Input.GetButtonDown("Jump1") && (extraJumpsLeft > 0))
            {
                //Limit extra jumps in the air
                if (!isGrounded) extraJumpsLeft--;
                //FIXME: Make paramterizable
                playerBody.velocity = (Vector3.up * jumpSpeed * 1.3f);
            }

            if (Input.GetKeyDown(KeyCode.E))
                transform.GetComponent<MoveList>().jab();


            if (Input.GetAxis("Fire1") > 0 || Input.GetKeyDown(KeyCode.Q))
                transform.GetComponent<MoveList>().Up_Normal();

            if (Input.GetKeyDown(KeyCode.R))
                transform.GetComponent<MoveList>().Forward_Normal();

        }
        else {
            horizontalDirection = Input.GetAxis("Horizontal2");
            if (Input.GetButtonDown("Jump2") && (extraJumpsLeft > 0))
            {
                //Limit extra jumps in the air
                if (!isGrounded) extraJumpsLeft--;
                //FIXME: Make paramterizable
                playerBody.velocity = (Vector3.up * jumpSpeed * 1.3f);
            }
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
