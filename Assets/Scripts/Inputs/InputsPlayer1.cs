using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputsPlayer1 : MonoBehaviour
{

    public float MoveAxisX1;
    public float MoveAxisY1;

    public float FightAxisX1;
    public float FightAxisY1;
    public float NormalButton1;
    public float SpecialButton1;
    public float RewindButton1;
    public float ParryButton1;
    public float jumpButton1;



    // Update is called once per frame
    void FixedUpdate()
    {
        MoveAxisX1 = Input.GetAxis("MoveAxisX1");
        MoveAxisY1 = Input.GetAxis("MoveAxisY1");
        FightAxisX1 = Input.GetAxis("FightAxisX1");
        FightAxisY1 = Input.GetAxis("FightAxisY1");
        NormalButton1 = Input.GetAxis("NormalButton1");
        SpecialButton1 = Input.GetAxis("SpecialButton1");
        RewindButton1 = Input.GetAxis("RewindButton1");
        ParryButton1 = Input.GetAxis("ParryButton1");
        jumpButton1 = Input.GetAxis("Jump1");


        //Debug.Log(MoveAxisX1);
        //Debug.Log(MoveAxisY1);
        //Debug.Log(FightAxisX1);
        //Debug.Log(FightAxisY1);
        Debug.Log("A " + jumpButton1);
        Debug.Log("X " + NormalButton1);
        Debug.Log("B " + SpecialButton1);
        Debug.Log("Y " + RewindButton1);
        Debug.Log("RB " + ParryButton1);


    }
}
