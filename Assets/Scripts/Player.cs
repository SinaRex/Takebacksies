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
    float smooth = 7.0f;
    float tiltAngle = -30.0f;
    private Canvas canvas;

    //Player Character Variables
    private float playerPercent = 0f;
    private float hitStunTimer = 0f;

    //Movement Variables
    private bool isGrounded;
    private bool isRewinding;
    private float horizontalDirection = 0;
    private int extraJumpsLeft = 2;

    private Rigidbody playerBody;

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

        //Basic hitstun
        if (hitStunTimer > 0f) hitStunTimer -= Time.deltaTime;
        else if (hitStunTimer < 0) hitStunTimer = 0;

        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundingDistance);
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

        if (hitStunTimer <= 0) playerBody.velocity = new Vector3(horizontalDirection * movementSpeed, playerBody.velocity.y, 0f);
        else playerBody.AddForce (new Vector3(horizontalDirection/5, 0f, 0f), ForceMode.Impulse);
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

    //Getters, Setters, and incrementers
    public void setHitStun(float inputStun) {
        hitStunTimer = inputStun;
    }

    public void addDamage(float inputDamage) {
        playerPercent += inputDamage;
    }
}
