using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct moveData
{
    public  Vector3 knockBack;
    public  float damage;
    public  float hitStun;
    public  string name;

    public moveData(Vector3 knockBackIn, float damageIn, float hitStunIn, string nameIn) {
        knockBack = knockBackIn;
        damage    = damageIn;
        hitStun   = hitStunIn;
        name      = nameIn;
    }   
}

public class MoveList : MonoBehaviour, IHitboxResponder
{

    //Define a default movelist
    IDictionary<string, moveData> moveDictionary = new Dictionary<string, moveData>() {

        {"Jab",             new moveData(new Vector3(10f, 20f, 0), 1.0f, 1.0f, "default")},
        {"Forward-Normal",  new moveData(new Vector3(600, 30, 0), 1.0f, 1.0f, "default")},
        {"Down-Normal",     new moveData(Vector3.zero, 1.0f, 1.0f, "default")},
        {"Up-Normal",       new moveData(new Vector3(30, 30, 0), 1.0f, 1.0f, "default")},

        {"Neutral-Air",     new moveData(Vector3.zero, 1.0f, 1.0f, "default")},
        {"Forward-Air",     new moveData(Vector3.zero, 1.0f, 1.0f, "default")},
        {"Back-Air",        new moveData(Vector3.zero, 1.0f, 1.0f, "default")},
        {"Up-Air",          new moveData(Vector3.zero, 1.0f, 1.0f, "default")},
        {"Down-Air",        new moveData(Vector3.zero, 1.0f, 1.0f, "default")},

        {"Neutral-Special", new moveData(Vector3.zero, 1.0f, 1.0f, "default")},
        {"Forward-Special", new moveData(Vector3.zero, 1.0f, 1.0f, "default")},
        {"Down-Special",    new moveData(Vector3.zero, 1.0f, 1.0f, "default")},
        {"Up-Special",      new moveData(Vector3.zero, 1.0f, 1.0f, "default")},
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
        GetComponent<PlayerManager>().StartAttacking(0.15f);
        GetComponent<Hitbox>().startHitbox(new List<Vector3>() { new Vector3(0.25f, 0, 0), new Vector3(0.5f, 0, 0), new Vector3(0.25f, 0, 0) }, 
                    new List<Quaternion>() { transform.rotation, transform.rotation, transform.rotation },
                    new List<Vector3>() { new Vector3(0.5f, 0.1f, 0.1f), new Vector3(1f, 0.1f, 0.1f), new Vector3(0.5f, 0.1f, 0.1f) }, 
                    new List<float>() { 0.05f, 0.05f, 0.05f }, 3, "Jab");
    }


    public void Up_Normal()
    {
        GetComponent<Hitbox>().setResponder(this);
        GetComponent<PlayerManager>().StartAttacking(0.3f);
        GetComponent<Hitbox>().startHitbox(new List<Vector3>() { Vector3.zero, Vector3.zero, Vector3.zero },
                    new List<Quaternion>() { Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, -45), Quaternion.Euler(0, 0, -90) },
                    new List<Vector3>() { new Vector3(0.2f, 2f, 0.1f), new Vector3(0.2f, 2f, 0.1f), new Vector3(0.2f, 2f, 0.1f) },
                    new List<float>() { 0.1f, 0.1f, 0.1f }, 3, "Up-Normal");

    }


    public void Forward_Normal()
    {
        GetComponent<Hitbox>().setResponder(this);
        GetComponent<PlayerManager>().StartAttacking(0.24f);
        GetComponent<Hitbox>().startHitbox(new List<Vector3>() { new Vector3(-0.6f, 0.7f, 0), new Vector3(0, 0.9f, 0), new Vector3(0.7f, 0.7f, 0), new Vector3(0.9f, 0f, 0) },
                    new List<Quaternion>() { Quaternion.Euler(0, 0, 45), Quaternion.Euler(0, 0, 0), Quaternion.Euler(0, 0, -45), Quaternion.Euler(0, 0, -90) },
                    new List<Vector3>() { new Vector3(0.2f, 0.7f, 0.1f), new Vector3(0.2f, 0.7f, 0.1f), new Vector3(0.2f, 0.7f, 0.1f), new Vector3(0.2f, 0.7f, 0.1f) },
                    new List<float>() {0.06f, 0.06f, 0.06f, 0.06f }, 4, "Forward-Normal");

    }

    //Used to apply the effects of an attack that connected with the opponent
    //Currently applies same effects for each move, but if we add some weirder moves this could be useful
    public void collisionedWith(Collider collider, string move) {
        collider.transform.root.GetComponent<Hurtbox>().getHitBy(moveDictionary[move]);
    }

}
