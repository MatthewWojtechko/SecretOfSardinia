using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum SwimType { NORMAL, STRAFE };

// TO DO: Remove double tap? Not useful.

public class SardineSwim : MonoBehaviour
{
    public bool isActive = false;

    [Header("Components")]
    public Swimput currentInput;
    public Animator animator;
    public Rigidbody body;
    public Skim skimScript;
    public DiveBoost diveBoostScript;
    public Targeting targetingScript;
    public Strafe strafeScript;
    public SpeedParticles speedParticleScript;
    public CamLookPoint camPoint;
    public AutoRotation autoRotScript;
    public Transform playerHeadTransform;
    public Transform rotationHelper;
    public PlayerSwimmingEffects visualEffects;
    public SlopeRotation sloptation;
    public ResetSardine resetter;
    public PlayerImmobilizer Immobilizer;
    public PlayerHealth Health;
    public Transform behindPoint;

    [Header("Animation")]
    public float turnAnimLerp = 0.7f;
    private float xTurnAnim = 0;
    private float yTurnAnim = 0;
    public float visualMinimumBoost = 10;
    public Transform middleBone;

    public float animationBaseSpeed;
    public float animSpeedMagMult;
    public float animSpeedRotMult;

    public PlayerSpeedManager Speed;

    // Current Magnitude //
    private float standardMagnitude;  // regular magnitude based on the player's input
    private float skimBoost;          // additional magnitude from skimming
    public float compositeMagnitude; // Total magnitude
    public float boostAmount;         // Amount larger compositeMagnitude is than what the magnitude would be without boosting (compositeMagnitude - standardMagnitude)

    [Header("Rotation")]
    [Space(10)]
    public bool freeRotate = false;
    public float autoRotLerp;

    private float mag, lerp;

    // Private fields used to calculate the current rotation
    public static float yAxisRotationAmount;
    public static float xAxisRotationAmount;
    private float currentLeftRightRotMag;
    private float currentUpDownRotMag;

    [Header("Player")]
    public static Transform playerTransform;
    public static SardineSwim instance;
    public static Rigidbody playerRigid;

    SwimType currentState = SwimType.NORMAL;

    //  ACTIONS
    public static event Action onRelease;
    public static event Action onUpdateComplete;

    public void orientAtSpawn(Transform spawnPoint)
    {
        yAxisRotationAmount = spawnPoint.rotation.eulerAngles.y;
    }

    public void Start()
    {
        if (isActive)
            setup();
    }

    public void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    public static void setup()
    {
        //Time.timeScale = 0.1f;//
        instance.body.inertiaTensorRotation = Quaternion.identity; // What exactly does this do? Needed?
        instance.body.velocity = Vector3.zero;
        playerTransform = instance.transform;
        //Debug.Log("is this getting called?");
        playerRigid = instance.body;
        //instance = this;

        instance.compositeMagnitude = 0;
        instance.currentLeftRightRotMag = 0;
        instance.currentUpDownRotMag = 0;
        instance.standardMagnitude = 0;
        instance.skimBoost = 0;  
        instance.boostAmount = 0;

        instance.xTurnAnim = 0;
        instance.yTurnAnim = 0;
        instance.updateAnimation();

        xAxisRotationAmount = 0;
        instance.mag = instance.lerp = 0; 

        instance.currentInput.resetFields();

        //resetter.spawnIn();
        //Time.timeScale = 0.03f;
    }

    void updateAnimation()
    {
        animator.SetFloat("Forward", animationBaseSpeed + ((Speed.getMaxSpeedForce() + boostAmount) * animSpeedMagMult) + (Mathf.Abs(currentLeftRightRotMag) * animSpeedRotMult) + (Mathf.Abs(currentUpDownRotMag) * animSpeedRotMult));
        animationValues();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isActive || Immobilizer.isPlayerImmobile())
            return;

        // DEBUG - Use this data to determine the relationship between force vs. speed, so inform the approxSpeed() function.
        //if (compositeMagnitude != 0)
        //    Debug.Log(compositeMagnitude + " " + playerRigid.velocity.magnitude);//

        // Set new input
        //PlayerCharacterInput.setInput();
        currentInput.updates();

        // Make this a little more sophistated - more "effort" when swimming upwards, when beginnning the turn in a new direction. More effort when initiating a skim boost. More effort (faster animation) when swimming fast? Less because you're moving fast but expending same effort due to boosts? Or the same?
        animator.SetFloat("Forward", animationBaseSpeed + (compositeMagnitude * animSpeedMagMult) + (Mathf.Abs(currentLeftRightRotMag) * animSpeedRotMult) + (Mathf.Abs(currentUpDownRotMag) * animSpeedRotMult));
        animationValues();

        if (!freeRotate)
            diveBoostScript.calculate();

        speedParticleScript.set(compositeMagnitude, standardMagnitude);

        //if (currentInput.inputReleased)
        //    onRelease?.Invoke();

        onUpdateComplete?.Invoke();   // Needed? 
    }

    void FixedUpdate()
    {
        if (!isActive || Immobilizer.isPlayerImmobile())
            return;

        // Rotation - either targeting or player-controlled
        if (targetingScript.inRange() && currentInput.forward) //if (PlayerCharacterInput.currentInput.target && targetingScript.inRange())
            targetingScript.activate();

        if (strafeScript.isStrafing/*currentInput.strafe*/)    // If player is strafing
        {
            currentState = SwimType.STRAFE;
            strafeScript.move();
        }
        else
        {
            //if (currentState == SwimType.STRAFE)   // Match rotation to how it was while skimming
            //{
            //    currentState = SwimType.NORMAL;
            //    yAxisRotationAmount = this.transform.rotation.eulerAngles.y;
            //    xAxisRotationAmount = this.transform.rotation.eulerAngles.x;
            //}

            if (!freeRotate)                           // Otherwise normal rotation
                newSwimRotation(currentInput.rotation, CreviceCheck.inTunnel/*currentInput.doubleForward.isDouble || currentInput.doubleBackward.isDouble*/, false /*currentInput.rotationFast*/);//swimRotation(currentInput.rotation, currentInput.rotationFlick);
            else                                            // otherwise free rotation
                freeRotation(currentInput.rotation, CreviceCheck.inTunnel/*currentInput.doubleForward.isDouble || currentInput.doubleBackward.isDouble*/, currentInput.rotationFast);
        }
        // Swimming - calculate magnitude and apply force
        calculateSwimForce();
        compositeMagnitude = (standardMagnitude + skimBoost) * (1 + diveBoostScript.diveBoostFactor);
        boostAmount = compositeMagnitude - standardMagnitude;
        swim(compositeMagnitude);

        // Camera's target's must be updated at the proper time in order to prevent jitter. (Apparently)
        if (LookPointParent.instance != null)
            LookPointParent.updateTransform();
        camPoint.UpdateCamLookPoint();

        // Update seen effect for whales after the fish has moved to keep effect in time
        //SeenEffect.updatePosition();
    }

    public void resetRotation(Vector3 rot)
    {
        xAxisRotationAmount = rot.x;
        yAxisRotationAmount = rot.y;
    }

    // Given input, determine the standard input to apply to the player for swimming, as well as the amount of magnitude from skim boosting.
    // To be called in FixedUpdate().
    void calculateSwimForce()
    {
        if (currentInput.forward)
        {
            //if (CreviceCheck.inTunnel)
            //{
            //    standardMagnitude = Speed.getSlowSpeedForce();
            //}

            skimBoost = skimScript.currentBoostMag;
            standardMagnitude = Speed.getStandardSpeedForce();
        }
        else if (currentInput.backward)
        {
            //if (CreviceCheck.inTunnel)
            //{
            //    standardMagnitude = Speed.getSlowBackwardSpeedForce();
            //}
            standardMagnitude = Speed.getBackwardSpeedForce();
            skimBoost = 0;
        }
        else
        {
            standardMagnitude = 0;
            skimBoost = 0;
        }

        //Debug.Log("MAG: " + compositeMag);//
    }

    // Apply a forward force with the given magnitude.
    void swim(float force)
    {
        body.AddForce(transform.forward * force);
        body.AddForce(transform.forward * skimScript.getImpulse(), ForceMode.Impulse); // Applies impulse, if available
        //body.AddForce(skimScript.currentBoostMag * transform.forward, ForceMode.Impulse);//
    }

    void newSwimRotation(Vector2 input, bool slow, bool fast)
    {
        if (currentInput.camera)    // No rotations while manually operating camera
        {
            return;
        }

        // YAW //
        float yaw = getYaw(input, slow, fast);

        // PITCH //
        float targetPitch;  // TODO: change pitch to be based on button press vs. stick. Button press slowly turns up, analog tilt lerps to where you are tilted to.
        if (input.y < 0)
        {
            targetPitch = 89.5f;
        }
        else if (input.y > 0)
        {
            //xRotTarget = -89.5f;
            targetPitch = -sloptation.getMaxRotation();
        }
        else
        {
            targetPitch = 0;     // Rotate back to neutral
        }

        xAxisRotationAmount = getPitch(targetPitch, input.y, slow, fast);

        // DO IT //
        //this.transform.eulerAngles = new Vector3(xAxisRotationAmount, yAxisRotationAmount, 0);
        //body.rotation = Quaternion.Euler(xAxisRotationAmount, yAxisRotationAmount, 0);
        body.MoveRotation(Quaternion.Euler(xAxisRotationAmount, yAxisRotationAmount, 0));
        //body.MoveRotation(this.transform.rotation * Quaternion.Euler(xAxisRotationAmount, yAxisRotationAmount, 0));//
    }

    float getYaw(Vector2 input, bool slow, bool fast)
    {
        if (fast)
        {
            mag = Speed.leftRightMagFAST;
            lerp = Speed.leftRightRotLerpFAST;
        }
        else
        {
            mag = Speed.getLeftRightForceMagntiude();
            lerp = Speed.getLeftRightRotationLerp();
        }

        currentLeftRightRotMag = Mathf.Lerp(currentLeftRightRotMag, input.x * (mag + skimScript.leftRightRotRateBoostAddition), lerp);
        if (!currentInput.forward || strafeScript.strafeDebugOverride)
            yAxisRotationAmount += currentLeftRightRotMag;
        return yAxisRotationAmount;
    }

    float getPitch(float targetPitch, float input, bool slow, bool fast)
    {
        if (fast)
        {
            lerp = Speed.upDownRotLerpFAST;
        }
        else
        {
            lerp = Speed.getUpDownRotationLerp();
        }
        //Debug.Log("target: " + targetPitch);//
        xAxisRotationAmount = Mathf.Lerp(xAxisRotationAmount, targetPitch * Mathf.Abs(input), lerp * Time.deltaTime);
        return xAxisRotationAmount;
    }

    void freeRotation(Vector2 input, bool slow, bool fast)
    {
        if (currentInput.camera)    // No rotations while manually operating camera
        {
            return;
        }

        // X-Axis - Up & Down //
        //xAxisRotationAmount = body.rotation.eulerAngles.x;
        //currentUpDownRotMag = Mathf.Lerp(currentUpDownRotMag, -1 * input.y * (mag + skimScript.upDownLerpBoostAddition), lerp);
        Vector3 pitch = -1 * this.transform.right * input.y;
        Vector3 yaw = this.transform.up * input.x;
        if (fast)
        {
            pitch *= Speed.leftRightMagFAST;
            yaw *= Speed.leftRightMagFAST;
        }
        else
        {
            pitch *= Speed.getLeftRightForceMagntiude();
            yaw *= Speed.getLeftRightForceMagntiude();
        }

        // Do it!
        body.MoveRotation(body.rotation * Quaternion.Euler(pitch + yaw));
    }

    void animationValues()
    {
        xTurnAnim = Mathf.Lerp(xTurnAnim, currentInput.rotation.x, turnAnimLerp * Time.deltaTime);
        yTurnAnim = Mathf.Lerp(yTurnAnim, currentInput.rotation.y, turnAnimLerp * Time.deltaTime);

        animator.SetFloat("joyX", xTurnAnim);
        animator.SetFloat("joyY", yTurnAnim);
    }

    /*
     * <summary>
     * A cost-effective estimate of the player's speed.
     * </summary>
     */
    public float approxSpeed()
    {
        return compositeMagnitude / 8.56f;//playerRigid.mass;
    }
}