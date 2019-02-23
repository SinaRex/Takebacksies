﻿using System.Collections;
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

    }
}