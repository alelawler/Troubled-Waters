using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldBehaviour : MonoBehaviour
{


    public GameObject target;


    private float movementSpeed = 15;
    private bool isPlaced = false;
    private Vector3 auxPos;
    private Vector3 randomPos;
    private Vector3 randomScale;
    private float timer;
    

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Target");
        randomPos = transform.position + new Vector3(Random.value, Random.value) * 3;

        float randomScaleMultiplyer = Random.Range(-0.3f, 0.3f);
        randomScale = transform.localScale + new Vector3(randomScaleMultiplyer, randomScaleMultiplyer);
        Debug.Log(randomScale);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPlaced)
        {
            auxPos = transform.position;


            transform.position = Vector3.MoveTowards(transform.position, randomPos, movementSpeed * 2 * Time.deltaTime);
            transform.localScale = randomScale;

            if (auxPos == transform.position)
            {
                isPlaced = true;
            }
        }
        else
        {

            if (timer >= 0.5f)
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position,  movementSpeed * Time.deltaTime*1.2f);
            else
                timer += Time.deltaTime;
        }
    }
}
