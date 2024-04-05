using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float positionCheckInterval = 1f; // Interval to check player's position
    [SerializeField] private float positionCheckThreshold = 0.01f; // Threshold for position change
    [SerializeField] private float stuckDurationThreshold = 0.054f; // Time threshold for being stuck
    [SerializeField] private LayerMask platformLayer; // Layer mask for platforms

    private bool isPaused = false;
    private bool isStuck = false;
    private Vector2 previousPosition;
    private Vector2 startPosition;
    private float lastPositionCheckTime;
    private float stuckTimer = 0f;

    private CameraController cameraController;
    private Player player; 

    void Start()
    {
        // Record the player's starting position
        startPosition = transform.position;
        previousPosition = startPosition;
        lastPositionCheckTime = Time.time;

        cameraController = FindObjectOfType<CameraController>();
        player = FindObjectOfType<Player>();
    }

    public void RespawnPlayer()
    {
        Debug.Log("Player respawned at: " + respawnPoint.position);
        transform.position = respawnPoint.position;
        Time.timeScale = 0f;
        isPaused = true;

        if (cameraController != null)
        {
            cameraController.OnPlayerRespawn();
        }
        if (player != null)
        {
            player.ResetMovementConstraints(); // Call the method to reset movement constraints
        }
    }

    void Update()
    {
        if (isPaused && Input.anyKeyDown)
        {
            Time.timeScale = 1f;
            isPaused = false;
        }

        // Check if the player is stuck and not jumping to another platform
        if (isStuck && Time.time - lastPositionCheckTime >= positionCheckInterval)
        {
            // Update the last position check time
            lastPositionCheckTime = Time.time;

            // Check if the player's position has stopped changing
            Vector2 currentPosition = transform.position;
            if (Vector2.Distance(currentPosition, previousPosition) < positionCheckThreshold)
            {
                // If player position has not changed significantly, start the stuck timer
                stuckTimer += Time.deltaTime;

                // If the stuck timer exceeds the threshold, respawn the player
                if (stuckTimer >= stuckDurationThreshold)
                {
                    RespawnPlayer();
                    stuckTimer = 0f; // Reset the stuck timer after respawn
                }
            }
            else
            {
                // Reset the stuck timer if the player's position has changed
                stuckTimer = 0f;
            }

            // Update the previous position
            previousPosition = currentPosition;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision.gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        HandleCollision(other.gameObject);
    }

    void HandleCollision(GameObject collidedObject)
    {
        if (collidedObject.CompareTag("Player"))
        {
            Debug.Log("Player collided with obstacle");
            RespawnPlayer();
        }
        else if (collidedObject.CompareTag("Obstacle"))
        {
            // Handle collision with obstacles as needed
            Debug.Log("Player collided with obstacle");
            RespawnPlayer(); // For example, you can respawn the player when colliding with an obstacle
        }
        else
        {
            // Handle collision with other objects if necessary
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // Reset isStuck when no longer colliding with a platform
        if (((1 << collision.gameObject.layer) & platformLayer) != 0)
        {
            isStuck = false;
            Debug.Log("Player exited platform");
        }
        else
        {
            Debug.Log("Player exited obstacle"); // Debug log for obstacle exit
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // Reset isStuck when no longer colliding with a platform
        if (other.CompareTag("Player"))
        {
            isStuck = false;
            Debug.Log("Player exited obstacle trigger");
        }
    }
}
