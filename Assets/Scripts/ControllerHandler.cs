using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TBInput
{

    public float MoveAxisX;
    public float MoveAxisY;
    public float FightAxisX;
    public float FightAxisY;
    public float NormalButton;
    public float SpecialButton;
    public float RewindButton;
    public float ParryButton;
    public float jumpButton;
}
public Queue<float> actions1 = new Queue<float>();
public Queue<float> actions2 = new Queue<float>();

//Size of the queue
private int limit = 75;
private int count1 = 0;
private int count2 = 0;


public class ControllerHandler : MonoBehaviour
{
    public TBInput input1;
    public TBInput input2;

    void FixedUpdate()
    {
        input1.MoveAxisX = Input.GetAxis("MoveAxisX1");
        input1.MoveAxisY = Input.GetAxis("MoveAxisY1");
        input1.FightAxisX = Input.GetAxis("FightAxisX1");
        input1.FightAxisY = Input.GetAxis("FightAxisY1");
        input1.NormalButton = Input.GetAxis("NormalButton1");
        input1.SpecialButton = Input.GetAxis("SpecialButton1");
        input1.RewindButton = Input.GetAxis("RewindButton1");
        input1.ParryButton = Input.GetAxis("ParryButton1");
        input1.jumpButton = Input.GetAxis("Jump1");

        input2.MoveAxisX = Input.GetAxis("MoveAxisX2");
        input2.MoveAxisY = Input.GetAxis("MoveAxisY2");
        input2.FightAxisX = Input.GetAxis("FightAxisX2");
        input2.FightAxisY = Input.GetAxis("FightAxisY2");
        input2.NormalButton = Input.GetAxis("NormalButton2");
        input2.SpecialButton = Input.GetAxis("SpecialButton2");
        input2.RewindButton = Input.GetAxis("RewindButton2");
        input2.ParryButton = Input.GetAxis("ParryButton2");
        input2.jumpButton = Input.GetAxis("Jump2");

        if (count1 < limit)
        {
            actions1.Enqueue(Input1);
            count1 += 1;
        }
        else
        {
            actions1.Dequeue();
            actions1.Enqueue(Input1);

        }
        if (count2 < limit)
        {
            actions2.Enqueue(Input2);
            count2 += 1;
        }
        else
        {
            actions2.Dequeue();
            actions2.Enqueue(Input2);

        }

    }
}
