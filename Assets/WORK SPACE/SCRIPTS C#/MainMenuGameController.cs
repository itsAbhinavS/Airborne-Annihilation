using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuGameController : MonoBehaviour
{
    public Transform playButton;
    public Transform quitButton;

    private bool hoverPlay;
    private bool hoverQuit;

    public void onhover(int i) 
    {
        if (i == 1) hoverPlay = true;
        if (i == 2) hoverQuit = true;
    }

    public void nothover(int i)
    {
        if (i == 1) hoverPlay = false;
        if (i == 2) hoverQuit = false;
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    // Create a ray from the camera to the mouse pointer
        RaycastHit hit;    // The raycast hit information

        if (Physics.Raycast(ray, out hit))    // Cast the ray and check if it hits something
        {
            Debug.Log("Hit object with tag: " + hit.collider.tag);
            if (hit.collider.CompareTag("playbutton"))
            {
                hoverPlay = true;
                if (Input.GetMouseButtonDown(0)) 
                {
                    LoadScene();
                }
            }
            else 
            {
                hoverPlay = false;
            }

            if (hit.collider.CompareTag("quitbutton"))
            {
                hoverQuit = true;
                if (Input.GetMouseButtonDown(0))
                {
                    QuitGame();
                }
            }
            else
            {
                hoverQuit = false;
            }
        }
        



        if (hoverPlay)
        {
            playButton.position = Vector3.Lerp(playButton.position, new Vector3(0, 0, 1.7f), 10 * Time.deltaTime);
        }
        else 
        {
            playButton.position = Vector3.Lerp(playButton.position, new Vector3(0, 0, 1f), 10 * Time.deltaTime);
        }

        if (hoverQuit)
        {
            quitButton.position = Vector3.Lerp(quitButton.position, new Vector3(0, -0.75f, 1.7f), 10 * Time.deltaTime);
        }
        else
        {
            quitButton.position = Vector3.Lerp(quitButton.position, new Vector3(0, -0.75f, 1f), 10 * Time.deltaTime);
        }
    }

    public void QuitGame() 
    {
        Application.Quit();
    }

    public void LoadScene() 
    {
        SceneManager.LoadScene(1);
    }
}
