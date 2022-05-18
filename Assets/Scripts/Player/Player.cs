using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float movementSpeed = 1;
    [SerializeField] private float rotationSpeed = 1;
    [SerializeField] private int maxWillPower = 100;
    [Header("Debug")]
    [SerializeField] private int willPower;

    private Rigidbody2D rb;
    private WeaponManager weaponManager;
    private Vector2 moveDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        weaponManager = GetComponent<WeaponManager>();

        willPower = maxWillPower;
    }

    private void FixedUpdate()
    {
        if (moveDirection != Vector2.zero)
        {
            Move();
            Rotate();
        }
    }

    private void OnMove(InputValue value)
    {
        moveDirection = value.Get<Vector2>();
    }

    private void Move()
    {
        rb.MovePosition((Vector2)transform.position + moveDirection.normalized * movementSpeed * Time.fixedDeltaTime);
    }

    private void Rotate()
    {
        Quaternion newRotation = Quaternion.LookRotation(Vector3.forward, moveDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, rotationSpeed * Time.fixedDeltaTime);
    }

    public void FreezePlayer()
    {
        enabled = false;
        weaponManager.enabled = false;
        weaponManager.currentWeaponController.enabled = false;
    }

    public void UnFreezePlayer()
    {
        enabled = true;
        weaponManager.enabled = true;
        weaponManager.currentWeaponController.enabled = true;
    }

    public void ResetPlayer()
    {
        weaponManager.currentWeaponController.ResetController();
    }

    public int GetWillPower() => willPower;
}
