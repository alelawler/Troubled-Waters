using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public float spawnCD;
    public List<GameObject> spawnPoints;
    public GameObject player;
    public bool spawnEnemies;
    public Slider lvlProgress;
    public GameObject radarPrefab;


    private int enemyIndex;
    [System.Serializable]
    public class enemiesDictionary
    {
        public GameObject preFab;
        public int count;
    }

    public List<enemiesDictionary> enemiesList;

    private void Awake()
    {
        spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint").ToList<GameObject>();
        player = GameObject.FindGameObjectWithTag("Target");
        spawnEnemies = true;

        if (GameObject.FindGameObjectWithTag("ProgressBar"))
            lvlProgress = (Slider)FindObjectOfType(typeof(Slider));

        StartCoroutine(SpawnEnemy());

    }


    IEnumerator SpawnEnemy()
    {
        int lvlProgressPercentage = 0;

        while (Time.deltaTime > 0 && spawnEnemies)
        {
            yield return new WaitForSeconds(spawnCD);

            enemiesList.RemoveAll(o => o.count == 0);
            if (enemiesList.Count > 0 && GameObject.FindGameObjectsWithTag("Enemy").Length <= 7)
            {
                lvlProgressPercentage = Mathf.RoundToInt(lvlProgress.value / lvlProgress.maxValue * 100);
                if (lvlProgressPercentage < 40)
                    enemyIndex = 0;
                else if (lvlProgressPercentage < 80)
                    enemyIndex = UnityEngine.Random.Range(0, 2);
                else
                    enemyIndex = UnityEngine.Random.Range(0, enemiesList.Count);

                enemiesDictionary enemyData = enemiesList[enemyIndex];

                Vector3 iniPos = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)].transform.position;
                iniPos.z = 0;
                GameObject enemy = Instantiate(enemyData.preFab, iniPos, Quaternion.identity);

                GameObject radar = Instantiate(radarPrefab, this.transform.parent.position - new Vector3(0f, 0.712f, 0f), Quaternion.identity);
                radar.GetComponent<RadarManager>().target = enemy;
                radar.transform.parent = this.transform.parent;
                enemiesList[enemyIndex].count--;


            }
        }
    }
    public void StopSpawning()
    {
        StopAllCoroutines();
        spawnEnemies = false;

    }
    private void Update()
    {
        if (!spawnEnemies)
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                    Destroy(enemy);

                switch (SceneManager.GetActiveScene().name)
                {
                    case "Level4Screen":
                        player.SendMessage("StartBossBattle");
                        break;
                    default:
                        player.SendMessage("EndLevel", SceneManager.GetActiveScene().name);
                        break;
                }


                lvlProgress.gameObject.SetActive(false);
                this.gameObject.SetActive(false);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (spawnPoints.Contains(collision.gameObject))
        {
            spawnPoints.Remove(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!spawnPoints.Contains(collision.gameObject))
        {
            spawnPoints.Add(collision.gameObject);
        }
    }

}
