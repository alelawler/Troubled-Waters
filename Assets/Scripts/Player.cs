using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Cinemachine;

public class Player : MonoBehaviour
{
    public int movementSpeed;
    public bool isFacingRight;
    public Health myHealth;
    public int totalGold;
    public Text playerLife;
    public Text gold;
    public Animator myAnimator;
    public GameObject deathScreen;
    public GameObject winScreen;
    public GameObject myCamera;
    public GameObject bossBattlePoint;
    public GameObject bossConfiner;
    public GameObject[] bossBorders;
    public GameObject bossPrefab;
    public GameObject bossSpawnPoint;
    public AudioSource lvlScreenAudioMusic;
    public AudioClip dangerAlarm;
    public AudioClip bossBattle;
    public GameObject bossInstance;
    public GameObject chatBoxes;
    public bool isIntro;
    public GameObject playerSpawnPoint;
    public GameObject areaOfAttack;
    public CircleCollider2D myAttackRange;
    public Image bossRedAlert;
    public bool goingToZeroAlpha;
    public SceneLoader sceneLoader;

    private void Awake()
    {
        transform.position = playerSpawnPoint.transform.position;
        Vector3 auxScale = areaOfAttack.transform.localScale;
        auxScale.x = myAttackRange.radius;
        auxScale.y = myAttackRange.radius;
        auxScale.z = myAttackRange.radius;
        areaOfAttack.transform.localScale = auxScale;
        totalGold = PlayerStatsManager.totalGold;
    }


    private void StartBossBattle()
    {
        //Destruyo clones porque aparecian en la bossfigth
        GameObject[] clones = GameObject.FindGameObjectsWithTag("Clone");
        if (clones.Length != 0)
            foreach (GameObject clone in clones)
                Destroy(clone);

        isIntro = true;
        transform.position = bossBattlePoint.transform.position;
        myCamera.GetComponent<CinemachineConfiner>().m_BoundingShape2D = bossConfiner.GetComponent<PolygonCollider2D>();
        myCamera.transform.position = bossBattlePoint.transform.position;
        myCamera.GetComponent<CinemachineVirtualCamera>().m_Lens.NearClipPlane = 0;
        myCamera.GetComponent<CinemachineVirtualCamera>().Follow = null;
        myCamera.GetComponent<CinemachineConfiner>().InvalidatePathCache();

        gold.transform.parent.gameObject.SetActive(false);

        foreach (GameObject border in bossBorders)
            border.SetActive(true);
        StartCoroutine(RedAlertBoss());
        StartFight();


    }

    private void EndLevel(string lvlName)
    {
        myHealth.isInvulnerable = true;
        string nivel = lvlName.Substring(5, 1) + "-Tavern";

        //Busco todos los oros en el mapa y los agarro inmediatamente
        GameObject[] golds = GameObject.FindGameObjectsWithTag("Gold");
        if (golds.Length != 0)
            foreach (GameObject gold in golds)
            {
                if (gold.layer == 15)
                    totalGold += Convert.ToInt32(Mathf.Round(10 * gold.gameObject.transform.localScale.x));
                else if (gold.layer == 22)
                    totalGold += 40;

                Destroy(gold);
            }

        //Guardo el oro en el general
        PlayerStatsManager.totalGold = totalGold;
        PlayerStatsManager.currentLife = myHealth.currentHealth;
        sceneLoader.LoadScene(nivel);
    }



    private void StartFight()
    {
        bossInstance = Instantiate(bossPrefab, bossSpawnPoint.transform.position + new Vector3(-5, 3, 0), Quaternion.identity);
        bossInstance.GetComponent<BoxCollider2D>().enabled = false;
    }

    private IEnumerator RedAlertBoss()
    {
        lvlScreenAudioMusic.clip = dangerAlarm;
        lvlScreenAudioMusic.Play();
        bossRedAlert.gameObject.SetActive(true);
        float timer = 0;

        while (timer <= 7f)
        {
            yield return new WaitForSeconds(0.01f);

            Color tempColor = bossRedAlert.color;


            if (goingToZeroAlpha)
            {
                tempColor.a -= 0.05f;
                if (tempColor.a <= 0f)
                {
                    goingToZeroAlpha = false;
                }
            }
            else
            {
                tempColor.a += 0.5f;
                if (tempColor.a >= 0.6f)
                {
                    goingToZeroAlpha = true;
                }
            }
            bossRedAlert.color = tempColor;
            timer += Time.deltaTime;
        }
        bossRedAlert.gameObject.SetActive(false);

        lvlScreenAudioMusic.Stop();
        StartCoroutine(PlayBGMusicAfterSeconds(2f, bossBattle));


        yield return IntroBattleTalk();

        bossInstance.GetComponent<Boss>().isIntro = false;
        bossInstance.GetComponent<BoxCollider2D>().enabled = true;
        bossInstance.GetComponent<Boss>().SendMessage("StartAttack");
        isIntro = false;


    }

    private IEnumerator PlayBGMusicAfterSeconds(float seconds, AudioClip backgroundMusic)
    {
        yield return new WaitForSeconds(seconds);
        lvlScreenAudioMusic.clip = backgroundMusic;
        lvlScreenAudioMusic.volume = 0.2f;
        lvlScreenAudioMusic.loop = true;
        lvlScreenAudioMusic.Play();
    }

    private IEnumerator IntroBattleTalk()
    {
        chatBoxes.transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        chatBoxes.transform.GetChild(0).gameObject.SetActive(false);
        chatBoxes.transform.GetChild(1).gameObject.SetActive(true);

        yield return new WaitForSeconds(3);
        chatBoxes.transform.GetChild(1).gameObject.SetActive(false);
    }

    private void Move()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        myAnimator.SetFloat("MoveX", horizontalInput);
        myAnimator.SetFloat("MoveY", verticalInput);

        if ((verticalInput != 0f || horizontalInput != 0f) && !isIntro)
        {
            myAnimator.SetBool("isMoving", true);

            Vector3 direction = new Vector3(horizontalInput, verticalInput).normalized;
            transform.position = transform.position + (direction * movementSpeed * Time.deltaTime);

        }
        else
            myAnimator.SetBool("isMoving", false);

    }

    private void Start()
    {
        myHealth.TakeDamage(0); //Dibujo los iconos
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.deltaTime > 0 && myHealth.isAlive)
        {
            Move();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetKey(KeyCode.Tab) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.LeftAlt))
            areaOfAttack.SetActive(true);
        else
            areaOfAttack.SetActive(false);

        gold.text = totalGold.ToString();
    }

    private void Win()
    {

        Time.timeScale = 0;
        winScreen.SetActive(true);
    }

    public void ApplyDeathEffects()
    {
        Time.timeScale = 0;
        myHealth.isAlive = false;
        deathScreen.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //si colisiono con el oro
        if (collision.gameObject.layer == 15)
        {
            //depende el tamaño del oro doy mas o menos oro
            totalGold += Convert.ToInt32(Mathf.Round(10 * collision.gameObject.transform.localScale.x));
            Destroy(collision.gameObject);
        }
        //Es un diamante
        else if (collision.gameObject.layer == 22)
        {
            totalGold +=  40;
            Destroy(collision.gameObject);
        }

    }
}
