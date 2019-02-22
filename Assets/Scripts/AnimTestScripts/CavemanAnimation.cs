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

    private float finalRotaiton = 180;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerOrientation = Orientation.Right;
    }

    private void FixedUpdate()
    {


        float h = Input.GetAxisRaw("HorizontalTesting"); // only -1, 0 and 1 (not a range)
        float v = Input.GetAxisRaw("Vertical");
        Orientation oldOrientation = playerOrientation;
        if (h > 0) playerOrientation = Orientation.Right;
        else if (h < 0) playerOrientation = Orientation.Left;

        Move(h, v);
        if (oldOrientation != playerOrientation)
        {
            StartCoroutine(Rotate(Vector3.up, 180, 0.25f));
        }  

        Animating(h, v);

    }

    private void Move(float h, float v)
    {
        movement.Set(h, 0f, 0f);
        // deltatime is the time between each update call.
        movement = movement.normalized * speed * Time.deltaTime;

        playerRigidbody.MovePosition(transform.position + movement);
    }

    private void Turning(float h)
    {
        if (h > 0)
        {

        }
        //if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        //{
        //    Vector3 playerToMouse = floorHit.point - transform.position;
        //    playerToMouse.y = 0f;
        //    Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
        //    playerRigidbody.MoveRotation(newRotation);
        //}
    }

    private void Animating(float h, float v)
    {
        bool running = h != 0f;
        anim.SetBool("isRunning", running);
    }

    IEnumerator Rotate(Vector3 axis, float angle, float duration = 1.0f)
    {
        Quaternion from = transform.rotation;
        Quaternion to = transform.rotation;
        to *= Quaternion.Euler(axis * angle);

        float elapsed = 0.0f;
        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = to;
    }
}
