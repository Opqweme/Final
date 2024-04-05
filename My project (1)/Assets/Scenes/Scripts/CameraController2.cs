using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController2 : MonoBehaviour
{
    [SerializeField] private Transform player;
    private float smoothSpeed = 0.125f;
    private float startFollowXPosition = 0;
    public Vector3 offset;

    private bool _isFollowing = false;
    private bool _freezeYPosition = true;

    // Start is called before the first frame update
    void Start()
    {
        // Reset the camera position to the initial location
        transform.position = new Vector3(9.2f, -1.3f, transform.position.z);
    }


    // Update is called once per frame
    void Update()
    {
        if (!_isFollowing && player != null)
        {
            if (player.position.x >= startFollowXPosition)
            {
                _isFollowing = true;
            }
        }

        if (_isFollowing && player != null)
        {
            var desiredPosition = player.position + offset;
            var smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Debug log for player's X position
            //Debug.Log("Player X position: " + player.position.x);

            // Debug log for freezeYPosition
            //Debug.Log("_freezeYPosition: " + _freezeYPosition);

            // Check if player's position is at x = 7
            if (player.position.x == 7f)
            {
                // Freeze the camera's Y position
                _freezeYPosition = true;
            }

            // Update the camera position based on freezeYPosition
            if (_freezeYPosition)
            {
                // Fix camera's y position
                transform.position = new Vector3(smoothedPosition.x, transform.position.y, transform.position.z);
            }
            else
            {
                // Allow camera's position to follow player freely
                transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
            }
        }
    }

    public void OnPlayerRespawn()
    {
        Debug.Log("Player respawned"); // Debug log to check if the method is being called

        // Reset camera's y position to -1.3f
        transform.position = new Vector3(transform.position.x, -1.3f, transform.position.z);

        // Reset _freezeYPosition to true
        _freezeYPosition = true;
    }
}
