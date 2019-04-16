﻿using System.Collections;
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

        {"Jab",             new moveData(new Vector3(30f, 60f, 0), Vector3.zero           , 3.0f , 0.5f, "default")},
        {"Forward-Normal",  new moveData(new Vector3(400, 100, 0), new Vector3(8, 3, 0)   , 10.0f, 0.9f, "default")},
        {"Down-Normal",     new moveData(new Vector3(0, 800, 0), new Vector3(0, 0, 0)     , 3.0f , 1.4f, "default")},
        {"Up-Normal",       new moveData(new Vector3(30, 450, 0) , new Vector3(0.1f, 8, 0), 4f   , 0.6f, "default")},
        {"Dash-Attack",     new moveData(new Vector3(100, 300, 0), new Vector3(0.5f, 3, 0), 3.0f , 1.0f, "default")},

        {"Neutral-Air",     new moveData(Vector3.zero            , Vector3.zero           , 1.0f , 1.0f, "default")},
        {"Forward-Air",     new moveData(new Vector3(300, 250, 0), new Vector3(4, 2, 0)   , 10.0f, 0.6f, "default")},
        {"Back-Air",        new moveData(new Vector3(-400, 100, 0) , new Vector3(-6, 2, 0), 4f   , 0.75f, "default")},
        {"Up-Air",          new moveData(new Vector3(30, 330, 0) , new Vector3(0.1f, 8f, 0), 4f   , 0.6f, "default")},
        {"Down-Air",        new moveData(new Vector3(100, 300, 0), new Vector3(0.5f, 3, 0), 3.0f , 1.0f, "default")},
										
        {"Neutral-Special", new moveData(new Vector3(200, 300, 0), new Vector3(4, 3, 0), 6.5f, 0.5f, "default")},
        {"Forward-Special", new moveData(new Vector3(300, 100, 0), new Vector3(6, 3, 0), 6.5f, 0.5f, "default")},
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


    //----------------- Grounded Attacks --------------------//


    public void jab() {
       
        GetComponent<Hitbox>().setResponder(this);
        GetComponent<PlayerManager>().StartAttacking(0.15f); // How long your character is frozen in place for FIXME
        moveSuccessful = GetComponent<Hitbox>().startHitbox(
                    new List<Vector3>() {Vector3.zero, new Vector3(0.25f, 0, 0), new Vector3(0.5f, 0, 0), new Vector3(0.25f, 0, 0), Vector3.zero }, 
                    new List<Quaternion>() {transform.rotation, transform.rotation, transform.rotation, transform.rotation, transform.rotation },
                    new List<Vector3>() { Vector3.zero, new Vector3(0.5f, 0.1f, 0.1f), new Vector3(1f, 0.1f, 0.1f), new Vector3(0.5f, 0.1f, 0.1f), Vector3.zero }, 
                    new List<float>() { 0.1f, 0.05f, 0.05f, 0.05f, 0.3f }, 5, "Jab"); // How long the move lasts/ how long it is between consecutive moves
        if (moveSuccessful)
        {
            playerAnimator.SetTrigger("Jab"); 
            GetComponent<AudioManager>().PlayJabSound();
        }

    }


    public void Forward_Normal()
    {
        GetComponent<Hitbox>().setResponder(this);
        GetComponent<PlayerManager>().StartAttacking(0.6f);
        moveSuccessful = GetComponent<Hitbox>().startHitbox(
                    new List<Vector3>() {Vector3.zero,  new Vector3(-0.6f, 0.7f, 0), new Vector3(0, 0.9f, 0), new Vector3(0.7f, 0.7f, 0), new Vector3(0.9f, 0f, 0), Vector3.zero },
                    new List<Quaternion>() {transform.rotation, Quaternion.Euler(0, 0, 45), Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, -45), Quaternion.Euler(0, 0, -90), transform.rotation },
                    new List<Vector3>() { Vector3.zero, new Vector3(0.1f, 0.8f, 0.1f), new Vector3(0.2f, 0.8f, 0.1f), new Vector3(0.2f, 0.8f, 0.1f), new Vector3(0.2f, 0.8f, 0.1f), Vector3.zero },
                    new List<float>() {(2f/2.8f)*0.26f, (2f / 2.8f) * 0.1f, (2f / 2.8f) * 0.06f, (2f / 2.8f) * 0.06f, (2f / 2.8f) * 0.06f, (2f / 2.8f) * 0.2f }, 6, "Forward-Normal");

        if (moveSuccessful)
        {
            playerAnimator.SetTrigger("Forward-Normal");
            GetComponent<AudioManager>().PlayForwardNormalSound();
        }

    }


    public void Up_Normal()
    {
        GetComponent<Hitbox>().setResponder(this);
        GetComponent<PlayerManager>().StartAttacking(0.46f);
        moveSuccessful = GetComponent<Hitbox>().startHitbox(
                    new List<Vector3>() { Vector3.zero, new Vector3(0, 0.4f, 0), new Vector3(0, 0.5f, 0), new Vector3(0, 0.8f, 0), Vector3.zero },
                    new List<Quaternion>() { transform.rotation, transform.rotation, transform.rotation, transform.rotation, transform.rotation},
                    new List<Vector3>() { Vector3.zero, new Vector3(1f, 0.65f, 0.1f), new Vector3(0.5f, 0.65f, 0.1f), new Vector3(0.9f, 0.1f, 0.1f), Vector3.zero },
                    new List<float>() { 0.1f, 0.05f, 0.05f, 0.05f, 0.05f, 0.2f }, 5, "Up-Normal");

        if (moveSuccessful)
        {
            playerAnimator.SetTrigger("Up-Normal");
            GetComponent<AudioManager>().PlayUpNormal();
        }

    }


    public void Down_Normal()
    {
        GetComponent<Hitbox>().setResponder(this);
        GetComponent<PlayerManager>().StartAttacking(0.56f);
        moveSuccessful = GetComponent<Hitbox>().startHitbox(
                    new List<Vector3>() {new Vector3(0f, 0f, 0f), new Vector3(0, -0.4f, 0), new Vector3(0, -0.4f, 0), new Vector3(0, -0.4f, 0), new Vector3(0, 0, 0) },
                    new List<Quaternion>() { transform.rotation, transform.rotation, transform.rotation, transform.rotation, transform.rotation},
                    new List<Vector3>() { Vector3.zero, new Vector3(0.8f, 0.3f, 0.1f), new Vector3(1.6f, 0.3f, 0.1f), new Vector3(0.5f, 0.3f, 0.1f), Vector3.zero},
                    new List<float>() { 0.4f, 0.06f, 0.06f, 0.06f, 0.2f }, 5, "Down-Normal");
        if (moveSuccessful)
        {
            playerAnimator.SetTrigger("Down-Normal");
            GetComponent<AudioManager>().PlayDownNormal();
            // Also play the particle effect which is index 3 in prefab
            transform.GetChild(3).GetComponent<ParticleSystem>().Play();
        }

    }


    //----------------- Aerial Attacks --------------------//

    public void Neutral_Air()
    {
        GetComponent<Hitbox>().setResponder(this);
        GetComponent<PlayerManager>().StartAttacking(0.48f);
        moveSuccessful = GetComponent<Hitbox>().startHitbox(
                   new List<Vector3>() { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero },
                   new List<Quaternion>() { Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, -45), Quaternion.Euler(0, 0, -90), Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, -45), Quaternion.Euler(0, 0, -90) },
                   new List<Vector3>() { new Vector3(0.2f, 1.5f, 0.1f), new Vector3(0.2f, 1.5f, 0.1f), new Vector3(0.2f, 1.5f, 0.1f), new Vector3(0.2f, 1.5f, 0.1f), new Vector3(0.2f, 1.5f, 0.1f), new Vector3(0.2f, 1.5f, 0.1f) },
                   new List<float>() { 0.08f, 0.08f, 0.08f, 0.08f, 0.08f, 0.08f }, 6, "Neutral-Special");
        if (moveSuccessful)
        {
            playerAnimator.SetTrigger("Special");
            GetComponent<AudioManager>().PlayBackAir();
        }

    }

    public void Back_Air()
    {
        GetComponent<Hitbox>().setResponder(this);
        GetComponent<PlayerManager>().StartAttacking(0.15f); 
        moveSuccessful = GetComponent<Hitbox>().startHitbox(
                    new List<Vector3>() { Vector3.zero, new Vector3(-0.25f, 0, 0), new Vector3(-0.5f, 0, 0), new Vector3(-0.25f, 0, 0), Vector3.zero },
                    new List<Quaternion>() { transform.rotation, transform.rotation, transform.rotation, transform.rotation, transform.rotation },
                    new List<Vector3>() { new Vector3(0f, 0f, 0f), new Vector3(0.5f, 0.65f, 0.1f), new Vector3(1f, 0.65f, 0.1f), new Vector3(0.5f, 0.65f, 0.1f), new Vector3(0f, 0f, 0f) },
                    new List<float>() { 0.1f, 0.05f, 0.05f, 0.05f, 0.3f }, 5, "Back-Air");
        if (moveSuccessful)
        {
            playerAnimator.SetTrigger("Back-Air");
            GetComponent<AudioManager>().PlayBackAir();
        }

    }


    public void Forward_Air()
    {
        GetComponent<Hitbox>().setResponder(this);
        GetComponent<PlayerManager>().StartAttacking(0.6f);
        moveSuccessful = GetComponent<Hitbox>().startHitbox(
                    new List<Vector3>() { Vector3.zero, new Vector3(0, 0.9f, 0), new Vector3(0.7f, 0.7f, 0), new Vector3(0.9f, 0f, 0), new Vector3(0.7f, -0.7f, 0), Vector3.zero },
                    new List<Quaternion>() { transform.rotation, Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, -45), Quaternion.Euler(0, 0, -90), Quaternion.Euler(0, 0, -135), transform.rotation },
                    new List<Vector3>() { Vector3.zero, new Vector3(0.2f, 0.8f, 0.1f), new Vector3(0.2f, 0.8f, 0.1f), new Vector3(0.2f, 0.8f, 0.1f), new Vector3(0.2f, 0.8f, 0.1f), Vector3.zero },
                    new List<float>() { 0.0f, 0.1f, 0.06f, 0.06f, 0.06f, 0.2f }, 6, "Forward-Air");
        if (moveSuccessful)
        {
            playerAnimator.SetTrigger("Forward-Air");
            GetComponent<AudioManager>().PlayForwardAir();
        }


    }


    public void Up_Air()
    {
        GetComponent<Hitbox>().setResponder(this);
        GetComponent<PlayerManager>().StartAttacking(0.46f);
        moveSuccessful = GetComponent<Hitbox>().startHitbox(
            new List<Vector3>() { Vector3.zero, new Vector3(-0.6f, 0.7f, 0), new Vector3(0, 0.9f, 0), new Vector3(0.7f, 0.7f, 0), new Vector3(0, 0.9f, 0), new Vector3(-0.6f, 0.7f, 0), new Vector3(0, 0.9f, 0), Vector3.zero },
            new List<Quaternion>() { transform.rotation, transform.rotation, transform.rotation, transform.rotation, transform.rotation, transform.rotation, transform.rotation, transform.rotation },
            new List<Vector3>() { Vector3.zero, new Vector3(1.0f, 0.2f, 0.1f), new Vector3(1.0f, 0.2f, 0.1f), new Vector3(1.0f, 0.2f, 0.1f), new Vector3(1.0f, 0.2f, 0.1f), new Vector3(1.0f, 0.2f, 0.1f), new Vector3(1.0f, 0.2f, 0.1f), Vector3.zero },
            new List<float>() { 0.08f, 0.08f, 0.08f, 0.08f, 0.08f, 0.08f, 0.08f, 0.15f }, 8, "Up-Air");

        if (moveSuccessful)
        {
            playerAnimator.SetTrigger("Up-Air");
            GetComponent<AudioManager>().PlayUpAir();
        }


    }


    public void Down_Air()
    {
        GetComponent<Hitbox>().setResponder(this);
        GetComponent<PlayerManager>().StartAttacking(0.46f);
        moveSuccessful = GetComponent<Hitbox>().startHitbox(
                    new List<Vector3>() { new Vector3(0f, 0f, 0f), new Vector3(0, -0.2f, 0), new Vector3(0.3f, -0.3f, 0), new Vector3(0.65f, -0.4f, 0), new Vector3(0, 0, 0) },
                    new List<Quaternion>() { transform.rotation, transform.rotation, transform.rotation, transform.rotation, transform.rotation },
                    new List<Vector3>() { Vector3.zero, new Vector3(0.5f, 0.7f, 0.1f), new Vector3(0.5f, 0.5f, 0.1f), new Vector3(1.0f, 1.0f, 0.1f), Vector3.zero },
                    new List<float>() { 0.1f, 0.06f, 0.06f, 0.06f, 0.2f }, 5, "Down-Air");
        if (moveSuccessful)
        {
            playerAnimator.SetTrigger("Down-Air");
            GetComponent<AudioManager>().PlayDownAirSound();
        }


    }


    //----------------- Unique Attacks --------------------//

    public void Dash_Attack()
    {
        GetComponent<Hitbox>().setResponder(this);
        GetComponent<PlayerManager>().StartAttacking(0.66f);
        moveSuccessful = GetComponent<Hitbox>().startHitbox(
                    new List<Vector3>() { new Vector3(0f, 0f, 0f), new Vector3(0, -0.2f, 0), new Vector3(0.3f, -0.3f, 0), new Vector3(0.65f, -0.4f, 0), new Vector3(0, 0, 0) },
                    new List<Quaternion>() { transform.rotation, transform.rotation, transform.rotation, transform.rotation, transform.rotation },
                    new List<Vector3>() { Vector3.zero, new Vector3(0.5f, 0.7f, 0.1f), new Vector3(0.5f, 0.5f, 0.1f), new Vector3(1.0f, 1.0f, 0.1f), Vector3.zero },
                    new List<float>() { 0.1f, 0.06f, 0.06f, 0.06f, 0.2f }, 5, "Down-Air");
        if (moveSuccessful)
        {
            playerAnimator.SetTrigger("Down-Air");
            StartCoroutine(SlideForward(0.66f));
            GetComponent<AudioManager>().PlayDashAttackSound();
        }

    }

    public void Neutral_Special()
    {
        /*
        GetComponent<Hitbox>().setResponder(this);
        GetComponent<PlayerManager>().StartAttacking(0.48f);
        moveSuccessful = GetComponent<Hitbox>().startHitbox(
                   new List<Vector3>() { Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero },
                   new List<Quaternion>() { Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, -45), Quaternion.Euler(0, 0, -90), Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, -45), Quaternion.Euler(0, 0, -90) },
                   new List<Vector3>() { new Vector3(0.2f, 1.5f, 0.1f), new Vector3(0.2f, 1.5f, 0.1f), new Vector3(0.2f, 1.5f, 0.1f), new Vector3(0.2f, 1.5f, 0.1f), new Vector3(0.2f, 1.5f, 0.1f), new Vector3(0.2f, 1.5f, 0.1f) },
                   new List<float>() { 0.08f, 0.08f, 0.08f, 0.08f, 0.08f, 0.08f }, 6, "Neutral-Special");*/
        GetComponent<Hitbox>().setResponder(this);
        GetComponent<PlayerManager>().StartAttacking(0.6f);
        moveSuccessful = GetComponent<Hitbox>().startHitbox(
                    new List<Vector3>() { Vector3.zero, new Vector3(-0.6f, 0.7f, 0), new Vector3(0, 0.9f, 0), new Vector3(0.7f, 0.7f, 0), new Vector3(0.9f, 0f, 0), Vector3.zero },
                    new List<Quaternion>() { transform.rotation, Quaternion.Euler(0, 0, 45), Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, -45), Quaternion.Euler(0, 0, -90), transform.rotation },
                    new List<Vector3>() { Vector3.zero, new Vector3(0.2f, 0.7f, 0.1f), new Vector3(0.2f, 0.7f, 0.1f), new Vector3(0.2f, 0.7f, 0.1f), new Vector3(0.2f, 0.7f, 0.1f), Vector3.zero },
                    new List<float>() { 0.16f, 0.1f, 0.06f, 0.06f, 0.06f, 0.2f }, 6, "Forward-Special");
        if (moveSuccessful) playerAnimator.SetTrigger("Special");


    }



    //Used to apply the effects of an attack that connected with the opponent
    //Currently applies same effects for each move, but if we add some weirder moves this could be useful
    public void collisionedWith(Collider collider, string move) {

        //FIXME: Lazy Way of making knockback apply in the correct direction
        moveData inputData = moveDictionary[move];
        inputData.baseKnockBack = Quaternion.Euler(0, (float)transform.GetComponent<PlayerManager>().getPlayerOrientation(), 0) * inputData.baseKnockBack;
        inputData.knockBackGrowth = Quaternion.Euler(0, (float)transform.GetComponent<PlayerManager>().getPlayerOrientation(), 0) * inputData.knockBackGrowth;

        //Cannot hit your own parent
        if (collider.transform.root == GetComponent<PlayerManager>().getEchoParent()) return;
        else collider.transform.root.GetComponent<Hurtbox>().getHitBy(inputData, transform);
    }


    //--------------------Helper Functions-----------------//

    private IEnumerator SlideForward(float slideDuration)
    {
        float increment = 4 / (slideDuration * 50);
        increment = (transform.GetComponent<PlayerManager>().getPlayerOrientation() == Orientation.Right) ? increment : -increment;

        for (int i = 0; i < slideDuration*50; i++) {

            transform.position = new Vector3(transform.position.x + increment, transform.position.y, transform.position.z);
            yield return new WaitForFixedUpdate();
        }

    }

}