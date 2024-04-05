using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishLine : MonoBehaviour
{
    public GameObject levelCompleteObject; // Reference to the "Level Complete" GameObject
    public GameObject particleEffectObject; // Reference to the particle effect GameObject

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        // Ensure the "Level Complete" GameObject is initially disabled
        if (levelCompleteObject != null)
        {
            levelCompleteObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("levelCompleteObject reference is not assigned!");
        }
    }

    // This method is called when another collider enters the trigger collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that entered the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Check if the levelCompleteObject reference is assigned
            if (levelCompleteObject != null)
            {
                // Activate the levelCompleteObject
                levelCompleteObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("levelCompleteObject reference is not assigned! GameObject will not be activated.");
            }

            // Check if the particleEffectObject reference is assigned
            if (particleEffectObject != null)
            {
                // Activate the particleEffectObject
                particleEffectObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("particleEffectObject reference is not assigned! GameObject will not be activated.");
            }
        }
    }
}
