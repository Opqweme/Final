using UnityEngine;
using UnityEngine.SceneManagement;

public class Player2 : MonoBehaviour
{
    private const float NormalSpeed = 9;
    private const float SlowSpeedValue = 6;
    private const float JumpForce = 13;
    private const float RotationAngle = -90f; // Changed to negative for clockwise rotation
    private const float RotationDuration = 0.3f; // Duration of rotation in seconds
    private const float FlyForce = 7f; // Define the FlyForce constant

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private Transform groundCheckObject; // Ground check object, attached to the player
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Sprite newSprite; // New sprite to change into
    [SerializeField] private Sprite normalSprite; // Normal sprite for the player

    private bool _isGrounded;
    private bool _canJump = true;
    private bool _isFlying;
    private Vector3 groundCheckOffset;
    private Quaternion targetRotation;
    private float rotationStartTime;
    private Respawn respawnScript;

    public Gamemodes CurrentGamemode;
    public Speeds CurrentSpeed;
    public Gravity CurrentGravity;

    private void Start()
    {
        groundCheckOffset = groundCheckObject.position - transform.position;
        respawnScript = GetComponent<Respawn>();
        if (respawnScript == null)
        {
            Debug.LogError("Respawn component not found on the player!");
        }
    }

    private void Update()
    {
        if (!Global.InPlayMode) return;

        groundCheckObject.position = transform.position + groundCheckOffset;

        _isGrounded = Physics2D.OverlapCircle(groundCheckObject.position, 0.1f, layerMask);

        if (_isGrounded && Input.GetKeyDown(KeyCode.Space) && _canJump)
        {
            Jump();
        }

        if (_isGrounded)
        {
            particleSystem.Play();
            _canJump = true;
        }
        else
        {
            particleSystem.Stop();
            if (_isFlying && Input.GetKey(KeyCode.Space))
            {
                Fly();
            }
        }

        if (!_isFlying)
        {
            ChangeToNormalSprite2();
        }

        if (Time.time - rotationStartTime < RotationDuration)
        {
            float t = (Time.time - rotationStartTime) / RotationDuration;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, t);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Portal3"))
        {
            SlowSpeed();
            ResetPlayerState();
            _isFlying = true;
            ChangeSprite();
        }
        else if (other.CompareTag("ChangeSpriteTrigger2"))
        {
            ChangeToNormalSprite2();
            CurrentSpeed = Speeds.Normal;
        }
        else if (other.CompareTag("DeathTrigger"))
        {
            Die(); // Call Die() method when the player enters the death trigger
            if (respawnScript != null)
            {
                respawnScript.RespawnPlayer(); // Call RespawnPlayer() method from the Respawn component
            }
            else
            {
                Debug.LogError("Respawn script not found!");
            }
        }
    }


    private void ChangeSprite()
    {
        if (newSprite != null)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = newSprite;
            }
        }
    }

    private void ChangeToNormalSprite2()
    {
        Debug.Log("Changing to normal sprite");
        if (normalSprite != null)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = normalSprite;
                Debug.Log("Sprite changed successfully to normal sprite");
            }
            else
            {
                Debug.LogError("SpriteRenderer component not found!");
            }

            ResetMovementConstraints();
        }
        else
        {
            Debug.LogError("Normal sprite not assigned!");
        }
    }

    private void FixedUpdate()
    {
        if (!Global.InPlayMode) return;
        Move();
        particleSystem.transform.position = transform.position + new Vector3(0f, -0.6f, 0f);
    }

    private void Move()
    {
        float speed = (CurrentSpeed == Speeds.Normal) ? NormalSpeed : GetSlowSpeed();
        rb.velocity = new Vector2(speed, rb.velocity.y);
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, JumpForce);
        targetRotation = Quaternion.Euler(0, 0, transform.eulerAngles.z + RotationAngle);
        rotationStartTime = Time.time;
        _canJump = false;
    }

    private void Fly()
    {
        rb.velocity = new Vector2(rb.velocity.x, FlyForce);
    }

    public void ChangeThroughPortal(Gamemodes gamemode, Speeds speed, Gravity gravity, int state)
    {
        switch (state)
        {
            case 0:
                CurrentSpeed = speed;
                break;
            case 1:
                CurrentGamemode = gamemode;
                break;
            case 2:
                rb.gravityScale = Mathf.Abs(rb.gravityScale) * (int)gravity;
                CurrentGravity = gravity;
                break;
            case 3:
                rb.gravityScale = Mathf.Abs(rb.gravityScale) * -(int)gravity;
                CurrentGravity = gravity == Gravity.Upright ? Gravity.Upsidedown : Gravity.Upright;
                break;
        }
    }

    public void ResetMovementConstraints()
    {
        _canJump = true;
        _isFlying = false;
    }

    private void SlowSpeed()
    {
        CurrentSpeed = Speeds.Slow;
    }

    private float GetSlowSpeed()
    {
        return SlowSpeedValue;
    }     

    private void Die()
    {
        // Reset player state upon death
        ResetPlayerState();
        CurrentSpeed = Speeds.Normal;
    }

    private void ResetPlayerState()
    {
        // Check if the current sprite is the new sprite
        if (GetComponent<SpriteRenderer>().sprite == newSprite)
        {
            // Change sprite back to normal if it's the new sprite
            ChangeToNormalSprite2();
        }

        // Reset movement constraints
        ResetMovementConstraints();

        // Disable flying
        _isFlying = false;

        // Allow jumping
        _canJump = true;
    }   
}
