using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [Header("Rain Objects")]
    [SerializeField] private List<GameObject> objects;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Entrance"))
        {
            foreach(GameObject obj in objects)
            {
                obj.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Entrance"))
        {
            foreach (GameObject obj in objects)
            {
                obj.SetActive(true);
            }
        }
    }
}
