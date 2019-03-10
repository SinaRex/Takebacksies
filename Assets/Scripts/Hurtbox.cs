using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour, IAttackResponder
{
    Transform AttatchedCharacter;
    moveData parryData = new moveData(new Vector3(0f, 200f, 0), Vector3.zero, 3.0f, 1.5f, "default");

    void start() {
        AttatchedCharacter = transform.root;
    }

    public void getHitBy(moveData move, Transform attacker) {

        AttatchedCharacter = transform.root;

        //Cannot be hit while time travelling
        if (AttatchedCharacter.GetComponent<PlayerManager>().GetState() == PlayerState.TimeTravelling) return;

        //If hit during parry, reflect attack
        if (AttatchedCharacter.GetComponent<PlayerManager>().GetState() == PlayerState.Parrying) {
            attacker.root.GetComponent<Hurtbox>().getHitBy(parryData, transform);
            GetComponent<Animator>().SetTrigger("Forward-Normal");
            return;
        }

        Vector3 totalKnockback = move.baseKnockBack + (move.knockBackGrowth * AttatchedCharacter.GetComponent<PlayerManager>().getPercent());

        AttatchedCharacter.GetComponent<PlayerManager>().setHitStun(move.hitStun);
        AttatchedCharacter.GetComponent<PlayerManager>().addDamage(move.damage);
        AttatchedCharacter.GetComponent<Rigidbody>().velocity = Vector3.zero;
        AttatchedCharacter.GetComponent<Rigidbody>().AddForce(totalKnockback);

        GetComponent<Animator>().SetTrigger("Flinch");


    }
}