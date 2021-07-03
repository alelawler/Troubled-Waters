using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineProjectile : MonoBehaviour
{

    public float speed;
    public float maxPosibleTimer;
    public float minPosibleTimer;
    public int damage;
    public GameObject attackPrefab;
    public GameObject explosionSign;
    public AudioClip shootNoise;
    public float shootClipVolume;

    private float timer;
    private GameObject sign;
    private void Awake()
    {
        timer = Random.Range(minPosibleTimer, maxPosibleTimer);

        Vector3 signPos = transform.position + transform.up * speed * -1 * timer;
        sign = Instantiate(explosionSign, signPos, Quaternion.identity);

        StartCoroutine(Timer(timer));
    }


    IEnumerator Timer(float timer)
    {
        int bulletNumber = 18;



        while (true)
        {
            yield return new WaitForSeconds(timer);

            for (int i = 0; i < bulletNumber; i++)
            {
                GameObject proyectile = Instantiate(attackPrefab, gameObject.transform.position, Quaternion.identity);
                proyectile.transform.right = new Vector3(1, 0);
                proyectile.transform.Rotate(0, 0, i * 360 / bulletNumber);

            }

            if (shootNoise != null)
                SoundHelper.PlaySound(shootNoise, shootClipVolume);
            // calcular proyectile.transform.right = target.position - gameObject.position;
            Destroy(sign.gameObject);
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Health hitHealth = collision.gameObject.GetComponent<Health>();


        hitHealth.TakeDamage(damage);
        Destroy(sign.gameObject);
        Destroy(this.gameObject);

    }

    private void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;

    }
}
