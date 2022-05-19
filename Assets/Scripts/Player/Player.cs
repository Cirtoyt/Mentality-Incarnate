using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Statics")]
    [SerializeField] private Transform weaponTarget;
    [SerializeField] private Animator anim;
    [SerializeField] private Transform bodySprite;
    [Header("Settings")]
    [SerializeField] private float movementSpeed = 1;
    [SerializeField] private float rotationSpeed = 1;
    [SerializeField] private int maxWillPower = 100;
    [Header("Debug")]
    [SerializeField] private int willPower;

    private Rigidbody2D rb;
    private WeaponManager weaponManager;
    private Vector2 moveDirection;

    private int lives = 10;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        weaponManager = GetComponent<WeaponManager>();
    }

    private void FixedUpdate()
    {
        if (moveDirection != Vector2.zero)
        {
            Move();
            Rotate();

            anim.SetBool("IsWalking", true);

            float FacingRightPDot = Vector3.Dot(moveDirection, Vector3.right);
            if (FacingRightPDot > 0)
            {
                bodySprite.localScale = new Vector3(1, 1, 1);
            }
            else if (FacingRightPDot < 0)
            {
                bodySprite.localScale = new Vector3(-1, 1, 1);
            }
        }
        else
        {
            anim.SetBool("IsWalking", false);
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
        weaponTarget.rotation = Quaternion.RotateTowards(weaponTarget.rotation, newRotation, rotationSpeed * Time.fixedDeltaTime);
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

    public void IncreaseWillPower(int amount)
    {
        willPower += amount;
        if (willPower > maxWillPower)
            willPower = maxWillPower;
    }

    public Transform GetWeaponTarget() => weaponTarget;

    public void DealDamage(int damage)
    {
        lives -= damage;
    }
}
