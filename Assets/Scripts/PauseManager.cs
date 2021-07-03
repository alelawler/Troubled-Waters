using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public KeyCode pauseKey = KeyCode.Escape;
    public KeyCode pauseKey2 = KeyCode.P;
    public GameObject pauseContainer;
    public Health playerHealth;

    private void Update()
    {
        if (playerHealth.isAlive)
        {

            if (Input.GetKeyDown(pauseKey) || Input.GetKeyDown(pauseKey2))
            {
                pauseContainer.SetActive(!pauseContainer.activeSelf);

                if (pauseContainer.activeSelf)
                    Time.timeScale = 0f;
                else
                    Time.timeScale = 1f;
            }
        }
    }
}
