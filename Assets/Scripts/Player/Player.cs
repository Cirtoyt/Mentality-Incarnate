using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Statics")]
    [SerializeField] private StateController stateCtlr;
    [Header("Options")]
    [SerializeField] private float movementSpeed;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (stateCtlr.playerState == PlayerStates.active)
        {
            Move();
        }
    }

    private void Move()
    {
        Vector2 moveDirection = Vector2.zero;

        if (stateCtlr.inputMode == InputModes.keyboardAndMouse)
        {
            moveDirection = new Vector2(Input.GetAxisRaw("Horizontal Keyboard"),
                                        Input.GetAxisRaw("Vertical Keyboard"));
        }

        if (stateCtlr.inputMode == InputModes.controller)
        {
            moveDirection = new Vector2(Input.GetAxisRaw("Horizontal Controller"),
                                        Input.GetAxisRaw("Vertical Controller"));
        }

        rb.MovePosition((Vector2)transform.position + moveDirection.normalized * movementSpeed * Time.fixedDeltaTime);
    }
}
