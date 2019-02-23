using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CavemanAnimation : MonoBehaviour
{

    public float speed = 6f;

    Vector3 movement;
    Animator anim;
    Rigidbody playerRigidbody;

    private Orientation playerOrientation;
    private float horizontalInput;

    private float sinceTime = 0.0f;
    private bool isRotating = false;
    Vector3 to;

    private int extraJumps = 3;
    private bool isGrounded;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerOrientation = Orientation.Right;

        to = transform.eulerAngles;

        Physics.gravity = new Vector3(0, -40f, 0);
    }

    private void FixedUpdate()
    {


        float h = Input.GetAxis("MoveAxisX1"); // only -1, 0 and 1 (not a range)
        float v = Input.GetAxisRaw("MoveAxisY1");

        Orientation oldOrientation = playerOrientation;
        if (h > 0) playerOrientation = Orientation.Right;
        else if (h < 0) playerOrientation = Orientation.Left;

        Move(h, v);
        Turning(oldOrientation);

        isGrounded = Physics.Raycast(transform.position, Vector3.down, 2.5f);
        if (isGrounded)
        {
            extraJumps = 3;
        }

        if (Input.GetButtonDown("Jump1") && (extraJumps > 0))
        {
            //Limit extra jumps in the air
            if (!isGrounded) extraJumps--;
            //FIXME: Make paramterizable
            playerRigidbody.velocity = (Vector3.up * 15f);
        }

        if (Input.GetKeyDown(KeyCode.Q))
            anim.SetTrigger("Jab");

        if (Input.GetKeyDown(KeyCode.R))
            anim.SetTrigger("Smash");

        Animating(h, v);

    }

    private void Move(float h, float v)
    {
        movement.Set(h, 0f, 0f);
        // deltatime is the time between each update call.
        movement = movement.normalized * speed * Time.deltaTime;

        playerRigidbody.MovePosition(transform.position + movement);
    }

    private void Turning(Orientation oldOrientation)
    {
        Vector3 from = transform.rotation.eulerAngles;
        if (oldOrientation != playerOrientation)
        {
            if (playerOrientation == Orientation.Left)
            {
                sinceTime = 0.0f;
                to = new Vector3(0f, 0f, 0f);
                isRotating = true;
            }
            else
            {
                sinceTime = 0.0f;
                to = new Vector3(0f, 180f, 0f);
                isRotating = true;
            }
        }
        if (isRotating)
        {
            from = Vector3.Lerp(from, to, sinceTime / 0.25f);
            transform.rotation = Quaternion.Euler(from);
            sinceTime += Time.deltaTime;
            if (sinceTime > 0.25f)
            {
                isRotating = false;
            }
        }


    }

    private void Animating(float h, float v)
    {
        bool running = h != 0f;
        anim.SetBool("isRunning", running);
    }

    //IEnumerator Rotate(Vector3 axis, float angle, float duration = 1.0f)
    //{
    //    Quaternion from = transform.rotation;
    //    Quaternion to = transform.rotation;
    //    to *= Quaternion.Euler(axis * angle);

    //    float elapsed = 0.0f;
    //    while (elapsed < duration)
    //    {
    //        transform.rotation = Quaternion.Slerp(from, to, elapsed / duration);
    //        elapsed += Time.deltaTime;
    //        yield return null;
    //    }
    //    //transform.rotation = to;
    //}
}
