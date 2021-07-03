using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    // Start is called before the first frame update
    public int baseHealth;
    public int currentHealth;
    public bool isAlive;
    public bool isInvulnerable;
    public HealthIconsManager iconDrawer;
    public Slider progressBar;
    public Slider bossLife;
    public AudioClip hurtNoise;

    public void Awake()
    {
        if (gameObject.layer == 9) //si soy el player
            currentHealth = PlayerStatsManager.currentLife;
        else
            currentHealth = baseHealth;

        isAlive = true;
        isInvulnerable = false;

        if (GameObject.FindGameObjectWithTag("ProgressBar"))
            progressBar = (Slider)FindObjectOfType(typeof(Slider));

    }

    private void Start()
    {
        if (iconDrawer != null)
            iconDrawer.UpdateIcons(currentHealth);
    }
    public void TakeDamage(int amount, bool ignoreInvulnerability = false, bool giveGold = true)
    {
        if (ignoreInvulnerability || (isAlive && amount > 0 && !isInvulnerable))
        {

            isInvulnerable = true;

            StartCoroutine(BlinkingBoat());

            currentHealth -= amount;

            if (gameObject.tag == "Boss")
                gameObject.SendMessage("AddProgress", amount);

            if (currentHealth <= 0)
                Die(giveGold);
            else
            {
                if (hurtNoise != null)
                    SoundHelper.PlaySound(hurtNoise, 0.3f);
            }
        }

        if (iconDrawer != null)
            iconDrawer.UpdateIcons(currentHealth);
    }

    private void Die(bool giveGold = true)
    {
        int progress = 0;
        isAlive = false;
        if (gameObject.layer == 10 && gameObject.tag != "Boss")//si es enemy
        {
            progress = this.GetComponent<Enemy>().experienceOtorged;
            progressBar.SendMessage("AddProgress", progress);
        }


        SendMessage("ApplyDeathEffects", giveGold);
    }
    IEnumerator BlinkingBoat()
    {
        int counter = 0;
        int blinkCount = 2;

        if (gameObject.layer == 9) //si es el player
            blinkCount = 6;

        while (counter < blinkCount)
        {
            yield return new WaitForSeconds(0.1f);

            Renderer boatRenderer = GetComponent<Renderer>();
            boatRenderer.enabled = !boatRenderer.enabled;
            counter++;
        }
        isInvulnerable = false;
        Renderer boatRenderer2 = GetComponent<Renderer>();
        boatRenderer2.enabled = true;
    }
}
