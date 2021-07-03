using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwisterManager : MonoBehaviour
{
    public float twisterStrenght;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,0,15);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        collision.transform.position -= (collision.transform.position - transform.position)/ (100/twisterStrenght);
    }
}
