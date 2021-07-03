using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float speed;
    public float lifeTime = 3;
    public int damage;
    private float timer;
    public GameObject target;

    private void Awake()
    {
        StartCoroutine(Timer());

    }


    IEnumerator Timer()
    {
        while (true)
        {

            yield return new WaitForSeconds(lifeTime);

            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != 18)
        {
            Health hitHealth = collision.gameObject.GetComponent<Health>();

            hitHealth.TakeDamage(damage);

            Destroy(this.gameObject);
        }
        else
        {
            Obstacle hitHealth = collision.gameObject.GetComponent<Obstacle>();

            hitHealth.TakeDamage(damage);

            Destroy(this.gameObject);
        }


    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (this.gameObject.layer == 12 && timer <= 0.6 && target != null)
        { //Bala player, seguir un poco al enemigo
            transform.right = target.transform.position - transform.position;
        }
        transform.position += transform.right * speed * Time.deltaTime;


    }

}
