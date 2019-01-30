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
        // Movement Inputs
        if (isPlayer1)
        {
            horizontalDirection = Input.GetAxis("Horizontal1");
            isJumping = Input.GetAxis("Jump1");

            //Attack Inputs
            if (Input.GetKeyDown(KeyCode.E))
                Attack();
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



        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundingDistance); 

        if (isGrounded && isJumping > 0.01) {
            playerBody.velocity  = (Vector3.up*jumpSpeed);
        }

        playerBody.velocity = new Vector3(horizontalDirection * movementSpeed, playerBody.velocity.y, 0f);
        //playerBody.AddForce (new Vector3(horizontalDirection * movementSpeed, 0f, 0f));
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

    void Attack() {
        Vector3 hitboxPosition = transform.position;
        Vector3 hitboxSize = new Vector3(1f,0.5f,0.1f);
        Quaternion hitboxRotation = transform.rotation;

        Collider[] colliders = Physics.OverlapBox(hitboxPosition, hitboxSize, hitboxRotation, LayerMask.GetMask("Hurtbox"));

        foreach (Collider c in colliders) {
            if (c.transform.root == transform)
                continue;

            //for now, just apply knock back, will compartmentalize this later
            Debug.Log(c.name);
            c.transform.root.GetComponent<Rigidbody>().velocity = new Vector3(10,10,0);
            //c.transform.root.GetComponent<Rigidbody>().AddForce( new Vector3(100, 100, 0));

        }
    }

    void OnDrawGizmos()
    {
        Vector3 hitboxPosition = transform.position;
        Vector3 hitboxSize = new Vector3(1f, 0.5f, 0.1f);
        Quaternion hitboxRotation = transform.rotation;

        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        Gizmos.DrawCube(Vector3.zero, new Vector3(hitboxSize.x * 2/transform.localScale.x, hitboxSize.y * 2 / transform.localScale.y, hitboxSize.z * 2));
    }
}
