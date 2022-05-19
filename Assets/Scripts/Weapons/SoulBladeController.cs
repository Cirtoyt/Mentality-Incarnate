using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SoulBladeController : WeaponController
{
    [SerializeField] private float positionSmoothing = 1;
    [SerializeField] private float rotationSmoothing = 1;
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

    private Player player;
    private Transform weaponTarget;
    private SoulBlade soulBlade;
    private Vector3 targetPos;
    private Quaternion targetLocalRot;
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

    override public void SetupController(Player player, Weapon soulBlade)
    {
        this.player = player;
        this.soulBlade = (SoulBlade)soulBlade;
        weaponTarget = this.player.GetWeaponTarget();
        this.soulBlade.SetPlayer(this.player);
    }

    void Update()
    {
        switch (attackState)
        {
            case AttackStates.Idle:
                targetPos = weaponTarget.position;
                targetLocalRot = weaponTarget.rotation;
                break;
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

        // Smooth move weapon towards target pos/rot
        soulBlade.transform.position = Vector3.Slerp(soulBlade.transform.position, targetPos, positionSmoothing);
        soulBlade.transform.rotation = Quaternion.Slerp(soulBlade.transform.rotation, targetLocalRot, rotationSmoothing);
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
                    currentSwingPivotDir = weaponTarget.right;
                    currentSwingRotationDir = 180;
                }
                else
                {
                    currentSwingPivotDir = -weaponTarget.right;
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
        targetPos = currentSwingPivotDir * swingAttackDistance * swingAttackDistanceSmoothing.Evaluate(attackTimerHalfPeakPerc);
        // Set rotation around player
        targetPos = Quaternion.AngleAxis(currentSwingRotationDir * attackTimerPerc, Vector3.forward) * targetPos;
        // Set final local position
        targetPos = weaponTarget.position + targetPos;

        // Rotation

        // Get spin direction
        float rotationAngle;
        if (swingFromRight)
            rotationAngle = swingAttackBladeSpinSpeed;
        else
            rotationAngle = -swingAttackBladeSpinSpeed;
        // Update rotation with angle
        targetLocalRot = Quaternion.AngleAxis(rotationAngle, Vector3.forward);

        // Check attack is done
        if (doneSwinging)
        {
            swingAttackTimer = 0;
            swingFromRight = !swingFromRight;
            targetLocalRot = Quaternion.identity;

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
                    currentSwingPivotDir = weaponTarget.right;
                    currentSwingRotationDir = 180;
                }
                else
                {
                    currentSwingPivotDir = -weaponTarget.right;
                    currentSwingRotationDir = -180;
                }
            }
        }
    }

    private void OnSecondaryAttack()
    {
        if (attackState == AttackStates.Idle)
        {
            //attackState = AttackStates.SecondaryAttackFlyingAway;
        }
    }

    private void HandleThrowAttack()
    {
        
    }

    private void OnPrimarySkill()
    {
        if (attackState == AttackStates.Idle)
        {
            //attackState = AttackStates.PrimarySkillTravelingToSoulBlade;
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
