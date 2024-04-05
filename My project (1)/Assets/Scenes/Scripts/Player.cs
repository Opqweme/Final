using UnityEngine;

public class Player : MonoBehaviour
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

    public Gamemodes CurrentGamemode;
    public Speeds CurrentSpeed;
    public Gravity CurrentGravity;

    private void Start()
    {
        groundCheckOffset = groundCheckObject.position - transform.position;
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
            ChangeToNormalSprite();
        }

        if (Time.time - rotationStartTime < RotationDuration)
        {
            float t = (Time.time - rotationStartTime) / RotationDuration;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, t);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Portal"))
        {
            Die();
            _isFlying = true;
            Debug.Log("Entered Fly Mode");
            ChangeSprite();
        }
        else if (other.CompareTag("ChangeSpriteTrigger"))
        {
            ChangeSprite();
        }
        else if (other.CompareTag("ChangeSpriteTrigger1"))
        {
            ChangeToNormalSprite();
        }
        else if (other.CompareTag("Portal1"))
        {
            SlowSpeed();
        }
        else if (other.CompareTag("ChangeSpeed"))
        {
            CurrentSpeed = Speeds.Normal;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Portal1"))
        {
            _isFlying = false;
            Debug.Log("Exited Fly Mode");
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

    private void ChangeToNormalSprite()
    {
        if (normalSprite != null)
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = normalSprite;
            }

            ResetMovementConstraints();
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
        // Handle player death here, such as resetting constraints or playing death animation
        ResetMovementConstraints(); // Reset movement constraints
        // You may add more logic here based on your game's requirements

        // Change sprite back to normal
        ChangeToNormalSprite();
    }
    
}
