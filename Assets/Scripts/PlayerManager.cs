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

    // Start is called before the first frame update
    void Start()
    {
        GoUntransparent();
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
    private void OnTriggerEnter(Collider other)
    {
        string BlastSelect;
        if (GetComponent<Player>().isPlayer1)
        {
            BlastSelect = "blast1";
        }
        else
        {
            BlastSelect = "blast2";
        }
        GameObject.FindWithTag(BlastSelect).transform.rotation = Quaternion.identity;
        if (other.gameObject.CompareTag("LeftWall")) {
            GameObject.FindWithTag(BlastSelect).transform.Rotate(new Vector3(-45, 90, 0));
        }
        else if (other.gameObject.CompareTag("RightWall"))
        {
            GameObject.FindWithTag(BlastSelect).transform.Rotate(new Vector3(-45, -90, 0));
        }
        else if (other.gameObject.CompareTag("TopWall"))
        {
            GameObject.FindWithTag(BlastSelect).transform.Rotate(new Vector3(90, 0, 0));
        }
        else if (other.gameObject.CompareTag("ButtomWall"))
        {
            GameObject.FindWithTag(BlastSelect).transform.Rotate(new Vector3(-90, 0, 0));
        }
    }
    private IEnumerator Respawn()
    {
        //Make it flashing
        //InvokeRepeating("blink", 0, 0.16f);
        StartCoroutine(Testing());

        state = PlayerState.Respawning;
        yield return new WaitForSeconds(2);
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None |
            RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX;
        GameObject platform;

       if (GameObject.Find("Player2").GetComponent<PlayerManager>().state == PlayerState.Respawning && GetComponent<Player>().isPlayer1)
        {
            platform = Instantiate(respawnPlatform, new Vector3(-2, 6, -5.45f), Quaternion.identity);
            transform.position = platform.transform.position + new Vector3(0, 1, 0);
        }
        else
        {
            platform = Instantiate(respawnPlatform, new Vector3(0, 6, -5.45f), Quaternion.identity);
            transform.position = platform.transform.position + new Vector3(0, 1, 0);
        }
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
        //CancelInvoke("blink");

        //Make them not colide



    }
    private IEnumerator Testing()
    {
        Debug.Log("Start");
        for (int i = 0; i < 16; i++)
        {
            yield return new WaitForSeconds(0.125f);
            GoTransparent();
            yield return new WaitForSeconds(0.125f);
            GoUntransparent();

        }
        Debug.Log("Done");
    }


    //private IEnumerator Flash()
    //{
    //    material.color = new Color(material.color.r, material.color.g, material.color.b, 0f);
    //}

    void GoUntransparent()
    {
        material.color = new Color(material.color.r, material.color.g, material.color.b, 1f);
    }
    void GoTransparent()
    {
        material.color = new Color(material.color.r, material.color.g, material.color.b, 0f);
    }

}
