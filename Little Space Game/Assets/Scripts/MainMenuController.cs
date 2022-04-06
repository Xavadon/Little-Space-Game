using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void LoadGame()
    {
        StartCoroutine(loadGame());
    }
    public void MainMenu()
    {
        StartCoroutine(mainMenu());
    }
    public void Quit()
    {
        Application.Quit();
    }
    IEnumerator loadGame()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().KillPlayer();
        }
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("GameScene");
    }
    IEnumerator mainMenu()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().KillPlayer();
        }
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MainMenu");
    }
}
