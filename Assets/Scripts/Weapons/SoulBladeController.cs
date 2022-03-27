using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulBladeController : WeaponController
{
    [SerializeField] private float swingAttackTime = 0.5f;
    [SerializeField] private float swingAttackDistance = 0.5f;

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
    private bool swingFromRight;
    private float attackTimer;

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

    void Update()
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

    private void OnPrimaryAttack()
    {
        if (attackState == AttackStates.Idle)
        {
            Debug.Log("Began primary attack");
            attackState = AttackStates.PrimaryAttackSwinging;
        }
    }

    private void HandleSwingAttack()
    {
        attackTimer += Time.deltaTime;

        float attackTimerPerc = attackTimer / swingAttackTime;

        if (attackTimerPerc >= 1)
        {
            attackTimerPerc = 1;
        }

        // For running from 0 to 0.5 then back to 0 during the timer running from 0 to 1
        float attackTimerHalfPeakPerc = attackTimerPerc;
        if (attackTimerPerc > 0.5f)
            attackTimerHalfPeakPerc = 1 - attackTimerPerc;

        if (swingFromRight)
        {
            // Set length away from player
            Vector2 playerRight = Quaternion.Euler(0, 0, 90) * player.up;
            Vector2 targetPos = playerRight * swingAttackDistance * attackTimerHalfPeakPerc;
            // Set rotation around player
            targetPos = Quaternion.Euler(0, 0, -180 * attackTimerPerc) * targetPos;
            // Set final local position
            soulBlade.transform.localPosition = targetPos;
        }
        else
        {
            Vector2 playerLeft = Quaternion.Euler(0, 0, -90) * player.up;
            Vector2 targetPos = playerLeft * swingAttackDistance * attackTimerHalfPeakPerc;
            targetPos = Quaternion.Euler(0, 0, 180 * attackTimerPerc) * targetPos;
            soulBlade.transform.localPosition = targetPos;
        }

        if (attackTimerPerc >= 1)
        {
            // End attack
            attackTimer = 0;
            attackState = AttackStates.Idle;
            swingFromRight = !swingFromRight;
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
        attackTimer = 0;
        attackState = AttackStates.Idle;
        swingFromRight = true;
    }
}
