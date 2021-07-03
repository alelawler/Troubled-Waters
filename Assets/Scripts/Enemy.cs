using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float movementSpeed;
    public GameObject target;
    public Health targetHealth;
    public Health myHealth;
    public GameObject shootingPoint;
    public GameObject goldPrefab;
    public GameObject diamondPrefab;
    public Animator myAnimator;
    public int goldSpawned;
    public int experienceOtorged;
    public float diamondChance;

    public GameObject deathSpritePreFab;
    public GameObject wreckPrefab;

    // Update is called once per frame
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Target");
        targetHealth = target.GetComponent<Health>();
    }
    void Update()
    {
        if (Time.deltaTime > 0 && myHealth.isAlive && targetHealth.isAlive)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, movementSpeed * Time.deltaTime);
            SetMovingAnimation();
        }

    }

    private void SetMovingAnimation()
    {
        Vector3 direction = Vector3.Normalize(target.transform.position - transform.position);

        myAnimator.SetFloat("MoveX", direction.x);
        myAnimator.SetFloat("MoveY", direction.y);

    }
    public void ApplyDeathEffects(bool giveGold = true)
    {
        if (giveGold)
        {

            for (int i = 0; i < goldSpawned; i++)
            {
                Instantiate(goldPrefab, transform.position, Quaternion.identity);
            }
            float number = Random.Range(0.0f, 100.0f);
            if (number <= diamondChance)
            {
                Instantiate(diamondPrefab, transform.position, Quaternion.identity);

            }

        }

        if (deathSpritePreFab != null && wreckPrefab != null)
        {
            Instantiate(deathSpritePreFab, transform.position, Quaternion.identity);
            Instantiate(wreckPrefab, transform.position, Quaternion.identity);

        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Target") //Si choco con mi objetivo
        {
            Health targetHealth = collision.gameObject.GetComponent<Health>();
            targetHealth.TakeDamage(1);
            myHealth.TakeDamage(myHealth.baseHealth, true); // Al·lahu-àkbar
        }
    }
}
