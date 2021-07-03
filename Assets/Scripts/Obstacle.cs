using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public int baseHealth;
    public int currentHealth;
    public AudioClip hitNoise;

    private Renderer myRenderer;
    private void Awake()
    {
        currentHealth = baseHealth;
        myRenderer = GetComponent<Renderer>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (myRenderer.isVisible && (collision.gameObject.layer == 9 || collision.gameObject.layer == 10))
        {
            if (gameObject.layer != 19) // si no es un twister
            {
                collision.gameObject.GetComponent<Health>().TakeDamage(1);
                if (hitNoise != null)
                    SoundHelper.PlaySound(hitNoise, 0.6f);

                Die();
            }
            else
            {
                if (collision.gameObject.layer == 10 && collision.gameObject.tag != "Boss") //enemy = instadeath no gold
                {
                    if (hitNoise != null)
                        SoundHelper.PlaySound(hitNoise, 0.3f);
                    collision.gameObject.GetComponent<Health>().TakeDamage(collision.gameObject.GetComponent<Health>().currentHealth, true, false);
                }
                else //player, 1 damage
                    collision.gameObject.GetComponent<Health>().TakeDamage(1);
                TakeDamage(1);

            }
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (hitNoise != null)
            SoundHelper.PlaySound(hitNoise, 0.6f);
        if (currentHealth <= 0)
        {
            Die();
        }
    }
}
