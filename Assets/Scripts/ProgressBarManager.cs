using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarManager : MonoBehaviour
{
    public int endLvlValue;
    public Slider mySlider;
    public GameObject spawner;

    private void Awake()
    {
        mySlider = this.gameObject.GetComponent<Slider>();
        mySlider.maxValue = endLvlValue;
    }
    public void AddProgress(int progressValue = 1)
    {

        mySlider.value += progressValue;

        //si llego al maximo, dejo de spawnear pero hay que matar a los que ya spawnearon
        if (mySlider.value >= endLvlValue)
        {
            
            spawner.SendMessage("StopSpawning");
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            mySlider.value = endLvlValue;
            spawner.SendMessage("StopSpawning");
        }
    }
}
