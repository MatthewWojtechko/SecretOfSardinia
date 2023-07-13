using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
public class Skim : MonoBehaviour
{
    public DiveBoost dive;

    [Header("Boosting Magnitudes")]
    [SerializeField] private float boostImpulseMag;
    [SerializeField] private float boostConstantMagAddend;

    [SerializeField] private float minWaitSec = 0.5f;
    private float boostStartTime = -10f;

    // Add these * the currentTotalBoostMag to their respective variables
    // This keeps the rotation speeds proportional to increasing speeds.
    [SerializeField] private float upDownRotLerpBoostRate;
    [SerializeField] private float leftRightRotRateBoostRate;
    [SerializeField] private float leftRightRotDecelBoostRate;

    [SerializeField] private float boostDecelPerSec = 1;

    // Results
    public float currentBoostMag;
    //public float currentImpulse;
    private bool impulseAvailabile = false;

    public float upDownLerpBoostAddition;
    public float leftRightRotRateBoostAddition;
    public float leftRightRotDecelBoostAddition;

    // Ground Spherecast
    [SerializeField] private float maxGroundDist;
    [SerializeField] private float spherecastRadius = 0.02f;
    [SerializeField] private float warningDistance = 0.08f;
    [SerializeField] private float targetDistance = 0.02f;
    [SerializeField] private LayerMask groundLayer;

    // Input Window
    [SerializeField] private float inputWindow = 0.4f;
    private float timeNearedGround = -1;

    // Input
    [SerializeField] private float buttonTapMaxSec = 0.5f;
    private float buttonLastHeldTime = -10;

    // Visualization
    public SpeedParticles particleScript;
    public DiveBoost boostScript;
    public ParticleSystem[] proximityParticles;
    public ParticleSystem.MainModule[] proximityParticlesMain;
    public float proximityParticlesNormalSpeed = 3;
    public float proximityParticlesFastSpeed = 10;
    public LineRenderer proximityLine;
    public SpriteRenderer triangleSprite;
    public SpriteRenderer backgroundSprite;
    public float minLineWidth = 0.01f;
    public float maxLineWarningWidth = 0.1f;
    public float targetLineWidth = 0.2f;
    public Color lineColorStart = new Color(1, 1, 1, 0.5f);
    public Color lineColorEnd = new Color(0, 0, 0, 1);
    Vector3[] linePoints = new Vector3[2];
    public float idleDuration = 0.5f;
    public float fadeAwayDuration = 1;
    public float fadeInDuration = 0.2f;
    public float fadeInTime;
    public float fadeOutTime;
    bool isIdle = false;
    float idleCounter = 0;

    // Flags
    public bool isNearGround;
    //private bool activatedBoost;
    private bool lostBoost;

    public static float groundSteepness;

    public Swimput currentInput;

    private bool isBoostAvailShowing = false;

    public static event Action onBoost;

    bool isInputting = false;

    public SpriteRenderer pointMarker;
    public Vector3 markerMaxScale = new Vector3(0.005f, 0.01f, 0.0025f);
    public Vector3 markerMinScale = new Vector3(0.0025f, 0.0025f, 0.0025f);

    public bool isAtOracle = false;

    public bool lineNeedsToDie = false;
    RaycastHit oldHit;

    public void Start()
    {
        OraclePlayerControl.onOracleBegun += atOracle;
        OraclePlayerControl.onOracleExit += notAtOracle;

        proximityParticlesMain = new ParticleSystem.MainModule[proximityParticles.Length];
        for (int i = 0; i < proximityParticles.Length; i++)
        {
            proximityParticlesMain[i] = proximityParticles[i].main;
        }
    }
    public void OnDestroy()
    {
        OraclePlayerControl.onOracleBegun -= atOracle;
        OraclePlayerControl.onOracleExit -= notAtOracle;
    }

    void atOracle()
    {
        isAtOracle = true;
    }
    void notAtOracle()
    {
        isAtOracle = false;
    }

    private void Update()
    {
        UpdateFields();

        ////Debug.Log("currentBoostMag: " + currentBoostMag +
        //        "\ncurrentImpulse: " + currentImpulse +
        //        "\nupDownLerpBoostAddition: " + upDownLerpBoostAddition +
        //        "\nleftRightRotRateBoostAddition: " + leftRightRotRateBoostAddition +
        //        "\nleftRightRotDecelBoostAddition: " + leftRightRotDecelBoostAddition);
        // Raycast //
        RaycastHit hit;
        bool isHit = Physics.SphereCast(this.transform.position, spherecastRadius, this.transform.up * -1, out hit, warningDistance, groundLayer);
        float distance = Vector3.Distance(hit.point, this.transform.position);
        if (isHit && !isAtOracle)
        {
            if (distance <= targetDistance && !isNearGround)
            {
                groundSteepness = hit.normal.y;
                timeNearedGround = TimeKeeper.getOverworld().getTime();
                isNearGround = true;
                proximityLine.gameObject.SetActive(true);

                if (isInputting)
                {
                    setParticleSpeed(proximityParticlesNormalSpeed);
                    proximityParticles[0].Play();
                }


            }
        }
        else
        {
            isNearGround = false;
        }
        if (Time.time != 0)
            drawLine(isHit, hit, distance);
        else
            disableLine();
    }

    void setParticleSpeed(float f)
    {
        for (int i = 0; i < proximityParticlesMain.Length; i++)
        {
            proximityParticlesMain[i].simulationSpeed = f;
        }

    }

    void showBoostAvail()
    {
        isBoostAvailShowing = true;
        //SardineOutlineManager.boostAvailable();
    }

    void removeShowBoostAvail()
    {
        isBoostAvailShowing = false;
        //SardineOutlineManager.boostNotAvailable();
    }

    public void OnSkimButton()
    {
        //// Turn on UI if off
        //if (!UI.activeSelf)
        //    UI.SetActive(true); // ui

        if (isBoostQualifying())
        {
            //activatedBoost = true;
            particleScript.activateSpeedBoost();

            setParticleSpeed(proximityParticlesFastSpeed);
            proximityLine.gameObject.SetActive(false);

            // Add boost [OPTION 1]
            currentBoostMag += boostConstantMagAddend;

            // Multiply boost [OPTION 2]
            //// Boosts stack multiplicitvely. Only multiply if the product is greater than the minimum boost.
            //currentBoostMag *= boostConstantMagAddend;
            //if (currentBoostMag < boostConstantMagAddend)
            //    currentBoostMag = boostConstantMagAddend;
            impulseAvailabile = true;

            // Broadcast to cameras so they know to shake // DON'T THINK THIS IS USED AS OF NOW. Use for sounds/particle effects?
            //Messenger.Broadcast(GameEvent.BOOST);
            onBoost?.Invoke();

            boostScript.onSkimStart();
            //activatedBoost = false;
        }
        else
        {
            this.loseIt();
            dive.loseIt();
        }
    }

    bool boostInCooldown()
    {
        return TimeKeeper.getOverworld().getTime() - boostStartTime <= minWaitSec;
    }

    bool isBoostQualifying()
    {
        return isNearGround && TimeKeeper.getOverworld().getTime() - timeNearedGround < inputWindow && isInputting;
    }

    // Should be called every frame
    void SkimUpdate()
    {
        if (boostInCooldown())
            return;

        if (isBoostQualifying())
        {
            // Turn on UI if off
            if (!isBoostAvailShowing)
                showBoostAvail();

            //// Input window
            //if (currentInput.getBoost())
            //{
            //    if (Time.time - buttonLastHeldTime < buttonTapMaxSec)
            //    {
            //        activatedBoost = true;
            //        particleScript.activateSpeedBoost();
            //    }
            //}
        }
        else if (isBoostAvailShowing) // If it's not skim time, but the effect is still up, turn it off
        {
            removeShowBoostAvail(); // turn off ui
        }

        if (!currentInput.forward) // Let go, so zero everything.
            loseIt();

        //if (currentInput.forward)
        //    buttonLastHeldTime = Time.time;
        //else // let go 
        //{
        //    // if let go outside of window (time expired, or not close enough to qualify)
        //    //if (windowStopwatch >= inputWindow || !nearGround)
        //        lostBoost = true;
        //}
    }

    void loseIt()
    {
        currentBoostMag = upDownLerpBoostAddition = leftRightRotRateBoostAddition = leftRightRotDecelBoostAddition = 0;
    }

    // Should be called by the SardineSwim script every frame.
    // Informs what the continual boost magnitude addition is, what the impulse magnitude is if there is one.
    // Once this function is run, the public fields are up-to-date.
    public void UpdateFields()
    {
        SkimUpdate();  // Calculations
        updateBoost(); // Modify final values
        //activatedBoost = lostBoost = false;          // Reset flags
        isInputting = currentInput.forward || currentInput.backward;
    }

    void updateBoost()
    {

        // Modify boost accordingly
        // Set impulse on/off.
        //if (activatedBoost)
        //{

        //}
        //else
        //{
            // Decelerate boost magnitude.
            currentBoostMag -= boostDecelPerSec * Time.deltaTime;
            if (currentBoostMag < 0)
                currentBoostMag = 0;
        //}

        // Modify other final result fields based on currentBoostMag
        upDownLerpBoostAddition = upDownRotLerpBoostRate * currentBoostMag;
        leftRightRotRateBoostAddition = leftRightRotRateBoostRate * currentBoostMag;
        leftRightRotDecelBoostAddition = leftRightRotDecelBoostRate * currentBoostMag;
    }

    public void drawLine(bool isHit, RaycastHit hit, float distance)
    {
        if ((isNearGround && TimeKeeper.getOverworld().getTime() - timeNearedGround >= inputWindow && isInputting) || boostInCooldown() || (distance <= targetDistance && isInputting) || distance > warningDistance || !isNearGround)
        {
            // TO DO: Instead of just disappearing, fade away
            //proximityLine.gameObject.SetActive(false);
            //return;
            lineNeedsToDie = true;
        }
        else
        {
            lineNeedsToDie = false;
        }

        //if (distance > targetDistance)
        //{
        // warning line
        float distancePercent = 1 - ((distance - targetDistance) / (warningDistance - targetDistance));
        proximityLine.startWidth = Mathf.Lerp(minLineWidth, maxLineWarningWidth, distancePercent);
        Color newColor = Color.Lerp(lineColorStart, lineColorEnd, distancePercent);
        setLineColor(newColor);
        setMarkScale(distancePercent);



        //proximityLine.startColor = newColor;
        //proximityLine.endColor = newColor;
        //}
        //else
        //{
        //    proximityLine.startWidth = targetLineWidth;
        //    proximityLine.enabled = false;
        //}
        if (isHit)
            linePoints = new Vector3[] { this.transform.position, hit.point };
        else
            linePoints = new Vector3[] { this.transform.position, this.transform.position + (-1 * warningDistance * this.transform.up) };
        proximityLine.SetPositions(linePoints);

        pointMarker.gameObject.SetActive(true);
        pointMarker.transform.localPosition = new Vector3(0, -distance*2, 0);
    }

    public void disableLine()
    {
        proximityLine.gameObject.SetActive(false);
    }

    void setLineColor(Color skimLineCurrentColor)
    {
        if (!isIdle && !lineNeedsToDie)
        {
            ////Debug.Log("NOT idle");//
            float percent = Mathf.Clamp01(1 - ((Time.time - fadeInTime) / fadeInDuration));
            triangleSprite.color = lerpFadeLineAlpha(skimLineCurrentColor, percent);
            setMarkAlpha(1 - percent);

            // Condition to escape idle state
            if (!isInputting)
                idleCounter += TimeKeeper.deltaPlayTime();
            if (idleCounter >= idleDuration)
            {
                isIdle = true;
            }

            fadeOutTime = TimeKeeper.getOverworld().getTime();
        }
        else // if idle
        {
            ////Debug.Log("IDLE");//

            float percent = Mathf.Clamp01((TimeKeeper.getOverworld().getTime() - fadeOutTime) / fadeAwayDuration);
            //Debug.Log(percent);
            triangleSprite.color = lerpFadeLineAlpha(skimLineCurrentColor, percent);
            setMarkAlpha(1 - percent);

            // Condition to enter idle state
            if (isInputting)
            {
                isIdle = false;
                idleCounter = 0;
            }

            fadeInTime = TimeKeeper.getOverworld().getTime();
        }
    }

    Color lerpFadeLineAlpha(Color color, float percent)
    {
        ////Debug.Log("percent: " + percent);//
        Color fadedColor = color;
        fadedColor.a = 0;

        Color lerpedColor = Color.Lerp(color, fadedColor, percent);

        proximityLine.startColor = lerpedColor;
        proximityLine.endColor = lerpedColor;

        return lerpedColor;
    }

    void setMarkAlpha(float percent)
    {
        pointMarker.color = new Color(pointMarker.color.r, pointMarker.color.g, pointMarker.color.b, percent);
        backgroundSprite.color = new Color(0, 0, 0, percent);
    }
    void setMarkScale(float percent)
    {
        pointMarker.transform.localScale = Vector3.Lerp(markerMinScale, markerMaxScale, 1 - percent);
    }

    public float getImpulse()
    {
        float result = 0;
        if (impulseAvailabile)
        {
            result = boostImpulseMag;
        }
        impulseAvailabile = false;
        ////Debug.Log(result);///
        return result;
    }
}
