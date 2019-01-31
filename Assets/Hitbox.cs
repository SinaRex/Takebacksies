using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColliderState
{
    Closed, Open, Colliding
}

public class Hitbox : MonoBehaviour
{
    //Hitbox Dimensions
    Vector3 hitboxPosition;
    Vector3 hitboxSize;
    Quaternion hitboxRotation;

    //Hitbox State
    private ColliderState _state;

    //Other Hitbox Variables
    private float hitboxTimer = 0f;

    //Input Variables for tracking location
    private Vector3    hitboxPosOffset;


    // Activate the hitbox with the given parameters
    public void startHitbox(Vector3 posOffsetIn, Quaternion rotationIn, Vector3 sizeIn)
    {
        // If a hitbox is already active, return
        if (_state == ColliderState.Open || _state == ColliderState.Colliding) return;

        //Set hitbox shape and relative position
        hitboxPosOffset = posOffsetIn;

        hitboxPosition = transform.position + hitboxPosOffset;
        hitboxRotation = rotationIn;
        hitboxSize = sizeIn;

        hitboxTimer = 0.2f;

        _state = ColliderState.Open;
    }


    // Called when hitbox deactivated
    void endHitbox() {

        //Reset Hitbox Size
        hitboxPosition = transform.position;
        hitboxSize = new Vector3(0.1f, 0.1f, 0.1f);
        hitboxRotation = transform.rotation;
        hitboxTimer = 2f;

        _state = ColliderState.Closed;
    }


    private void Update()
    {
        if (_state == ColliderState.Closed) return;

        //Update Hitbox location to follow player
        hitboxPosition = transform.position + hitboxPosOffset;
        hitboxRotation = transform.rotation;

        //Decrement move up-time timer
        hitboxTimer -= Time.deltaTime;

        //Check for collision
        Collider[] colliders = Physics.OverlapBox(hitboxPosition, hitboxSize, hitboxRotation, LayerMask.GetMask("Hurtbox"));

        if (colliders.Length == 0) _state = ColliderState.Open;

        foreach (Collider c in colliders)
        {
            //Do not count collisions with yourself
            if (c.transform.root == transform)
                continue;

            //FIXME: for now, just apply knock back, will compartmentalize this later
            _state = ColliderState.Colliding;

            Debug.Log(c.name);
            c.transform.root.GetComponent<Rigidbody>().velocity = new Vector3(10, 10, 0);
        }


        //Check if move is finished
        if (hitboxTimer <= 0f)
        {
            endHitbox();
            hitboxTimer = 0f;
        }
    }


    //Visual indicator for hitbox locations
    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        switch (_state)
        {
            case ColliderState.Closed:
                Gizmos.color = Color.yellow;
                break;

            case ColliderState.Open:
                Gizmos.color = Color.blue;
                break;

            case ColliderState.Colliding:
                Gizmos.color = Color.red;
                break;
        }

        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, transform.localScale);
        Gizmos.DrawCube(Vector3.zero, new Vector3(hitboxSize.x * 2 / transform.localScale.x, hitboxSize.y * 2 / transform.localScale.y, hitboxSize.z * 2));
    }
}
