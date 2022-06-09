using UnityEngine;

public class PlayerController : MonoBehaviour, IMovePointDependable
{
    [Header("Fields")]
    [Range(0f, 10f)] [SerializeField] private float speed = 4f;
    private bool isPlayerDeath;
    private Vector2 lastInput;
    private Vector3 origin;

    [Header("Assets")]
    [SerializeField] private Sprite startSprite;

    [Header("Layers")]
    [SerializeField] private LayerMask wall;
    [SerializeField] private LayerMask separator;

    [Header("Game Objects")]
    [SerializeField] private Transform movePoint;

    // Components
    private PlayerManager playerManager;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private AudioSource audioSource;

    private void Start()
    {
        // Get components
        playerManager = GetComponent<PlayerManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        // Initialize player movement
        origin = transform.position;
        movePoint.parent = null;
    }

    private void Update()
    {
        isPlayerDeath = playerManager.ScoreManager.IsPlayerDeath;
        if (!isPlayerDeath) PlayerMovement();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        /*
        if (other.gameObject.tag == "PowerUp")
        {
            Destroy(other.gameObject);
            playerManager.PowerUp = true;
        }
        */
    }

    private void PlayerMovement()
    {
        // Move towards move point
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, speed * Time.deltaTime);

        // Determine last input
        if (Input.GetAxisRaw("Horizontal") == 1 && !Physics2D.OverlapCircle(movePoint.position + Vector3.right, 0.25f, wall) && !Physics2D.OverlapCircle(movePoint.position + Vector3.right, 0.25f, separator))
        {
            lastInput = Vector2.right;
            animator.enabled = true;
        }
        else if (Input.GetAxisRaw("Horizontal") == -1 && !Physics2D.OverlapCircle(movePoint.position + Vector3.left, 0.25f, wall) && !Physics2D.OverlapCircle(movePoint.position + Vector3.left, 0.25f, separator))
        {
            lastInput = Vector2.left;
            animator.enabled = true;
        }
        else if (Input.GetAxisRaw("Vertical") == 1 && !Physics2D.OverlapCircle(movePoint.position + Vector3.up, 0.25f, wall) && !Physics2D.OverlapCircle(movePoint.position + Vector3.up, 0.25f, separator))
        {
            lastInput = Vector2.up;
            animator.enabled = true;
        }
        else if (Input.GetAxisRaw("Vertical") == -1 && !Physics2D.OverlapCircle(movePoint.position + Vector3.down, 0.25f, wall) && !Physics2D.OverlapCircle(movePoint.position + Vector3.down, 0.25f, separator))
        {
            lastInput = Vector2.down;
            animator.enabled = true;
        }

        // Move MovePoint towards last input
        if (Vector3.Distance(transform.position, movePoint.position) <= 0)
        {
            if (lastInput == Vector2.right && !Physics2D.OverlapCircle(movePoint.position + Vector3.right, 0.25f, wall) && !Physics2D.OverlapCircle(movePoint.position + Vector3.right, 0.25f, separator))
            {
                movePoint.position = new Vector3(transform.position.x + 1, transform.position.y, 0);
                transform.eulerAngles = Vector3.zero;
            }
            else if (lastInput == Vector2.left && !Physics2D.OverlapCircle(movePoint.position + Vector3.left, 0.25f, wall) && !Physics2D.OverlapCircle(movePoint.position + Vector3.left, 0.25f, separator))
            {
                movePoint.position = new Vector3(transform.position.x - 1, transform.position.y, 0);
                transform.eulerAngles = new Vector3(0, 0, 180);
            }
            else if (lastInput == Vector2.up && !Physics2D.OverlapCircle(movePoint.position + Vector3.up, 0.25f, wall) && !Physics2D.OverlapCircle(movePoint.position + Vector3.up, 0.25f, separator))
            {
                movePoint.position = new Vector3(transform.position.x, transform.position.y + 1, 0);
                transform.eulerAngles = new Vector3(0, 0, 90);
            }
            else if (lastInput == Vector2.down && !Physics2D.OverlapCircle(movePoint.position + Vector3.down, 0.25f, wall) && !Physics2D.OverlapCircle(movePoint.position + Vector3.down, 0.25f, separator))
            {
                movePoint.position = new Vector3(transform.position.x, transform.position.y - 1, 0);
                transform.eulerAngles = new Vector3(0, 0, 270);
            }
            else
            {
                movePoint.position = new Vector3(transform.position.x, transform.position.y, 0);
                animator.enabled = false;
            }
        }
    }

    private void Reset()
    {
        // Reset position and movement
        transform.position = origin;
        movePoint.position = origin;
        lastInput = Vector3.zero;

        // Reset sprite and animation
        animator.enabled = false;
        transform.eulerAngles = Vector3.zero;
        spriteRenderer.sprite = startSprite;
        animator.Play("Player Idle");

        audioSource.Play();

        // Reset movement
        isPlayerDeath = false;
        playerManager.ScoreManager.IsPlayerDeath = isPlayerDeath;
    }

    // Expose move point position
    public void SetMovePointPosition(Vector3 position)
    {
        movePoint.position = position;
    }
}
