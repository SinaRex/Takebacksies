using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerState {
    Alive, Dead, Respawning, Ghost,
    HitStunt, HitLag, LandingLag, Dashing,
    Crouching, Jumping
}


public class PlayerManager : MonoBehaviour
{ 
    public PlayerState state;
    public ParticleSystem deathParticle;
    public GameObject respawnPlatform;
    public Material material;
    private int blinkingDelay;

    // Start is called before the first frame update
    void Start()
    {
        blinkingDelay = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (state == PlayerState.Dead)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            GetComponentInChildren<ParticleSystem>().Play();
            StartCoroutine(Respawn());
        }

    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("PlayZone") && state != PlayerState.Respawning)
        {
            state = PlayerState.Dead;
        }
    }

    private IEnumerator Respawn()
    {
        //Make it flashing
        InvokeRepeating("blink", 0, 0.16f);

        state = PlayerState.Respawning;
        yield return new WaitForSeconds(2);
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None |
            RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX;
        GameObject platform = Instantiate(respawnPlatform, new Vector3(0, 6, -5.45f), Quaternion.identity);
        transform.position = platform.transform.position + new Vector3(0, 1, 0);
        platform.GetComponent<Rigidbody>().isKinematic = false;

        for (int i = 0; i < 30; i++)
        {
            yield return new WaitForFixedUpdate();
            platform.GetComponent<Rigidbody>().AddForce(new Vector3(0, -4.8f, 0));
            GetComponent<Rigidbody>().AddForce(new Vector3(0, -4.8f, 0));
        }
        platform.GetComponent<Rigidbody>().isKinematic = true;

        yield return new WaitForSeconds(0.5f);
        platform.GetComponent<BoxCollider>().enabled = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None |
            RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
        Destroy(platform);

        state = PlayerState.Alive;
        CancelInvoke("blink");
        //Make them not colide



    }

    void blink()
    {
        Invoke("goTransparent", 0f);
        Invoke("goUntransparent", 0.08f);
    }

    void goTransparent()
    {
        material.color = new Color(material.color.r, material.color.g, material.color.b, 1f);
    }

    void goUntransparent()
    {
        material.color = new Color(material.color.r, material.color.g, material.color.b, 0f);
    }
}
