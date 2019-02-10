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

    private float horizontalDirection = 0;
    private float isJumping;

    private bool isGrounded;
    private bool isRewinding;
    private bool canDoubleJump;

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

        canDoubleJump = false;
    }

    private void FixedUpdate()
    {

        // Movement Inputs
        if (isPlayer1)
        {

            horizontalDirection = Input.GetAxis("Horizontal1");
            isJumping = Input.GetAxis("Jump1");
            if (Input.GetButtonDown("Jump1"))
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.Q))
                GetComponent<Hitbox>().startHitbox(new List<Vector3>() { Vector3.zero }, new List<Quaternion>() { transform.rotation },
                    new List<Vector3>() { new Vector3(1f, 0.3f, 0.1f) }, new List<float>() { 0.2f }, 0);

            if (Input.GetAxis("Fire1") > 0 || Input.GetKeyDown(KeyCode.E))
                GetComponent<Hitbox>().startHitbox(new List<Vector3>() { Vector3.zero, Vector3.zero, Vector3.zero },
                    new List<Quaternion>() { Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, -45), Quaternion.Euler(0, 0, -90) },
                    new List<Vector3>() { new Vector3(0.2f, 2f, 0.1f), new Vector3(0.2f, 2f, 0.1f), new Vector3(0.2f, 2f, 0.1f) },
                    new List<float>() { 0.1f, 0.1f, 0.1f }, 3);
        
        }
        else {
            horizontalDirection = Input.GetAxis("Horizontal2");
            isJumping = Input.GetAxis("Jump2");
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

        //playerBody.velocity = new Vector3(horizontalDirection * movementSpeed, playerBody.velocity.y, 0f);
        playerBody.AddForce (new Vector3(horizontalDirection, 0f, 0f), ForceMode.VelocityChange);
    }

    /// <summary>
    /// Starts the rewind for all RewindObject in scene.
    /// </summary>
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

    /// <summary>
    /// Stops the rewind for all RewindObject in scene.
    /// </summary>
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

    void Jump()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundingDistance);

        if (isGrounded)
        {
            playerBody.velocity = (Vector3.up * jumpSpeed*1.3f);
            canDoubleJump = true;
        } 
        else if (canDoubleJump)
        {
            playerBody.velocity = (Vector3.up * jumpSpeed*1.3f);
            canDoubleJump = false;
        }
    }
}
