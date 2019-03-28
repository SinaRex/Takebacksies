using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrollingObjects : MonoBehaviour
{

    public float scrollSpeed = -1.5f;
    public Vector3 resetPosition;
    //public Vector3 startPos;

    private void Awake()
    {
        resetPosition = new Vector3(30, transform.position.y, transform.position.z);

        transform.position = resetPosition;
        //Vector3 startPos = new Vector3(Random.Range(1, 10), -60, 10);
    }
 
    // Update is called once per frame
    void Update()
    {

        if (transform.position.x <= -10)
        { 

        transform.position = resetPosition;
        print(transform.position.x);

        }
       
        transform.Translate(Time.deltaTime * scrollSpeed, 0, 0);    
    }

}

