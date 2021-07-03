using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSpriteManager : MonoBehaviour
{
    public SpriteRenderer myRenderer;
    public float spriteTimer;

    private void Start()
    {
        myRenderer = GetComponent<SpriteRenderer>();
    }
        
    private void Update()
    {
        Color tempColor = myRenderer.color;
        tempColor.a -= 1 / spriteTimer*Time.deltaTime;
        
        if (tempColor.a <= 0f)
            Destroy(this.gameObject);

        myRenderer.color = tempColor;
        transform.position += transform.up * -1 * Time.deltaTime;
        
    }
}
