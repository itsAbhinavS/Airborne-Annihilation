using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuGameController : MonoBehaviour
{
    public float duration = 3.0f;
    private Vector3 endPosition;


    public void OnHover(GameObject woodButton) 
    {
        StartCoroutine(LerpPosition(woodButton));
    }

    IEnumerator LerpPosition(GameObject woodButton)
    {
        float elapsedTime = 0.0f;    // The elapsed time since the lerp started
        Vector3 currentPosition = woodButton.transform.position;    // The current position of the lerp
        endPosition =  new Vector3(woodButton.transform.position.x, woodButton.transform.position.y, 1.7f);

        while (elapsedTime < duration)    // Loop until the duration is reached
        {
            currentPosition = Vector3.Lerp(currentPosition, endPosition, elapsedTime / duration);    // Update the current position of the lerp using Lerp
            woodButton.transform.position = currentPosition;    // Update the position of the object
            elapsedTime += Time.deltaTime;    // Increment the elapsed time by the frame time
            yield return null;    // Yield until the next frame
        }

        woodButton.transform.position = endPosition;    // Set the final position to the end position
    }

    public void NotOnHover(GameObject woodButton)
    {
        StartCoroutine(LerpPositionBack(woodButton));
    }

    IEnumerator LerpPositionBack(GameObject woodButton)
    {
        float elapsedTime = 0.0f;    // The elapsed time since the lerp started
        Vector3 currentPosition = woodButton.transform.position;    // The current position of the lerp
        endPosition = new Vector3(woodButton.transform.position.x, woodButton.transform.position.y, 1f);

        while (elapsedTime < duration)    // Loop until the duration is reached
        {
            currentPosition = Vector3.Lerp(currentPosition, endPosition, elapsedTime / duration);    // Update the current position of the lerp using Lerp
            woodButton.transform.position = currentPosition;    // Update the position of the object
            elapsedTime += Time.deltaTime;    // Increment the elapsed time by the frame time
            yield return null;    // Yield until the next frame
        }

        woodButton.transform.position = endPosition;    // Set the final position to the end position
    }
}
