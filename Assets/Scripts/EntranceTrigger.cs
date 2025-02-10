using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntranceTrigger : MonoBehaviour
{
    public bool followObjects = true;
    public Action<bool> onfollowObject;

    public Animator tpsAnimator;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Entrance"))
        {
            followObjects = false;
            onfollowObject?.Invoke(followObjects);
        }
        else if (other.gameObject.CompareTag("Door"))
        {
            StartCoroutine(RotateDoor(other.gameObject));
        }
        else if (other.gameObject.CompareTag("Vehicle"))
        {
            UiManager.Instance.NextCommand();
        }
    }


    // Coroutine to smoothly rotate the door
    private IEnumerator RotateDoor(GameObject door)
    {
        Quaternion startRotation = door.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, 90, 0); // Rotating only on Y-axis

        float duration = 1f; // Time in seconds for the rotation
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            door.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        door.transform.rotation = targetRotation; // Ensure it reaches exactly 90°
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Entrance"))
        {
            followObjects = true;
            onfollowObject?.Invoke(followObjects);
        }
    }
}
