using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerState {
    Alive, Dead, Respawning, 
    HitStunt, HitLag, LandingLag
}


public class PlayerManager : MonoBehaviour
{ 
    public PlayerState state;
    public ParticleSystem deathParticle;
    public GameObject respawnPlatform;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (state == PlayerState.Dead)
        {
            //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponentInChildren<ParticleSystem>().Play();
            StartCoroutine(Respawn());
        }

    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("PlayZone") && state != PlayerState.Respawning)
        {
            Debug.Log("Yo");
            state = PlayerState.Dead;
        }
    }

    private IEnumerator Respawn()
    {
        state = PlayerState.Respawning;
        yield return new WaitForSeconds(2);
        //GetComponent<Rigidbody>().constraints = RigidbodyConstraints.;
        GetComponent<Rigidbody>().useGravity = true;
        Instantiate(respawnPlatform, new Vector3(0, 6, -5.45f), Quaternion.identity);


        //Make it flashing

        //Make them not colide


    }

}
