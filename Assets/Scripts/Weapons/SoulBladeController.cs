using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SoulBladeController : WeaponController
{
    [SerializeField] private float swingAttackTime = 0.5f;
    [SerializeField] private float swingAttackDistance = 0.5f;
    [SerializeField] private AnimationCurve swingAttackDistanceSmoothing;
    [SerializeField] private float swingAttackBladeSpinSpeed = 1;

    private enum AttackStates
    {
        Idle,
        PrimaryAttackSwinging,
        SecondaryAttackFlyingAway,
        SecondaryAttackFlyingBack,
        PrimarySkillTravelingToSoulBlade,
    }

    private Transform player;
    //private WeaponManager weaponManager;
    private SoulBlade soulBlade;
    private AttackStates attackState;
    private bool queueSwingAttack;
    private bool swingFromRight;
    private Vector3 currentSwingPivotDir;
    private float currentSwingRotationDir;
    private float swingAttackTimer;

    private void Awake()
    {
        //player = GetComponent<Player>();
        //weaponManager = GetComponent<WeaponManager>();

        ResetController();
    }

    override public void SetupController(Transform player, Weapon soulBlade)
    {
        this.player = player;
        this.soulBlade = (SoulBlade)soulBlade;
    }

    void FixedUpdate()
    {
        switch (attackState)
        {
            case AttackStates.PrimaryAttackSwinging:
                HandleSwingAttack();
                break;
            case AttackStates.SecondaryAttackFlyingAway:
            case AttackStates.SecondaryAttackFlyingBack:
                HandleThrowAttack();
                break;
            case AttackStates.PrimarySkillTravelingToSoulBlade:
                HandleAspireSkill();
                break;
        }
    }

    private void OnPrimaryAttack(InputValue value)
    {
        if (value.Get<float>() >= 0.5f)
        {
            if (attackState == AttackStates.Idle)
            {
                //Debug.Log("Began primary attack");
                attackState = AttackStates.PrimaryAttackSwinging;

                if (swingFromRight)
                {
                    currentSwingPivotDir = player.right;
                    currentSwingRotationDir = 180;
                }
                else
                {
                    currentSwingPivotDir = -player.right;
                    currentSwingRotationDir = -180;
                }
            }
            else
            {
                queueSwingAttack = true;
            }
        }
        else
        {
            queueSwingAttack = false;
        }
    }

    private void HandleSwingAttack()
    {
        swingAttackTimer += Time.deltaTime;

        bool doneSwinging = false;
        float attackTimerPerc = swingAttackTimer / swingAttackTime;

        if (attackTimerPerc >= 1)
        {
            attackTimerPerc = 1;
            doneSwinging = true;
        }

        // Position

        // For running from 0 to 1 then back to 0 during the timer running from 0 to 1
        float attackTimerHalfPeakPerc = attackTimerPerc * 2;
        if (attackTimerHalfPeakPerc > 1)
            attackTimerHalfPeakPerc = 2 - attackTimerHalfPeakPerc;

        // Set length away from player
        Vector3 targetPos = currentSwingPivotDir * swingAttackDistance * swingAttackDistanceSmoothing.Evaluate(attackTimerHalfPeakPerc);
        // Set rotation around player
        targetPos = Quaternion.AngleAxis(currentSwingRotationDir * attackTimerPerc, Vector3.forward) * targetPos;
        // Set final local position
        soulBlade.transform.position = player.position + targetPos;

        // Rotation

        // Get spin direction
        float rotationAngle;
        if (swingFromRight)
            rotationAngle = swingAttackBladeSpinSpeed;
        else
            rotationAngle = -swingAttackBladeSpinSpeed;
        // Update rotation with angle
        soulBlade.transform.Rotate(Vector3.forward, rotationAngle);

        // Check attack is done
        if (doneSwinging)
        {
            swingAttackTimer = 0;
            swingFromRight = !swingFromRight;
            soulBlade.transform.localRotation = Quaternion.identity;

            if (!queueSwingAttack)
            {
                // End attack
                attackState = AttackStates.Idle;
            }
            else
            {
                queueSwingAttack = false;

                // Stay in swing attack state and flip swing direction vars
                if (swingFromRight)
                {
                    currentSwingPivotDir = player.right;
                    currentSwingRotationDir = 180;
                }
                else
                {
                    currentSwingPivotDir = -player.right;
                    currentSwingRotationDir = -180;
                }
            }
        }
    }

    private void OnSecondaryAttack()
    {
        if (attackState == AttackStates.Idle)
        {
            attackState = AttackStates.SecondaryAttackFlyingAway;
        }
    }

    private void HandleThrowAttack()
    {
        
    }

    private void OnPrimarySkill()
    {
        if (attackState == AttackStates.Idle)
        {
            attackState = AttackStates.PrimarySkillTravelingToSoulBlade;
        }
    }

    private void HandleAspireSkill()
    {

    }

    public override void ResetController()
    {
        swingAttackTimer = 0;
        attackState = AttackStates.Idle;
        swingFromRight = true;
    }
}
