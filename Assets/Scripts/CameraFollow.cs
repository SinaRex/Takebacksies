using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CameraFollow : MonoBehaviour
{

    public Transform p1;
    public Transform p2;

    public Transform follow;
    public Transform cam;

    public float XBound;
    public float YBoundMax;
    public float YBoundMin;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = (p1.position + p2.position) / 2;

        follow.position = target;

        if (follow.position.x > XBound)
        {
            follow.position = new Vector3(XBound, follow.position.y, follow.position.z);
        }
        else if (follow.position.x < -XBound)
        {
            follow.position = new Vector3(-XBound, follow.position.y, follow.position.z);
        }

        if (follow.position.y > YBoundMax)
        {
            follow.position = new Vector3(follow.position.x, YBoundMax, follow.position.z);
        }
        else if (follow.position.y < YBoundMin)
        {
            follow.position = new Vector3(follow.position.x, YBoundMin, follow.position.z);
        }

        cam.position = Vector3.Lerp(cam.position, follow.position, 0.1f);
    }
}
