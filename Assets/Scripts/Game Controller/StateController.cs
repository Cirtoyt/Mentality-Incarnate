using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Scenes
{
    game,
    mainMenu
}

public enum InputModes
{
    keyboardAndMouse,
    controller
}

public class StateController : MonoBehaviour
{
    public InputModes inputMode;

    private void Awake()
    {
        inputMode = InputModes.keyboardAndMouse;
    }
    
    void Update()
    {
        // Test buttons
        if (Input.GetKeyDown("j"))
        {
            if (inputMode == InputModes.keyboardAndMouse)
            {
                inputMode = InputModes.controller;
            }
            else
            {
                inputMode = (int)InputModes.keyboardAndMouse;
            }
        }
    }
}
