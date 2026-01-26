using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicControl : MonoBehaviour
{
    [Header("Movement Settings")]
    public float baseMoveSpeed = 5f;
    public float baseJumpForce = 7f;

    [Header("Ground Detection")]
    public Transform groundCheck;    // Assign an empty child object here
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;    // Select your "Ground" layer here

    private Rigidbody2D _rb;
    private float _currentMoveSpeed;
    private SpriteRenderer _sr;
    private bool _isDead = false;
    private bool _isGrounded;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        if (transform.childCount > 0)
            _sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
            
        _currentMoveSpeed = baseMoveSpeed;
    }

    void Update()
    {
        if (_isDead) return;
        
        CheckGround();
        
        HandleMovement();
        
        HandleJump();
        
        float h = Input.GetAxis("Horizontal");
        if (h != 0 && _sr != null)
        {
            _sr.flipX = h > 0; // Note: Depending on sprite, this might need to be h < 0
        }
    }

    void CheckGround()
    {
        // Creates a small invisible circle at the feet to see if it touches the Ground Layer
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void HandleJump()
    {
        // Jump only if Space is pressed AND we are touching the ground
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, baseJumpForce);
        }
    }

    void HandleMovement()
    {
        // Removed the specific KeyCode check to allow Joystick/Arrow support automatically
        float h = Input.GetAxis("Horizontal");
        _rb.velocity = new Vector2(h * _currentMoveSpeed, _rb.velocity.y);
    }
    
    // Visualization for the Editor to see the ground check circle
    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}