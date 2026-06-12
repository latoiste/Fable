using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    private readonly int DirectionHash = Animator.StringToHash("Direction");
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
    private bool frozen = false;

    public Vector3 Position => rb.position;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (frozen) return;

        Vector2 direction = ctx.ReadValue<Vector2>();
        // direction.Normalize();

        rb.linearVelocity = direction * moveSpeed;
    }

    public void Freeze()
    {
        frozen = true;
        rb.linearVelocity = Vector2.zero;
        animator.SetInteger(DirectionHash, (int)Direction.DOWN);
        enabled = false;
    }

    public void Unfreeze()
    {
        frozen = false;
        enabled = true;
    }

    public void SetSpawnPoint(Vector3 spawn)
    {
        rb.position = spawn;
    }

    void Update()
    {
        if (rb.linearVelocityX > 0)
        {
            animator.SetInteger(DirectionHash, (int)Direction.RIGHT);
            sprite.flipX = true;
        } else if (rb.linearVelocityX < 0)
        {
            animator.SetInteger(DirectionHash, (int)Direction.LEFT);
            sprite.flipX = false;
        } 
        else if (rb.linearVelocityY > 0)
        {
            animator.SetInteger(DirectionHash, (int)Direction.UP);
        } else if (rb.linearVelocityY < 0)
        {
            animator.SetInteger(DirectionHash, (int)Direction.DOWN);
        }
    }
}
