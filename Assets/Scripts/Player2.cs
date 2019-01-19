using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{

    public float jumpSpeed = 4;
    public float movementSpeed = 2;
    public float groundingDistance = 1f;
    float smooth = 7.0f;
    float tiltAngle = -30.0f;

    private float horizontalDirection = 0;
    private float tiltAroundZ;

    private bool isGrounded;

    private Rigidbody playerBody;


    // Start is called before the first frame update
    void Start()
    {
        playerBody = gameObject.GetComponent<Rigidbody>();

        //Control the fall speed
        Physics.gravity = new Vector3(0, -15.0F, 0);

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        // Smoothly tilts a transform towards a target rotation.
        tiltAroundZ = Input.GetAxis("Horizontal2") * tiltAngle;
        horizontalDirection = Input.GetAxis("Horizontal2");

        Quaternion target = Quaternion.Euler(0, 0, tiltAroundZ);

        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundingDistance);

        if (isGrounded && Input.GetAxis("Jump2") > 0.01)
        {
            playerBody.velocity = (Vector3.up * jumpSpeed);
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
}
