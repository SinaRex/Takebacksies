using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ledgeController : MonoBehaviour
{


    public Orientation ledgeSide = Orientation.Left;
    private Orientation otherSide = Orientation.Right;
    private Collider currentMovee = null;
    private Transform player;
    private int moveCount = 0;

    private void Start()
    {
        if (ledgeSide == Orientation.Left) otherSide = Orientation.Right;
        else otherSide = Orientation.Left;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currentMovee == other) return;
        if (currentMovee != null) player.GetComponent<Rigidbody>().isKinematic = false;

        currentMovee = other;
        player = currentMovee.transform.root;

        if (player.GetComponent<PlayerManager>().getPlayerOrientation() == ledgeSide && false) //FIXME
        {
            currentMovee = null;
            return;
        }

        moveCount = 0;
        player.GetComponent<PlayerManager>().setHitStun(0.2f);
        player.position = transform.position + new Vector3(0, 0.2f, 0)  + Quaternion.Euler(0, (float)ledgeSide, 0) * new Vector3(0.2f, 0f, 0f);
        player.GetComponent<Rigidbody>().isKinematic = true;

    }

    private void Update()
    {
        if (currentMovee == null) return;
        moveCount++;

        player.position = player.position + new Vector3(0f, 0.1f, 0f);
        if (moveCount >= 5) player.position = player.position + Quaternion.Euler(0, (float)otherSide, 0) * new Vector3(0.2f, 0f, 0f);


        if (moveCount >= 10) {
            player.GetComponent<Rigidbody>().isKinematic = false;
            currentMovee = null;
        }
    }
}
