using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flickerFire : MonoBehaviour
{
    private float maxFlickerTimer;
    private float flickerCounter;
    // Start is called before the first frame update
    void Start()
    {
        flickerCounter = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {   
        if (flickerCounter >= maxFlickerTimer)
        {
            gameObject.GetComponent<Light>().intensity = Random.Range(0.5f,1.2f);
            maxFlickerTimer = Random.Range(1.5f, 3);
            flickerCounter = 0;
        }
        flickerCounter += 1;

    }
}
