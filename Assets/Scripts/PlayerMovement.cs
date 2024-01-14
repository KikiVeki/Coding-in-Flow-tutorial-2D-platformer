using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rigidBody;
    private Animator animator;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;

    float dirX = 0f;
    private enum MovementState { idle, running, jumping, falling };    

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();    
    }

     private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rigidBody.velocity = new Vector2(dirX * moveSpeed, rigidBody.velocity.y);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpForce);
        }

        UpdateAnimationState();
    }

    private void UpdateAnimationState() 
    {
        MovementState movementState;

        if (dirX > 0f)
        {
            movementState = MovementState.running;
            spriteRenderer.flipX = false;
        }
        else if (dirX < 0f)
        {
            movementState = MovementState.running;
            spriteRenderer.flipX = true;
        }
        else
        {
            movementState = MovementState.idle;
        }

        if (rigidBody.velocity.y > .01f)
        {
            movementState = MovementState.jumping;
        } 
        else if(rigidBody.velocity.y < -.01f)
        {
            movementState = MovementState.falling;
        }

        animator.SetInteger("movementState", (int)movementState);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }
}
