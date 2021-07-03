using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TavernTalkManager : MonoBehaviour
{
    public Image ChuckLaugh;
    public Image ChuckNormal;
    public Image ChuckScheming;
    public Image MCNormal;
    public Image MCSurprised;
    public Text TextChuckLaugh;
    public Text TextChuckNormal;
    public Text TextChuckScheming;
    public Text TextMCNormal;
    public Text TextMCSurprised;

    public Image MCTriangle;
    public Image ChuckTriangle;

    public AudioClip bipSound;

    public GameObject NextLevelButton;
    public GameObject goldButton;
    public GameObject lifeButton;
    public GameObject repairBoatGO;
    public Button repairBoatButton;
    public Text goldText;
    public Text lifeText;

    private Coroutine coroutine;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartTalk(SceneManager.GetActiveScene().name));
        UpdateTavernIcons();

    }

    public void UpdateTavernIcons()
    {
        goldText.text = PlayerStatsManager.totalGold.ToString();
        lifeText.text = PlayerStatsManager.currentLife.ToString();

        if (PlayerStatsManager.currentLife == 5 || PlayerStatsManager.totalGold < 300)
            repairBoatButton.interactable = false;
    }

    private IEnumerator StartTalk(string sceneName)
    {
        yield return new WaitForSeconds(1f);
        if (sceneName == "1-Tavern")
        {
            yield return ChuckNormalTalk("Welcome! I'm Chuck the Innkeeparr. What can I get you?");

            yield return MCNormalTalk("Just give me your strongest stuff. I don't have time, I need to find this treasure.");

            yield return ChuckSchemingTalk("Treasure, what treasure?");

            yield return MCNormalTalk("Found this old map in this really old bottle.");

            yield return ChuckNormalTalk("Oh.. arrgh you talking about THAT map?");

            yield return ChuckLaughingTalk("Good luck. You will need it");

        }
        else if (sceneName == "2-Tavern")
        {
            yield return MCNormalTalk("You again? How is that even posible?");

            yield return ChuckNormalTalk("I don't know what you arrgh talking about.");

            yield return MCNormalTalk("What can you tell me about this map? Everybody wants to kill me and it's getting dark.");

            yield return ChuckNormalTalk("Legend says is belonged to Willy, the insatiable.");

            yield return ChuckSchemingTalk("It must be his treasure.");

            yield return MCSurprisedTalk("Willy!");

            yield return MCNormalTalk("I will find Willy's treasure! I will be the richest pirate alive. NOBODY WILL STOP ME.");
        }
        else if (sceneName == "3-Tavern")
        {
            yield return MCNormalTalk("So Chuck, why there are so many pirates with Willy's flag? He dissapeared 10 years ago, isn't he dead?");

            yield return ChuckLaughingTalk("Oh boy, you dont know squat.");

            yield return ChuckNormalTalk("You will understand soon enough. Your time is ruming out.");

            yield return MCSurprisedTalk("Choose what? What time?");

            yield return MCNormalTalk("Answer me! You don't wanna make me angry Chuck");

            yield return ChuckNormalTalk("By the seaven Cs, Willy you old damn bastard.");

            yield return MCNormalTalk("...");
        }
        NextLevelButton.SetActive(true);
        goldButton.SetActive(true);
        lifeButton.SetActive(true);
        repairBoatGO.SetActive(true);
    }

    private IEnumerator ChuckNormalTalk(string text)
    {
        ChuckNormal.gameObject.SetActive(true);
        TextChuckNormal.text = text;
        coroutine = StartCoroutine(BlinkingArrow(ChuckTriangle));
        yield return new WaitForSeconds(0.3f);
        while (!Input.anyKeyDown)
            yield return null;
        if (bipSound != null)
            SoundHelper.PlaySound(bipSound, 1f);
        StopCoroutine(coroutine);
        ChuckTriangle.gameObject.SetActive(false);
        ChuckNormal.gameObject.SetActive(false);
    }
    private IEnumerator ChuckLaughingTalk(string text)
    {
        ChuckLaugh.gameObject.SetActive(true);
        TextChuckLaugh.text = text;
        coroutine = StartCoroutine(BlinkingArrow(ChuckTriangle));
        yield return new WaitForSeconds(0.3f);
        while (!Input.anyKeyDown)
            yield return null;
        if (bipSound != null)
            SoundHelper.PlaySound(bipSound, 1f); 
        StopCoroutine(coroutine);
        ChuckTriangle.gameObject.SetActive(false);
        ChuckLaugh.gameObject.SetActive(false);
    }
    private IEnumerator ChuckSchemingTalk(string text)
    {
        ChuckScheming.gameObject.SetActive(true);
        TextChuckScheming.text = text;
        coroutine = StartCoroutine(BlinkingArrow(ChuckTriangle));
        yield return new WaitForSeconds(0.3f);
        while (!Input.anyKeyDown)
            yield return null;
        if (bipSound != null)
            SoundHelper.PlaySound(bipSound, 1f);
        StopCoroutine(coroutine);
        ChuckTriangle.gameObject.SetActive(false);
        ChuckScheming.gameObject.SetActive(false);
    }
    private IEnumerator MCNormalTalk(string text)
    {
        MCNormal.gameObject.SetActive(true);
        TextMCNormal.text = text;
        coroutine = StartCoroutine(BlinkingArrow(MCTriangle));
        yield return new WaitForSeconds(0.3f);
        while (!Input.anyKeyDown)
            yield return null;
        if (bipSound != null)
            SoundHelper.PlaySound(bipSound, 1f);
        StopCoroutine(coroutine);
        MCTriangle.gameObject.SetActive(false);
        MCNormal.gameObject.SetActive(false);
    }
    private IEnumerator MCSurprisedTalk(string text)
    {
        MCSurprised.gameObject.SetActive(true);
        TextMCSurprised.text = text;
        coroutine = StartCoroutine(BlinkingArrow(MCTriangle));
        yield return new WaitForSeconds(0.3f);
        while (!Input.anyKeyDown)
            yield return null;
        if (bipSound != null)
            SoundHelper.PlaySound(bipSound, 1f);
        StopCoroutine(coroutine);
        MCTriangle.gameObject.SetActive(false);
        MCSurprised.gameObject.SetActive(false);
    }

    private IEnumerator BlinkingArrow(Image arrow)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.4f);
            arrow.gameObject.SetActive(!arrow.gameObject.activeSelf);
        }
    }

}
