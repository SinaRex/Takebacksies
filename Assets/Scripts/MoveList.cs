using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct moveData
{
    public  Vector3 baseKnockBack;
    public  Vector3 knockBackGrowth;
    public  float damage;
    public  float hitStun;
    public  string name;

    public moveData(Vector3 baseKnockBackIn, Vector3 knockBackGrowthIn, float damageIn, float hitStunIn, string nameIn) {
        baseKnockBack = baseKnockBackIn;
        knockBackGrowth = knockBackGrowthIn;
        damage    = damageIn;
        hitStun   = hitStunIn;
        name      = nameIn;
    }   
}

public class MoveList : MonoBehaviour, IHitboxResponder
{
    //Variables
    bool moveSuccessful;
    Animator playerAnimator;

    private void Start() {
        playerAnimator = GetComponent<Animator>();

    }

    //Define a default movelist
    IDictionary<string, moveData> moveDictionary = new Dictionary<string, moveData>() {

        {"Jab",             new moveData(new Vector3(30f, 60f, 0), Vector3.zero, 3.0f, 0.5f, "default")},
        {"Forward-Normal",  new moveData(new Vector3(200, 100, 0), new Vector3(4, 2, 0), 10.0f, 1.0f, "default")},
        {"Down-Normal",     new moveData(new Vector3(100, 300, 0), new Vector3(0.5f, 2, 0), 3.0f, 1.0f, "default")},
        {"Up-Normal",       new moveData(new Vector3(30, 100, 0) , new Vector3(0.1f, 6, 0), 4f, 1.0f, "default")},

        {"Neutral-Air",     new moveData(Vector3.zero, Vector3.zero, 1.0f, 1.0f, "default")},
        {"Forward-Air",     new moveData(Vector3.zero, Vector3.zero, 1.0f, 1.0f, "default")},
        {"Back-Air",        new moveData(Vector3.zero, Vector3.zero, 1.0f, 1.0f, "default")},
        {"Up-Air",          new moveData(Vector3.zero, Vector3.zero, 1.0f, 1.0f, "default")},
        {"Down-Air",        new moveData(Vector3.zero, Vector3.zero, 1.0f, 1.0f, "default")},
										
        {"Neutral-Special", new moveData(new Vector3(200, 100, 0), new Vector3(4, 2, 0), 6.5f, 1.0f, "default")},
        {"Forward-Special", new moveData(Vector3.zero, Vector3.zero, 1.0f, 1.0f, "default")},
        {"Down-Special",    new moveData(Vector3.zero, Vector3.zero, 1.0f, 1.0f, "default")},
        {"Up-Special",      new moveData(Vector3.zero, Vector3.zero, 1.0f, 1.0f, "default")},
    };

    //Function used to end moves abrudptly
    public void interruptMove()
    {
        GetComponent<Hitbox>().endHitbox();
    }

    //These functions set themselves as the hitbox's responders, Set's the player managers attack timer to the attack duration
    // and the instatiates the hitbox(es)

    public void jab() {
       
        GetComponent<Hitbox>().setResponder(this);
        GetComponent<PlayerManager>().StartAttacking(0.15f); // How long your character is frozen in place for FIXME
        moveSuccessful = GetComponent<Hitbox>().startHitbox(
                    new List<Vector3>() {Vector3.zero, new Vector3(0.25f, 0, 0), new Vector3(0.5f, 0, 0), new Vector3(0.25f, 0, 0), Vector3.zero }, 
                    new List<Quaternion>() {transform.rotation, transform.rotation, transform.rotation, transform.rotation, transform.rotation },
                    new List<Vector3>() { new Vector3(0f, 0f, 0f), new Vector3(0.5f, 0.1f, 0.1f), new Vector3(1f, 0.1f, 0.1f), new Vector3(0.5f, 0.1f, 0.1f), new Vector3(0f, 0f, 0f) }, 
                    new List<float>() { 0.1f, 0.05f, 0.05f, 0.05f, 0.3f }, 5, "Jab"); // How long the move lasts/ how long it is between consecutive moves
        if(moveSuccessful) playerAnimator.SetTrigger("Jab");

    }


    public void Forward_Normal()
    {
        GetComponent<Hitbox>().setResponder(this);
        GetComponent<PlayerManager>().StartAttacking(0.5f);
        moveSuccessful = GetComponent<Hitbox>().startHitbox(
                    new List<Vector3>() {Vector3.zero,  new Vector3(-0.6f, 0.7f, 0), new Vector3(0, 0.9f, 0), new Vector3(0.7f, 0.7f, 0), new Vector3(0.9f, 0f, 0), Vector3.zero },
                    new List<Quaternion>() {transform.rotation, Quaternion.Euler(0, 0, 45), Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, -45), Quaternion.Euler(0, 0, -90), transform.rotation },
                    new List<Vector3>() { Vector3.zero, new Vector3(0.2f, 0.7f, 0.1f), new Vector3(0.2f, 0.7f, 0.1f), new Vector3(0.2f, 0.7f, 0.1f), new Vector3(0.2f, 0.7f, 0.1f), Vector3.zero },
                    new List<float>() {0.06f, 0.1f, 0.06f, 0.06f, 0.06f, 0.2f }, 6, "Forward-Normal");
        if (moveSuccessful) playerAnimator.SetTrigger("Forward-Normal");

    }


    public void Up_Normal()
    {
        GetComponent<Hitbox>().setResponder(this);
        GetComponent<PlayerManager>().StartAttacking(0.46f);
        moveSuccessful = GetComponent<Hitbox>().startHitbox(
                    new List<Vector3>() { Vector3.zero, new Vector3(-0.6f, 0.7f, 0), new Vector3(0, 0.9f, 0), new Vector3(0.7f, 0.7f, 0), new Vector3(0, 0.9f, 0), new Vector3(-0.6f, 0.7f, 0), new Vector3(0, 0.9f, 0), Vector3.zero },
                    new List<Quaternion>() { transform.rotation, Quaternion.Euler(0, 0, 85), Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, -85), Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, 85), Quaternion.Euler(0, 0, 0), transform.rotation },
                    new List<Vector3>() { Vector3.zero, new Vector3(0.2f, 0.55f, 0.1f), new Vector3(0.3f, 0.3f, 0.1f), new Vector3(0.2f, 0.55f, 0.1f), new Vector3(0.3f, 0.3f, 0.1f), new Vector3(0.2f, 0.55f, 0.1f), new Vector3(0.3f, 0.3f, 0.1f), Vector3.zero },
                    new List<float>() { 0.08f, 0.08f, 0.08f, 0.08f, 0.08f, 0.08f, 0.08f, 0.3f }, 8, "Up-Normal");
        if (moveSuccessful) playerAnimator.SetTrigger("Up-Normal");


    }


    public void Down_Normal()
    {
        GetComponent<Hitbox>().setResponder(this);
        GetComponent<PlayerManager>().StartAttacking(0.46f);
        moveSuccessful = GetComponent<Hitbox>().startHitbox(
                    new List<Vector3>() {new Vector3(0f, 0f, 0f), new Vector3(0, -0.2f, 0), new Vector3(0.3f, -0.3f, 0), new Vector3(0.65f, -0.4f, 0), new Vector3(0, 0, 0) },
                    new List<Quaternion>() { transform.rotation, transform.rotation, transform.rotation, transform.rotation, transform.rotation},
                    new List<Vector3>() { Vector3.zero, new Vector3(0.5f, 0.7f, 0.1f), new Vector3(0.5f, 0.5f, 0.1f), new Vector3(1.0f, 1.0f, 0.1f), Vector3.zero},
                    new List<float>() { 0.1f, 0.06f, 0.06f, 0.06f, 0.2f }, 5, "Down-Normal");
        if (moveSuccessful) playerAnimator.SetTrigger("Down-Normal");


    }


    public void Neutral_Special()
    {
        GetComponent<Hitbox>().setResponder(this);
        GetComponent<PlayerManager>().StartAttacking(0.48f);
        moveSuccessful = GetComponent<Hitbox>().startHitbox(
                   new List<Vector3>() { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero },
                   new List<Quaternion>() { Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, -45), Quaternion.Euler(0, 0, -90), Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, -45), Quaternion.Euler(0, 0, -90) },
                   new List<Vector3>() { new Vector3(0.2f, 1.5f, 0.1f), new Vector3(0.2f, 1.5f, 0.1f), new Vector3(0.2f, 1.5f, 0.1f), new Vector3(0.2f, 1.5f, 0.1f), new Vector3(0.2f, 1.5f, 0.1f), new Vector3(0.2f, 1.5f, 0.1f) },
                   new List<float>() { 0.08f, 0.08f, 0.08f, 0.08f, 0.08f, 0.08f }, 6, "Neutral-Special");
        if (moveSuccessful) playerAnimator.SetTrigger("Special");


    }



    //Used to apply the effects of an attack that connected with the opponent
    //Currently applies same effects for each move, but if we add some weirder moves this could be useful
    public void collisionedWith(Collider collider, string move) {

        //FIXME: Lazy Way of making knockback apply in the correct direction
        moveData inputData = moveDictionary[move];
        inputData.baseKnockBack = Quaternion.Euler(0, (float)transform.GetComponent<PlayerManager>().getPlayerOrientation(), 0) * inputData.baseKnockBack;
        inputData.knockBackGrowth = Quaternion.Euler(0, (float)transform.GetComponent<PlayerManager>().getPlayerOrientation(), 0) * inputData.knockBackGrowth;
        collider.transform.root.GetComponent<Hurtbox>().getHitBy(inputData, transform);
    }

}