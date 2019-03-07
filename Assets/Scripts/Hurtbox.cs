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

        if (AttatchedCharacter.GetComponent<PlayerManager>().GetState() == PlayerState.TimeTravelling) return;

        Vector3 totalKnockback = move.baseKnockBack + (move.knockBackGrowth * AttatchedCharacter.GetComponent<PlayerManager>().getPercent());

        AttatchedCharacter.GetComponent<PlayerManager>().setHitStun(move.hitStun);
        AttatchedCharacter.GetComponent<PlayerManager>().addDamage(move.damage);
        AttatchedCharacter.GetComponent<Rigidbody>().velocity = Vector3.zero;
        AttatchedCharacter.GetComponent<Rigidbody>().AddForce(totalKnockback);

        //GetComponent<Animator>().SetTrigger("Flinch");


    }
}