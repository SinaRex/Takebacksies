using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColliderState
{
    Closed, Open, Colliding
}

public class Hitbox : MonoBehaviour
{
    //Input Variables for tracking size
    private List<Vector3> hitboxSizeList;
    private List<Quaternion> hitboxRotOffset;
    private List<Vector3> hitboxPosOffset;

    //Other input Hitbox Variables
    private List<float> hitboxTimer = new List<float>() { 0f };

    //Used for when we have a dynamic/moving hitbox
    private int hitboxNumStates = 1;
    private int hitboxIndex = 0;


    //Local position/size of the hitbox, defined relative to the player
    private Vector3 hitboxPosition = Vector3.zero;
    private Vector3 hitboxSize = Vector3.zero;
    private Quaternion hitboxRotation = Quaternion.identity;

    //Hitbox State
    private ColliderState _state;

    //For health counting
    public int attackDamage = 1;
    PlayerHealth playerHealth;

    // Activate the hitbox with the given parameters
    public void startHitbox(List<Vector3> posOffsetIn, List<Quaternion> rotationIn, List<Vector3> sizeIn, List<float> timersIn, int numStatesIn)
    {
        // If a hitbox is already active, return
        if (_state == ColliderState.Open || _state == ColliderState.Colliding) return;

        //Reset the index 
        hitboxIndex = 0;
        hitboxNumStates = numStatesIn;

        //Set hitbox shape and relative position
        hitboxSizeList  = sizeIn;
        hitboxRotOffset = rotationIn;
        hitboxPosOffset = posOffsetIn;

        //Set hitbox timer
        hitboxTimer = timersIn;

        _state = ColliderState.Open;
    }


    // Called when hitbox deactivated
    void endHitbox() {

        //Reset Hitbox Size
        hitboxSize = new Vector3(0.1f, 0.1f, 0.1f);
        hitboxRotation = transform.rotation;
        hitboxPosition = transform.position;

        //Reset dynamic hitbox counter
        hitboxIndex = 0;
        hitboxNumStates = 0;

        //Reset input matrices
        hitboxSizeList = new List<Vector3>() { new Vector3(0.1f, 0.1f, 0.1f) };
        hitboxRotOffset = new List<Quaternion>() { Quaternion.identity};
        hitboxPosOffset = new List<Vector3>() { Vector3.zero};
        hitboxTimer = new List<float>() { 0f };

    _state = ColliderState.Closed;
    }


    private void Update()
    {
        if (_state == ColliderState.Closed)
        {
            endHitbox();
            return;
        }

        //Update Hitbox location and orientation to follow player
        hitboxPosition = transform.position + hitboxPosOffset[hitboxIndex];
        hitboxRotation = hitboxRotOffset[hitboxIndex];
        hitboxSize     = hitboxSizeList[hitboxIndex];  

        //Decrement move up-time timer
        hitboxTimer[hitboxIndex] = hitboxTimer[hitboxIndex] - Time.deltaTime;

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
            //c.transform.root.GetComponent<Rigidbody>().velocity = new Vector3(10, 10, 0);
            c.transform.root.GetComponent<Rigidbody>().AddForce(new Vector3(30, 30, 0));

            //Health update
            playerHealth = c.transform.root.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(attackDamage);
        }


        //Check if move is finished
        if (hitboxTimer[hitboxIndex] <= 0f)
        {
            if (hitboxIndex >= hitboxNumStates - 1 ) endHitbox();
            else hitboxIndex++;
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

        Gizmos.matrix = Matrix4x4.TRS(hitboxPosition, hitboxRotation, transform.localScale);
        Gizmos.DrawCube(Vector3.zero, new Vector3(hitboxSize.x * 2 / transform.localScale.x, hitboxSize.y * 2 / transform.localScale.y, hitboxSize.z * 2));
    }
}
