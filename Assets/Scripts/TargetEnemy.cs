using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetEnemy : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Transform> enemiesInRange;
    public float attackCD;
    public Transform attackPoint;
    public GameObject attackPrefab;
    public int ammoCount;
    public int gunCount;
    public int angleSpread;
    public AudioClip shootNoise;
    public float shootClipVolume;
    public Health parentHealth;
    public Health targetHealth;

    private void Awake()
    {
        enemiesInRange = new List<Transform>();
        parentHealth = GetComponentInParent<Health>();
        StartCoroutine(Shoot());

    }

    IEnumerator Shoot()
    {


        int i;
        float minDistance;
        float auxDistance;
        int enemyIndex = 0;

        while (Time.deltaTime > 0 && parentHealth.isAlive)
        {
            yield return new WaitForSeconds(attackCD);
            minDistance = float.MaxValue;

            for (i = 0; i < enemiesInRange.Count; i++)
            {
                auxDistance = Vector3.Distance(transform.position, enemiesInRange[i].position);
                if (minDistance > auxDistance)
                {
                    minDistance = auxDistance;
                    enemyIndex = i;
                }

            }


            if (enemiesInRange.Count > 0 && ammoCount > 0)
            {

                //Modo viejo
                //if (ammoCount == 1)
                //{
                //    myAnimator.SetBool("isShooting", true);
                //    GameObject proyectile = Instantiate(attackPrefab, attackPoint.position, Quaternion.identity);
                //    proyectile.transform.right = enemiesInRange[enemyIndex].position - attackPoint.position;
                //}
                //else if (ammoCount > 1)
                //{
                //    myAnimator.SetBool("isShooting", true);
                //    StartCoroutine(MultiShoot(enemiesInRange[enemyIndex].position, ammoCount));
                //}


                StartCoroutine(MultiShoot(enemiesInRange[enemyIndex], ammoCount, gunCount));
            }

        }
    }

    IEnumerator MultiShoot(Transform target, int ammoCount, int gunCount)
    {
        int i; //ammoCounter
        int j; //gunCounter
        int finalAngle; //final angle for spread

        for (i = 0; i < ammoCount; i++)
        {


            finalAngle = 0; //reincio el spread

            if (target != null && target.GetComponent<Health>().isAlive)
            {
                if (shootNoise != null)
                    SoundHelper.PlaySound(shootNoise, shootClipVolume);

                GameObject proyectile = Instantiate(attackPrefab, attackPoint.position, Quaternion.identity);
                if (this.gameObject.layer == 11) //player sensor
                    proyectile.GetComponent<Projectile>().target = target.gameObject;


                proyectile.transform.right = target.position - attackPoint.position;

                for (j = 1; j < gunCount; j++) //arranca en 1 porque el primer dispario es siempre en el attackPoint
                {
                    GameObject proyectileSpread = Instantiate(attackPrefab, attackPoint.position, Quaternion.identity);
                    proyectileSpread.transform.right = target.position - attackPoint.position;

                    if (j % 2 == 1) //es impar
                        finalAngle = Mathf.Abs(finalAngle) + angleSpread;
                    else
                        finalAngle = finalAngle * -1;

                    proyectileSpread.transform.Rotate(0, 0, finalAngle);

                    if (this.gameObject.layer == 11) //player sensor
                        proyectileSpread.GetComponent<Projectile>().target = target.parent.gameObject;
                }
                yield return new WaitForSeconds(0.1f);

            }
            else
                Debug.Log("era null");
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enemiesInRange.Contains(collision.transform))
        {
            enemiesInRange.Add(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (enemiesInRange.Contains(collision.transform))
        {
            enemiesInRange.Remove(collision.transform);
        }
    }

}
