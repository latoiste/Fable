using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    [SerializeField, Range(1, 5)]
    private float moveSpeed;

    private enum Direction
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
    
    private Animator animator;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 direction = ctx.ReadValue<Vector2>();
        // direction.Normalize();

        rb.linearVelocity = direction * moveSpeed;
    }

    void Update()
    {
        if (rb.linearVelocityX > 0)
        {
            animator.SetInteger("Direction", (int)Direction.RIGHT);
            sprite.flipX = true;
        } else if (rb.linearVelocityX < 0)
        {
            animator.SetInteger("Direction", (int)Direction.LEFT);
            sprite.flipX = false;
        } 
        else if (rb.linearVelocityY > 0)
        {
            animator.SetInteger("Direction", (int)Direction.UP);
        } else if (rb.linearVelocityY < 0)
        {
            animator.SetInteger("Direction", (int)Direction.DOWN);
        }
    }
}
