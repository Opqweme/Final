using UnityEngine;

public class CameraController : MonoBehaviour
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

            // Check if player's x position is greater than or equal to 115.86f
            if (player.position.x >= 115.8f && player.position.x < 140.5f)
            {
                // Unfreeze the camera's Y position
                _freezeYPosition = false;
            }

            // Check if player's position is at x = 7
            if (player.position.x == 7f)
            {
                // Freeze the camera's Y position
                _freezeYPosition = true;
            }
            // Check if player's position is between x >= 140.5f and x < 177.1f
            else if (player.position.x >= 140.5f && player.position.x < 177.1f)
            {
                // Freeze the camera's Y position
                _freezeYPosition = true;
            }
            // Check if player's position is at x >= 177.1f and x < 242f
            else if (player.position.x >= 177.1f && player.position.x < 242f)
            {
                // Unfreeze the camera's Y position
                _freezeYPosition = false;
            }
            // Check if player's position is at x >= 242f
            else if (player.position.x >= 242f && player.position.x < 459f)
            {
                // Freeze the camera's Y position
                _freezeYPosition = true;
            }
            //Check if player's rotation is at x >= 459f 
            else if (player.position.x >= 459f && player.position.x < 476f)
            {
                _freezeYPosition = false;
            }
            else if (player.position.x >= 476f && player.position.x < 579f)
            {
                // Freeze the camera's Y position
                _freezeYPosition = true;
            }
            // Check if player's position is at x >= 579f
            else if (player.position.x >= 579f)
            {
                // Unfreeze the camera's Y position
                _freezeYPosition = false;
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


