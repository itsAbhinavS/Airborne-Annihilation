using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuGameController : MonoBehaviour
{
    public void QuitGame() 
    {
        Application.Quit();
    }

    public void LoadScene() 
    {
        SceneManager.LoadScene(1);
    }
}
