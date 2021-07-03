using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarManager : MonoBehaviour
{
    public GameObject target;
    public float destroyTimer;
    private SpriteRenderer targetRenderer;

    private float timer;
    private void Start()
    {
        timer = 0;
        targetRenderer = target.GetComponent<SpriteRenderer>();
    }
    void Update()
    {

        if (target != null && !targetRenderer.isVisible)
        {
            Vector3 difference = target.transform.position - transform.position;
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotationZ);
            timer = 0; //reinicio el timer por si salio de la pantalla del jugador
        }
        else
        {
            timer += Time.deltaTime;
            if (timer >= destroyTimer)
                Destroy(this.gameObject);
        }

    }
}
