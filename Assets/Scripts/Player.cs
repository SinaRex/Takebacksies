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

    private float horizontalDirection = 0;
    private float tiltAroundZ;
    private float isJumping;

    private bool isGrounded;
    private bool isRewinding;

    private Rigidbody playerBody;

    // Start is called before the first frame update
    void Start()
    {
        m_rewinders = FindObjectsOfType<Rewind3DObject>();
        playerBody = gameObject.GetComponent<Rigidbody>();

        isRewinding = false;
        //Control the fall speed
        Physics.gravity = new Vector3(0, -15.0F, 0);
    }

    private void FixedUpdate()
    {
        // Smoothly tilts a transform towards a target rotation.
        if (isPlayer1)
        {
            tiltAroundZ = Input.GetAxis("Horizontal1") * tiltAngle;
            horizontalDirection = Input.GetAxis("Horizontal1");
            isJumping = Input.GetAxis("Jump1"); 
        }
        else {
            tiltAroundZ = Input.GetAxis("Horizontal2") * tiltAngle;
            horizontalDirection = Input.GetAxis("Horizontal2");
            isJumping = Input.GetAxis("Jump2");
        }

        Quaternion target = Quaternion.Euler(0, 0, tiltAroundZ);

        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundingDistance); 

        if (isGrounded && isJumping > 0.01) {
            playerBody.velocity = (Vector3.up*jumpSpeed);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            StartRewind();
        } else if (Input.GetKeyDown(KeyCode.C))
        {
            StopRewind();
        }

        // Add real looking tilt physics
        if (transform.rotation.z <= 20)
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.fixedDeltaTime * smooth);
        else
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, 20), Time.fixedDeltaTime * smooth);
    
        if (transform.rotation.x <= 20)
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.fixedDeltaTime * smooth);
        else
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(20, 0, 0), Time.fixedDeltaTime * smooth);

        playerBody.velocity = new Vector3(horizontalDirection * movementSpeed, playerBody.velocity.y, 0f);

    }

    /// <summary>
    /// Starts the rewind for all RewindObject in scene.
    /// </summary>
    void StartRewind()
    {
        if (isRewinding == false)
        {
            isRewinding = true;

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
        if (isRewinding == true)
        {
            isRewinding = false;

            //send stop rewind for all rewind object in the current scene
            foreach (Rewind3DObject rewinder in m_rewinders)
            {
                rewinder.StopRewind();
            }
        }
    }
}
