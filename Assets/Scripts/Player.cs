﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LunarCatsStudio.SuperRewinder;

public class Player : MonoBehaviour
{
    public Rewind3DObject rewinder;
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
        rewinder = FindObjectOfType<Rewind3DObject>();
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

        if (Input.GetKeyDown(KeyCode.C) && !isRewinding)
        {
            rewinder.StartRewind();
            isRewinding = true;
        } else if (Input.GetKeyDown(KeyCode.C))
        {
            rewinder.StopRewind();
            isRewinding = false;
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