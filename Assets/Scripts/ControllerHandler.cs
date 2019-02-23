using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TBInput
{

    public float MoveAxisX;
    public float MoveAxisY;
    public float FightAxisX;
    public float FightAxisY;
    public bool NormalButton;  
    public bool SpecialButton;  
    public bool RewindButton;
    public bool ParryButton;
    public bool jumpButton;  
}

public class ControllerHandler : MonoBehaviour
{
    public TBInput input1;
    public TBInput input2;

    //Move recording Queues
    public Queue<TBInput> recording1 = new Queue<TBInput>();
    public Queue<TBInput> recording2 = new Queue<TBInput>();

    //Size of the queue
    private int recordingLimit = 150; // 50 calls/sec * 3 seconds
    private int recordingCount1 = 0;
    private int recordingCount2 = 0;


    void FixedUpdate()
    {
        input1.MoveAxisX = Input.GetAxis("MoveAxisX1");
        input1.MoveAxisY = Input.GetAxis("MoveAxisY1");
        input1.FightAxisX = Input.GetAxis("FightAxisX1");
        input1.FightAxisY = Input.GetAxis("FightAxisY1");
        input1.NormalButton = Input.GetButtonDown("NormalButton1");
        input1.SpecialButton = Input.GetButtonDown("SpecialButton1");
        input1.RewindButton = Input.GetButtonDown("RewindButton1");
        input1.ParryButton = Input.GetButtonDown("ParryButton1");
        input1.jumpButton = Input.GetButtonDown("Jump1");

        input2.MoveAxisX = Input.GetAxis("MoveAxisX2");
        input2.MoveAxisY = Input.GetAxis("MoveAxisY2");
        input2.FightAxisX = Input.GetAxis("FightAxisX2");
        input2.FightAxisY = Input.GetAxis("FightAxisY2");
        input2.NormalButton = Input.GetButtonDown("NormalButton2");
        input2.SpecialButton = Input.GetButtonDown("SpecialButton2");
        input2.RewindButton = Input.GetButtonDown("RewindButton2");
        input2.ParryButton = Input.GetButtonDown("ParryButton2");
        input2.jumpButton = Input.GetButtonDown("Jump2");


        //Record Character Actions
        if (recordingCount1 < recordingLimit)
        {
            recording1.Enqueue(input1);
            recordingCount1 += 1;
        }
        else
        {
            recording1.Dequeue();
            recording1.Enqueue(input1);

        }


        if (recordingCount2 < recordingLimit)
        {
            recording2.Enqueue(input2);
            recordingCount2 += 1;
        }
        else
        {
            recording2.Dequeue();
            recording2.Enqueue(input2);

        }

    }
}
