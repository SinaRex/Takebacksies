using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour, IAttackResponder
{
    Transform AttatchedCharacter; 

    void start() {
        AttatchedCharacter = transform.root;
    }

    public void getHitBy(moveData move) {

        AttatchedCharacter = transform.root;

        AttatchedCharacter.GetComponent<PlayerManager>().setHitStun(move.hitStun);
        AttatchedCharacter.GetComponent<PlayerManager>().addDamage(move.damage);
        AttatchedCharacter.GetComponent<Rigidbody>().velocity = Vector3.zero;
        AttatchedCharacter.GetComponent<Rigidbody>().AddForce(move.knockBack);

    }
}
