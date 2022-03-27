using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIncarnation : MonoBehaviour
{
    public float facingDirectionSmoothing;

    [Range(0, 1)] public float ctlrCastSensitivity;
    public float reach;
    public float castSpeed;
    public float passiveSpinSpeed;
    public float movingSpinSpeed;

    [Range(0, 1)] public float ctlrApireSensitivity;
    public float aspireSpeed;
    public float aspireSpinSpeed;

    private StateController stateCtlr;
    private InventoryManager invManager;

    private Rigidbody2D playerRB;

    private Transform arm1Trans;
    private Animator arm1Anim;
    private SpriteRenderer arm1Sprite;

    private GameObject incarnation1;
    private Transform inc1Trans;
    private SpriteRenderer inc1Sprite;
    private ParticleSystem inc1ActivePS;
    private ParticleSystem inc1StopCastingPS;
    private GameObject inc1Trail;

    private Vector2 incDir;

    private float castCount;
    private bool isCasting;
    private Vector2 currentArmRotationVelocity;

    private float aspireCount;
    private Vector3 aspireOriginPos;
    private Vector3 aspireTargetPos;
    private Vector3 aspireArmOriginLocalPos;

    void Start()
    {
        stateCtlr = GameObject.FindGameObjectWithTag("GameController").GetComponent<StateController>();
        invManager = GetComponent<InventoryManager>();

        playerRB = transform.GetComponent<Rigidbody2D>();

        arm1Trans = transform.Find("Player Arm 1");
        arm1Sprite = arm1Trans.Find("PA1 Sprite").GetComponent<SpriteRenderer>();
        arm1Anim = arm1Trans.Find("PA1 Sprite").GetComponent<Animator>();

        //UpdateIncarnationRefs();

        incDir = Vector2.zero;

        castCount = 0;
        isCasting = false;
        currentArmRotationVelocity = Vector2.zero;

        aspireCount = 0;
        aspireOriginPos = Vector3.zero;
        aspireTargetPos = Vector3.zero;
        aspireArmOriginLocalPos = Vector3.zero;
    }
    
    void Update()
    {
        if (true)// stateCtlr.playerState == PlayerStates.active)
        {
            // Get current arm direction
            UpdateSmoothedFacingDirection();

            // Rotate incarnation before arm is aimed to avoid rotation direction issues
            if (incarnation1 != null)
            {
                RotateIncarnation(1);
            }

            // Aim arm
            if (incDir != Vector2.zero)
            {
                arm1Trans.up = incDir;
            }

            if (incarnation1 != null)
            {
                // -Detect Incarnation Use-
                // ~Casting~

                // Begin cast
                if (stateCtlr.inputMode == InputModes.keyboardAndMouse && Input.GetButton("Cast Mouse") &&
                    !isCasting ||
                    stateCtlr.inputMode == InputModes.controller && Input.GetAxisRaw("Cast Controller") >=
                    ctlrCastSensitivity && !isCasting)
                {
                    BeginCast();
                }

                // End cast
                if (stateCtlr.inputMode == InputModes.keyboardAndMouse && !Input.GetButton("Cast Mouse") &&
                    isCasting ||
                    stateCtlr.inputMode == InputModes.controller && Input.GetAxisRaw("Cast Controller") <
                    ctlrCastSensitivity && isCasting)
                {
                    EndCast();
                }

                // ~Aspiring~

                // Begin aspire
                if (stateCtlr.inputMode == InputModes.keyboardAndMouse && Input.GetButton("Aspire Mouse") /*&&
                    stateCtlr.playerState != PlayerStates.aspiring && castCount == castSpeed*/ ||
                    stateCtlr.inputMode == InputModes.controller && Input.GetAxisRaw("Aspire Controller") >=
                    ctlrApireSensitivity && /*stateCtlr.playerState != PlayerStates.aspiring &&*/ castCount == castSpeed)
                {
                    BeginApire();
                }

                // -Perform Incarnation Actions-
                // ~Casting action~
                if (isCasting)
                {
                    UpdateCastCounter();
                    UpdateCastPosition();
                }
            }
        }

        // ~Aspiring action~
        if (false)// stateCtlr.playerState == PlayerStates.aspiring)
        {
            UpdateAspireCounter();
            
            RotateIncarnation(aspireSpinSpeed);

            UpdateAspirePositions();
            
            if (aspireCount == aspireSpeed)
            {
                EndAspire();
            }
        }
    }

    private void UpdateSmoothedFacingDirection()
    {
        Vector2 lookDir = Vector2.zero;

        if (stateCtlr.inputMode == InputModes.keyboardAndMouse)
        {
            Vector2 mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            lookDir = (mousePos - (Vector2)arm1Trans.position).normalized;
        }
        else if (stateCtlr.inputMode == InputModes.controller)
        {
            lookDir = new Vector2(Input.GetAxisRaw("Cast Horizontal"), Input.GetAxisRaw("Cast Vertical"));
        }

        incDir = Vector2.SmoothDamp(arm1Trans.up, lookDir, ref currentArmRotationVelocity,
            facingDirectionSmoothing);
    }

    private void RotateIncarnation(float _multiplier)
    {
        float angleDif = Vector2.SignedAngle(arm1Trans.up, incDir);

        if (angleDif > 0 && passiveSpinSpeed < 0)
        {
            passiveSpinSpeed *= -1;
        }
        if (angleDif < 0 && passiveSpinSpeed > 0)
        {
            passiveSpinSpeed *= -1;
        }

        if (_multiplier != 1)
        {
            inc1Trans.rotation *= Quaternion.AngleAxis(passiveSpinSpeed * _multiplier, Vector3.forward);
        }
        else
        {
            inc1Trans.rotation *= Quaternion.AngleAxis(passiveSpinSpeed + movingSpinSpeed * angleDif, Vector3.forward);
        }
    }

    private void BeginCast()
    {
        isCasting = true;

        arm1Anim.SetBool("CastingIncarnation", true);

        arm1Sprite.enabled = true;
        inc1Sprite.enabled = true;

        inc1ActivePS.Play();

        incarnation1.GetComponent<CircleCollider2D>().enabled = true;
    }

    private void EndCast()
    {
        isCasting = false;

        arm1Anim.SetBool("CastingIncarnation", false);

        inc1Sprite.enabled = false;

        GameObject.Destroy(inc1Trail);

        inc1ActivePS.Stop();
        inc1StopCastingPS.Play();

        incarnation1.GetComponent<CircleCollider2D>().enabled = false;

        castCount = 0;
    }

    private void BeginApire()
    {
        //stateCtlr.playerState = PlayerStates.aspiring;

        playerRB.simulated = false;

        aspireOriginPos = transform.position;
        aspireTargetPos = new Vector3(aspireOriginPos.x + incDir.normalized.x * reach,
                                      aspireOriginPos.y + incDir.normalized.y * reach,
                                      aspireOriginPos.z);

        arm1Anim.enabled = false;
        aspireArmOriginLocalPos = arm1Trans.Find("PA1 Sprite").localPosition;
    }

    private void EndAspire()
    {
        //stateCtlr.playerState = PlayerStates.active;
        playerRB.simulated = true;
        arm1Anim.enabled = true;
        aspireCount = 0;

        EndCast();
    }

    private void UpdateCastCounter()
    {
        if (castCount < castSpeed)
        {
            castCount += Time.deltaTime;
        }
        if (castCount > castSpeed)
        {
            castCount = castSpeed;
        }
    }

    private void UpdateCastPosition()
    {
        Vector3 newLocalPos = new Vector3(inc1Trans.localPosition.x, reach * (castCount / castSpeed),
                    inc1Trans.localPosition.z);

        inc1Trans.localPosition = newLocalPos;
    }

    private void UpdateAspireCounter()
    {
        //Rewrite to include smooth curve
        if (aspireCount < aspireSpeed)
        {
            aspireCount += Time.deltaTime;
        }
        if (aspireCount > aspireSpeed)
        {
            aspireCount = aspireSpeed;
        }
    }

    private void UpdateAspirePositions()
    {
        float aspirePerc = aspireCount / aspireSpeed;
        Vector3 reachLocalDir = new Vector3(0, reach, 0);

        transform.position = Vector3.Lerp(aspireOriginPos, aspireTargetPos, aspirePerc);

        arm1Trans.Find("PA1 Sprite").localPosition = Vector3.Lerp(aspireArmOriginLocalPos, Vector3.zero, aspirePerc);

        incarnation1.transform.localPosition = Vector3.Lerp(reachLocalDir, Vector3.zero, aspirePerc);
    }

    public void UpdateIncarnationRefs()
    {
        if (invManager.equippedWeapon1 != null)
        {
            incarnation1 = invManager.equippedWeapon1;
            inc1Trans = incarnation1.transform;
            inc1Sprite = inc1Trans.Find("Sprite").GetComponent<SpriteRenderer>();
            inc1ActivePS = inc1Trans.Find("Active PS").GetComponent<ParticleSystem>();
            inc1StopCastingPS = inc1Trans.Find("Stop Casting PS").GetComponent<ParticleSystem>();
        }
    }
}
