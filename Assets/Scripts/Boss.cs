using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public float travelTime;
    public GameObject spawnPoint;
    public GameObject pointA;
    public GameObject pointB;
    public bool goignToPointB;
    public GameObject target;
    public Health myHealth;
    public float attackCD;
    public int phase2TriggerPCT;
    public int phase3TriggerPCT;
    public GameObject[] attackPoints;
    public GameObject attackPrefab;
    public GameObject mineAttackPrefab;
    public bool isIntro;
    public GameObject bossBattleFlash;
    public GameObject bossBarLifeContainer;
    public AudioClip shootNoise;
    public AudioClip mineNoise;
    public AudioClip changePhaseNoise;
    public float shootClipVolume;
    public ParticleSystem frontWave;
    public ParticleSystemRenderer frontWaveRenderer;

    private int phase;
    private int currentphase;
    private float acumulatedTravelTime;
    private int attackPattern;
    private bool attack2random;
    private GameObject myPivot;
    private Vector3 auxScale;
    private Slider bossLife;
    private void Awake()
    {
        pointA = GameObject.Find("BossPointA");
        pointB = GameObject.Find("BossPointB");
        spawnPoint = GameObject.Find("BossSpawnPoint");
        bossBattleFlash = GameObject.Find("BossBattleFlash");
        bossBarLifeContainer = GameObject.Find("BossLifeContainer");
        target = GameObject.FindGameObjectWithTag("Target");


        phase = 1;
        attack2random = false;
        acumulatedTravelTime = travelTime / 2;
        goignToPointB = true;
        isIntro = true;
    }

    IEnumerator AttackPhase1()
    {

        while (Time.deltaTime > 0 && myHealth.isAlive)
        {
            yield return new WaitForSeconds(attackCD);

            attackPattern = UnityEngine.Random.Range(1, 3);

            if (attackPattern == 1)
                yield return Attack1(attackPoints);
            else //if (attackPattern == 2)
                yield return Attack2(attackPoints, attack2random);


        }
    }
    IEnumerator AttackPhase2()
    {
        while (Time.deltaTime > 0 && myHealth.isAlive)
        {
            yield return new WaitForSeconds(attackCD * 3);

            if (mineNoise != null)
                SoundHelper.PlaySound(mineNoise, shootClipVolume);

            GameObject proyectile = Instantiate(mineAttackPrefab, gameObject.transform.position, Quaternion.identity);
            proyectile.transform.up = new Vector3(0, -1);
        }
    }
    IEnumerator AttackPhase3()
    {

        while (Time.deltaTime > 0 && myHealth.isAlive)
        {
            yield return new WaitForSeconds(attackCD * 3);
            yield return Attack3(this.gameObject);

        }
    }

    IEnumerator Attack1(GameObject[] attackPoints)
    {
        int i = 0;
        if (target != null && target.GetComponent<Health>().isAlive)
        {
            for (i = 0; i < attackPoints.Length; i++)
            {
                GameObject proyectile = Instantiate(attackPrefab, attackPoints[i].transform.position, Quaternion.identity);
                proyectile.transform.right = target.transform.position - attackPoints[i].transform.position;

                if (shootNoise != null)
                    SoundHelper.PlaySound(shootNoise, shootClipVolume);

                yield return new WaitForSeconds(0.1f);
            }

            for (i = attackPoints.Length - 1; i >= 0; i--)
            {
                GameObject proyectile = Instantiate(attackPrefab, attackPoints[i].transform.position, Quaternion.identity);
                proyectile.transform.right = target.transform.position - attackPoints[i].transform.position;

                if (shootNoise != null)
                    SoundHelper.PlaySound(shootNoise, shootClipVolume);

                yield return new WaitForSeconds(0.1f);
            }
            // se comenta porque parece dificil
            //for (i = 0; i < attackPoints.Length; i++)
            //{
            //    GameObject proyectile = Instantiate(attackPrefab, attackPoints[i].transform.position, Quaternion.identity);
            //    proyectile.transform.right = target.transform.position - attackPoints[i].transform.position;

            //    yield return new WaitForSeconds(0.1f);
            //}
        }
        else
            Debug.Log("era null");
    }
    IEnumerator Attack2(GameObject[] attackPoints, bool trueRandom = false)
    {
        //true random le da un angulo diferente a CADA bala, no al set de balas
        int i = 0;
        int bullets = 0;
        int angle = 0;
        if (target != null && target.GetComponent<Health>().isAlive)
        {
            for (bullets = 0; bullets < 3; bullets++)
            {
                if (!trueRandom)
                    angle = UnityEngine.Random.Range(0, 11);
                for (i = 0; i < attackPoints.Length; i++)
                {
                    if (trueRandom)
                        angle = UnityEngine.Random.Range(0, 41);
                    GameObject proyectile = Instantiate(attackPrefab, attackPoints[i].transform.position, Quaternion.identity);
                    proyectile.transform.right = target.transform.position - attackPoints[i].transform.position;

                    proyectile.transform.Rotate(0, 0, angle);
                }

                if (shootNoise != null)
                    SoundHelper.PlaySound(shootNoise, shootClipVolume);

                yield return new WaitForSeconds(0.3f);

            }
        }
        else
            Debug.Log("era null");
    }
    IEnumerator Attack3(GameObject attackPoint)
    {
        int i = 0;

        for (i = 0; i < 72; i++)
        {
            GameObject proyectile = Instantiate(attackPrefab, gameObject.transform.position, Quaternion.identity);
            proyectile.transform.right = new Vector3(1, 0);
            proyectile.transform.Rotate(0, 0, 10 * i);

            if (shootNoise != null)
                SoundHelper.PlaySound(shootNoise, shootClipVolume);

            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator FlashScreen()
    {

        int counter = 0;
        int blinkCount = 2;

        while (counter < blinkCount)
        {
            yield return new WaitForSeconds(0.1f);

            bossBattleFlash.GetComponent<Image>().enabled = !bossBattleFlash.GetComponent<Image>().enabled;
            counter++;
        }
    }


    void Update()
    {
        if (myHealth.isAlive && !isIntro)
        {

            if (goignToPointB)
            {
                auxScale = transform.localScale;
                auxScale.x = Mathf.Abs(auxScale.x);
                transform.localScale = auxScale;
                frontWaveRenderer.flip = new Vector3(0, 0, 0);
                acumulatedTravelTime += Time.deltaTime;
                if (acumulatedTravelTime >= travelTime)
                    goignToPointB = false;
            }
            else
            {

                auxScale = transform.localScale;
                auxScale.x = -1 * Mathf.Abs(auxScale.x);
                transform.localScale = auxScale;
                frontWaveRenderer.flip = new Vector3(1, 0, 0);

                acumulatedTravelTime -= Time.deltaTime;
                if (acumulatedTravelTime <= 0f)
                    goignToPointB = true;
            }
            transform.position = Vector3.Slerp(pointA.transform.position, pointB.transform.position, acumulatedTravelTime / travelTime);

            phase = CheckPhase(phase);
        }
        else
        {
            if (myHealth.isAlive)
            {
                transform.position = Vector3.MoveTowards(this.transform.position, spawnPoint.transform.position, 2f * Time.deltaTime);

            }
        }
    }
    public void ApplyDeathEffects()
    {
        StopAllCoroutines();
        myHealth.isAlive = false;
        target.SendMessage("Win");
    }

    private int CheckPhase(int phase)
    {

        if (phase == 1 && (float)myHealth.currentHealth / (float)myHealth.baseHealth * 100 <= phase2TriggerPCT)
        { //phase 2
            if (changePhaseNoise != null)
                SoundHelper.PlaySound(changePhaseNoise, shootClipVolume);

            travelTime *= 0.9f; //aumento un 10% la velocidad
            acumulatedTravelTime *= 0.9f;

            ScaleBoss(10f, 2);

            attack2random = false;
            StartCoroutine(FlashScreen());
            StartCoroutine(AttackPhase2());
            return 2;
        }
        else if (phase == 2 && (float)myHealth.currentHealth / (float)myHealth.baseHealth * 100 <= phase3TriggerPCT) //phase 3
        { //phase 3

            if (changePhaseNoise != null)
                SoundHelper.PlaySound(changePhaseNoise, shootClipVolume);

            travelTime *= 0.9f; //aumento un 10% la velocidad
            acumulatedTravelTime *= 0.9f;

            ScaleBoss(15f, 3);
            StartCoroutine(FlashScreen());
            StartCoroutine(AttackPhase3());
            return 3;

        }
        else
            return phase;
    }

    private void ScaleBoss(float amountToScalePCT, int phase)
    {
        float multiplier = (1 + amountToScalePCT / 100);
        auxScale = transform.localScale;
        auxScale.x = auxScale.x * multiplier;
        auxScale.y = auxScale.y * multiplier;
        auxScale.z = auxScale.z * multiplier;
        transform.localScale = auxScale;

        var main = frontWave.main;
        var color = frontWave.colorOverLifetime;

        if (phase == 2)
        {
            main.startSizeMultiplier *= multiplier;
            main.startLifetimeMultiplier *= multiplier;
            
            color.enabled = true;

            Gradient grad = new Gradient();
            grad.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(new Color(255, 0, 77), 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.7f, 1.0f) });

            color.color = grad;
        }
        else if (phase == 3)
        {
            main.startSizeMultiplier *= multiplier;
            main.startLifetimeMultiplier *= multiplier;
            
            color.enabled = true;

            Gradient grad = new Gradient();
            grad.SetKeys(new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.red, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) });

            color.color = grad;
        }

    }

    public void StartAttack()
    {
        GameObject bossLifeGO = bossBarLifeContainer.transform.GetChild(0).gameObject;
        bossLifeGO.SetActive(true);
        bossLife = bossLifeGO.GetComponent<Slider>();
        bossLife.maxValue = myHealth.baseHealth;

        StartCoroutine(AttackPhase1());
    }

    public void AddProgress(int progress)
    {
        bossLife.value += progress;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Target") //Si choco con mi objetivo
        {
            Health targetHealth = collision.gameObject.GetComponent<Health>();
            targetHealth.TakeDamage(1);
        }
    }

}
