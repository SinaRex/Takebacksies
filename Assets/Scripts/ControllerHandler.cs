using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ControllerHandler : MonoBehaviour
{
    public Queue<float> actions1 = new Queue<float>();
    public Queue<float> actions2 = new Queue<float>();

    private float InputPlayer1;
    private float InputPlayer2;
    //private float MoveAxisX1 = Input.GetAxis("MoveAxisX1");
    //private float FightAxisX1 = Input.GetAxis("FightAxisX1");
    //private float FightAxisY1 = Input.GetAxis("FightAxisY1");
    //private float NormalButton1 = Input.GetAxis("NormalButton1");
    //private float SpecialButton1 = Input.GetAxis("SpecialButton1");
    //private float RewindButton1 = Input.GetAxis("RewindButton1");
    //private float ParryButton1 = Input.GetAxis("ParryButton1");

    //Size of the queue
    public int limit = 75;
    public int count1 = 0;
    public int count2 = 0;
    // Update is called once per frame
    void Update()
    {
        if (count1 < limit)
        {
            actions1.Enqueue(InputPlayer1);
            count1 += 1;
        }
        else
        {
            actions1.Dequeue();
            actions1.Enqueue(InputPlayer1);

        }
        if (count2 < limit)
        {
            actions2.Enqueue(InputPlayer2);
            count2 += 1;
        }
        else
        {
            actions2.Dequeue();
            actions2.Enqueue(InputPlayer1);

        }

    }
}
