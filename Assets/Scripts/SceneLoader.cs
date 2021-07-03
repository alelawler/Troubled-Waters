using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Image blackScreen;
    public void LoadScene(int sceneIndex)
    {
        StartCoroutine(FadeToBlack(sceneIndex));
        Time.timeScale = 1;
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(FadeToBlack(sceneName));
        Time.timeScale = 1;

    }

    private IEnumerator FadeToBlack(string sceneName)
    {

        if (blackScreen != null)
        {

            blackScreen.gameObject.SetActive(true);
            while (true)
            {

                yield return new WaitForSeconds(0.01f);

                Color tempColor = blackScreen.color;
                tempColor.a += 0.03f;
                blackScreen.color = tempColor;

                if (tempColor.a >= 1f)
                {
                    SceneManager.LoadScene(sceneName);
                }
            }
        }
        else
        {

            SceneManager.LoadScene(sceneName);
        }
    }
    private IEnumerator FadeToBlack(int sceneIndex)
    {
        blackScreen.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.01f);

        Color tempColor = blackScreen.color;
        tempColor.a -= 0.02f;
        blackScreen.color = tempColor;

        if (tempColor.a == 1f)
            SceneManager.LoadScene(sceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
