using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [Header("Rain Objects")]
    [SerializeField] private List<GameObject> objects;
    [SerializeField] private EntranceTrigger entranceTrigger;

    [SerializeField] private Transform player;
    [SerializeField] private float followSpeed;
    [SerializeField] private bool toFollow;
    [SerializeField] private float backwardSpeed = 2f; // Speed when moving backward

    private void Start()
    {
        toFollow = entranceTrigger.followObjects;
        entranceTrigger.onfollowObject += FollowObject;
    }

    private void OnDisable()
    {
        entranceTrigger.onfollowObject -= FollowObject;
    }

    private void FollowObject(bool followObjects)
    {
        toFollow = followObjects;
    }

    private void Update()
    {
        if (player != null)
        {
            if (toFollow)
            {
                // Follow the player
                transform.position = Vector3.Lerp(transform.position, player.position, followSpeed * Time.deltaTime);
            }
            else
            {
                // Move backward on the Z-axis only
                transform.position += new Vector3(0, 0, -backwardSpeed * Time.deltaTime);
            }
        }
    }




}
