using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRespawnTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Instead of triggering respawn here, you can handle other actions if needed
            Debug.Log("Player collided with obstacle trigger.");
        }
    }

}
