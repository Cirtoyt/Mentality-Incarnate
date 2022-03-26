using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Scenes
{
    game,
    mainMenu
}

public enum PlayerStates
{
    active,
    frozen,
    aspiring
}

public enum InputModes
{
    keyboardAndMouse,
    controller
}

public class StateController : MonoBehaviour
{
    public PlayerStates playerState;
    public InputModes inputMode;

    private void Awake()
    {
        playerState = PlayerStates.active;
        inputMode = InputModes.keyboardAndMouse;
    }
    
    void Update()
    {
        // Test buttons
        if (Input.GetKeyDown("h"))
        {
            if (playerState == PlayerStates.active)
            {
                playerState = PlayerStates.frozen;
            }
            else
            {
                playerState = PlayerStates.active;
            }
        }
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
