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

        //Set current Attacker as the last player that hit you
        attacker.GetComponent<PlayerManager>().AddTimeJuice(0.1f);
        AttatchedCharacter.GetComponent<PlayerManager>().updateLastHitBy(attacker);

        // When you get hurt
        StartCoroutine(MakeHitSparks(move.hitStun, AttatchedCharacter.GetComponent<PlayerManager>().getPercent()));
    }

    //------------------- Helper Functions ----------------------//
    // Hit Spark
    private IEnumerator MakeHitSparks(float hitStun, float percent)
    {
        transform.GetChild(3).GetComponent<ParticleSystem>().Play();
        GameObject playerModel = transform.GetChild(2).gameObject;// get CharacterModel gameobject
        //playerModel.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color = Color.red;
        LerpColorPercent(playerModel.transform.GetChild(1).gameObject.GetComponent<Renderer>().material, percent);


        yield return new WaitForSecondsRealtime(hitStun);

        playerModel.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color = Color.white;
        if (AttatchedCharacter.GetComponent<PlayerManager>().playerIdentity == PlayerIdentity.Echo)
        {
            playerModel.transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color = new Color(1f,1f,1f, 0.5f);
        }


    }

    // Just lerp to a predefined color that also matches the color of percentage
    private void LerpColorPercent(Material playerModelMat, float percent)
    {
        float alpha = AttatchedCharacter.GetComponent<PlayerManager>().playerIdentity == PlayerIdentity.Echo ? 0.5f : 1;
        if (percent < 50)
        {
            playerModelMat.color = Color.Lerp(Color.white, new Color(0.5f, 0f, 0, alpha), percent / 50);
        }
        else if(percent <= 100)
        {
            playerModelMat.color = Color.Lerp(new Color(0.5f, 0f, 0f), new Color(1f, 0f, 0f, alpha), percent / 100);
        }
        else
        {
            playerModelMat.color = Color.Lerp(new Color(1f, 0f, 0f), new Color(0.5f, 0f, 0f, alpha), percent / 200);
        }


    }

}