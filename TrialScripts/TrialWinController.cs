using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.InputSystem;

public class TrialWinController : MonoBehaviour
{
    public Volume ppVolume;
    private ColorAdjustments colorAdjust;

    [Header("Intro")]
    public float exposureLerpTime;
    public AnimationCurve exposureCurve;
    public float maxExposure;

    [Header("Between")]
    public CanvasGroup backgroundGr;
    public CanvasGroup unionGlyphGr;
    public CanvasGroup unionNumGr;
    public TMP_Text unionNumText;
    public CanvasGroup glyphTotemGr;
    public Image totemBar;
    public CanvasGroup totemBarGr;
    public TMP_Text totemNumText;
    public CanvasGroup totemNumGr;
    public float barHeightIncreaseSeconds = 0.3f;
    public const float fadeTime = 1;
    public float numUpdateBounceAmount = 0.2f;
    public float numUpdateSeconds = 0.2f;
    [Space]
    public Gift[] giftCollections;
    public int numGifts;
    public Image background;
    public TMP_Text giftMessage1;
    public TMP_Text giftMessage2;
    public RectTransform giftMessageRect;
    public CanvasGroup giftMessageGr;
    public Vector2 giftMessageLowScale;
    public Vector2 giftMessageHighScale;
    public CanvasGroup progressGr;
    public Animator giftGainEndingAnimator;

    public static event Action onTrialNewlyWonEnd;


    public UnionScreen_IA Controls;
    public Status currentStatus = Status.STANDBY;
    public enum Status { STANDBY, INTRO, INTRO_COMPLETE, GIFTS, GIFTS_COMPLETE, END };
    bool hasWonNewGift = false;
    int newGiftID = 0;
    LTSeq ltSequence;

    static TrialWinController instance;

    [System.Serializable]
    public struct Gift
    {
        public CanvasGroup glyphOverlay;
        public ParticleSystem affirmParticles;
        public ParticleSystem winParticles;
        public string message1;
        public string message2;
    }

    public void Start()
    {
        instance = this;

        ppVolume.profile.TryGet<ColorAdjustments>(out colorAdjust);

        //TrialWinState.onTrialNewlyWon += begin;

        Controls = new UnionScreen_IA();
        Controls.Tap.Next.started += inputNext;
    }

    public void OnDestroy()
    {
        //TrialWinState.onTrialNewlyWon -= begin;

        Controls.Tap.Next.started -= inputNext;
    }

    void inputNext(InputAction.CallbackContext context)
    {
        if (currentStatus == Status.INTRO_COMPLETE)
        {
            gifts();
        }
        else if (currentStatus == Status.GIFTS_COMPLETE)
        {
            commenceEnding();
        }
    }

    public static void begin()
    {
        instance.begin_dynamic();
    }
    public void begin_dynamic()
    {
        resetScreen();
        giftGainEndingAnimator.SetTrigger("intro");
        currentStatus = Status.INTRO;
        intro();
    }

    public void resetScreen()
    {
        backgroundGr.alpha = 0;
        unionGlyphGr.alpha = 0;
        unionNumGr.alpha = 0;
        glyphTotemGr.alpha = 0;
        totemBarGr.alpha = 1;
        totemNumGr.alpha = 0;
        giftMessageGr.alpha = 0;
        progressGr.alpha = 1;
        totemBar.fillAmount = 0;
        background.color = Color.white;
        hasWonNewGift = false;
        //endingAnimator.enabled = false;
        for (int i = 0; i < numGifts; i++)
        {
            giftCollections[i].glyphOverlay.alpha = 0;
        }
    }

    // INTRO //
    // Starts the union text number to the previous score the player had, before this win.
    // Adds tweens to the sequence thatramps up the exposure and fades to white. Then, fades the union glyph and the text number.
    // Finally after that, it completes the intro.
    void intro()
    {
        unionNumText.text = Convert.ToString(SaveDataManager.trials.numWon - 1);

        ltSequence = LeanTween.sequence();
        ltSequence
            .append(LeanTween.value(this.gameObject, setExposure, 0, maxExposure, exposureLerpTime))
            .append(getCanvasTween(true, backgroundGr))
            .append(getCanvasTween(true, unionGlyphGr))
            .append(getCanvasTween(true, unionNumGr).setOnComplete(completeIntro));
    }
    public void fadeCanvasGroup(float step, params CanvasGroup[] cg)
    {
        for (int i = 0; i < cg.Length; i++)
            cg[i].alpha = step;
    }
    void setExposure(float f)
    {
        colorAdjust.postExposure.value = f;
    }

    // Updates the union text number for the intro. Sets it to the next number and makes it jump.
    // Then, we say we're at the next state, and allow the player to enter input.
    void completeIntro()
    {
        unionNumText.text = Convert.ToString(SaveDataManager.trials.numWon);
        bounceGameObject(unionNumText.gameObject);
        currentStatus = Status.INTRO_COMPLETE;
        Controls.Enable();
    }

    // GIFTS TOTEM //
    // Begins the gift state.
    // Makes sure that the totem text starts at 0, as it will increment up.
    // Then, we add tweens to the sequence that:
    // - Fades out the elements that are already on screen.
    // - Fades in the elements we need now (the totem and its text mesh below it.)
    // - Increment the totem.
    // - Show what gifts have been one.
    void gifts()
    {
        currentStatus = Status.GIFTS;
        totemNumText.text = Convert.ToString(0);

        ltSequence = LeanTween.sequence();
        ltSequence
            .append(getCanvasTween(false, unionGlyphGr, unionNumGr))
            .append(getCanvasTween(true, glyphTotemGr))
            .append(getCanvasTween(true, totemNumGr));
        tweenTotem();
        showTotemGifts();
        ltSequence.append(LeanTween.delayedCall(0, () => currentStatus = Status.GIFTS_COMPLETE));
    }

    // Adds to LeanTween Sequence (ltSequence) the tweens that
    // make the bar fill up and the number below it increment.
    void tweenTotem()
    {
        float barUnit = 1f / SaveDataManager.trials.getNum();
        float barInscreaseSecondsPerUnit = barHeightIncreaseSeconds / Mathf.Clamp((SaveDataManager.trials.numWon / 5), 1, 5);

        for (int i = 1; i <= SaveDataManager.trials.numWon; i++)
        {
            incrementTextAndBar(i, barUnit);
        }

        void incrementTextAndBar(int i, float barUnit)
        {
            ltSequence.append(LeanTween.value(totemBar.gameObject, setBarProgress, (i - 1) * barUnit, i * barUnit, barInscreaseSecondsPerUnit))
                .insert(bounceGameObject(totemNumText.gameObject).setOnStart(() => totemNumText.text = Convert.ToString(i)));
        }

        void setBarProgress(float amount)
        {
            totemBar.fillAmount = amount;
        }
    }

    // Adds to LeanTween Sequence (ltSequence) the tweens that
    // make the necessary glyphs light up (the gifts that have been unlocked, the next one, and the one that the player has just won).
    void showTotemGifts()
    {
        float unionProgress = ((float)SaveDataManager.trials.numWon) / SaveDataManager.trials.getNum();
        for (int i = 1; i <= numGifts; i++)
        {
            ////Debug.Log(unionProgress + " >= " + i + "/" + numGifts + " is: ");
            if (unionProgress >= ((float)i) / numGifts)
            {
                ////Debug.Log("True");
                if (((float)(SaveDataManager.trials.numWon - 1)) / SaveDataManager.trials.getNum() < ((float)i) / numGifts) // This was just enough to earn this gift. That means the player is currently unlocking it.
                {
                    showNextGlyph(i-1);
                    hasWonNewGift = true;
                    newGiftID = i - 1;
                    //unlockGlyph(i-1);
                    break;
                }
                else
                {
                    alightPreviouslyEarnedGift(i-1);
                }
            }
            else  // This glyph is not yet won. Have it blink to let the player know it is next.
            {
                ////Debug.Log("false");
                showNextGlyph(i - 1);
                break;
            }
        }
    }

    void alightPreviouslyEarnedGift(int glyph)
    {
        ltSequence.append(getCanvasTween(true, giftCollections[glyph].glyphOverlay)
                                .setOnStart(() => playAffirmationParticles(glyph)));
    }

    // ENDING //
    void commenceEnding()
    {
        setExposure(0);
        currentStatus = Status.END;
        if (hasWonNewGift)
        {
            unlockGlyph(newGiftID);
        }
        else
        {
            trialWinEnd();
            getCanvasTween(false, backgroundGr);//
        }
    }

    // Adds tweens to sequence that:
    // Fades in winning gift glyph and plays its winning particles
    // Turns the background from white to black
    // Then fades the whole totem screen out.
    // After that, it adds tweens for the next phase of the sequence - the gift message screen. These include:
    // Setting up the proper gift message text animating it a bit.
    void unlockGlyph(int glyph)
    {
        ltSequence = LeanTween.sequence();

        ltSequence
            //.append(LeanTween.delayedCall(2, () => getCanvasTween(true, giftCollections[glyph].glyphOverlay)
            //                    .setOnStart(() => playWinningParticles(glyph))))
            .append(getCanvasTween(true, giftCollections[glyph].glyphOverlay)
                   .setOnStart(() => playWinningParticles(glyph)))
            .append((LeanTween.delayedCall(2, () => LeanTween.value(background.gameObject, c => background.color = c, Color.white, Color.black, 5))))
            .append(getCanvasTween(false, progressGr).setOnComplete(playEnding));

        setMessages(glyph);
    }
    void playEnding()
    {
        //endingAnimator.enabled = true;
        setExposure(0);
        giftGainEndingAnimator.SetTrigger("play");
    }
    void showNextGlyph(int glyph)
    {
        ltSequence.append(getCanvasTween(true, 0.1f, giftCollections[glyph].glyphOverlay))
                  .append(getCanvasTween(false, 0.1f, giftCollections[glyph].glyphOverlay))
                  .append(getCanvasTween(true, 0.1f, giftCollections[glyph].glyphOverlay))
                  .append(getCanvasTween(false, 0.1f, giftCollections[glyph].glyphOverlay));
    }

    public void playAffirmationParticles(int glyph)
    {
        giftCollections[glyph].affirmParticles.Play();
    }
    public void playWinningParticles(int glyph)
    {
        giftCollections[glyph].winParticles.Play();
    }

   
    void setMessages(int glyph)
    {
        giftMessage1.text = giftCollections[glyph].message1;
        giftMessage2.text = giftCollections[glyph].message2;
    }

    // Returns a tween that slowly fades the given canvas group(s) in/out.
    LTDescr getCanvasTween(bool fadeIn, params CanvasGroup[] canvases)
    {
        return getCanvasTween(fadeIn, fadeTime, canvases);
    }
    LTDescr getCanvasTween(bool fadeIn, float duration, params CanvasGroup[] canvases)
    {
        int beginning = 1, end = 0;
        if (fadeIn)
        {
            beginning = 0;
            end = 1;
        }
        return LeanTween.value(this.gameObject, f => fadeCanvasGroup(f, canvases), beginning, end, duration);
    }

    LTDescr bounceGameObject(GameObject g)
    {
        return LeanTween.moveLocalY(g, g.transform.localPosition.y + numUpdateBounceAmount, numUpdateSeconds).setEasePunch();
    }

    public void trialWinEnd()
    {
        //Debug.Log("trial win end");//
        giftGainEndingAnimator.SetTrigger("done");
        onTrialNewlyWonEnd?.Invoke();
    }
}