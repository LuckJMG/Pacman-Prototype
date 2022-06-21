using System.Diagnostics.Tracing;
using System.ComponentModel;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour, IMovePointDependable {
    [Header("Fields")]
    [Range(0f, 10f)] [SerializeField] float speed = 4f;
    bool isPlayerDeath;
    bool pause;
    bool gameOver;
    Vector2 lastInput = Vector2.right;
    Vector3 origin;

    // Events
    public static Action OnRestart;
    public static Action OnGameOver;

    [Header("Assets")]
    [SerializeField] Sprite startSprite;

    [Header("Layers")]
    [SerializeField] LayerMask wall;
    [SerializeField] LayerMask separator;

    // Components
    SpriteRenderer spriteRenderer;
    Animator animator;

    // Game Objects
    Transform movePoint;

    void Awake() {
        // Get components
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Restart static events
        OnRestart = null;
        OnGameOver = null;

        // Find game objects
        movePoint = transform.GetChild(0).GetComponent<Transform>();
    }

    void Start() {
        // Subscribe to events
        GameManager.OnPause += isPause => pause = isPause;
        GameManager.OnGameOver += () => {
            gameOver = true;
            spriteRenderer.enabled = false;
        };
        ScoreManager.OnPlayerDeath += () => isPlayerDeath = true;

        // Initialize
        origin = transform.position;
        movePoint.parent = null;
    }

    void Update() {
        if (!pause && !gameOver) {
            if (!isPlayerDeath) PlayerMovement();
        }
    }

    void PlayerMovement() {
        animator.enabled = true;

        // Move towards move point
        transform.position = Vector2.MoveTowards(transform.position, movePoint.position, speed * Time.deltaTime);

        DetermineLastInput();

        // Move MovePoint towards last input
        if (Vector3.Distance(transform.position, movePoint.position) <= 0) {
            if (lastInput == Vector2.right && !MovePointOverlapCircle(Vector2.right, wall) && !MovePointOverlapCircle(Vector2.right, separator)) {
                movePoint.position = new Vector2(transform.position.x + 1, transform.position.y);
                transform.eulerAngles = Vector3.zero;
            }
            else if (lastInput == Vector2.left && !MovePointOverlapCircle(Vector2.left, wall) && !MovePointOverlapCircle(Vector2.left, separator)) {
                movePoint.position = new Vector2(transform.position.x - 1, transform.position.y);
                transform.eulerAngles = new Vector3(0, 0, 180);
            }
            else if (lastInput == Vector2.up && !MovePointOverlapCircle(Vector2.up, wall) && !MovePointOverlapCircle(Vector2.up, separator)) {
                movePoint.position = new Vector2(transform.position.x, transform.position.y + 1);
                transform.eulerAngles = new Vector3(0, 0, 90);
            }
            else if (lastInput == Vector2.down && !MovePointOverlapCircle(Vector2.down, wall) && !MovePointOverlapCircle(Vector2.down, separator)) {
                movePoint.position = new Vector2(transform.position.x, transform.position.y - 1);
                transform.eulerAngles = new Vector3(0, 0, 270);
            }
            else {
                movePoint.position = new Vector2(transform.position.x, transform.position.y);
                animator.enabled = false;
            }
        }
    }

    void DetermineLastInput() {
        if (Input.GetAxisRaw("Horizontal") == 1 && !MovePointOverlapCircle(Vector2.right, wall) && !MovePointOverlapCircle(Vector2.right, separator)) {
            lastInput = Vector2.right;
            animator.enabled = true;
        }
        else if (Input.GetAxisRaw("Horizontal") == -1 && !MovePointOverlapCircle(Vector2.left, wall) && !MovePointOverlapCircle(Vector2.left, separator)) {
            lastInput = Vector2.left;
            animator.enabled = true;
        }
        else if (Input.GetAxisRaw("Vertical") == 1 && !MovePointOverlapCircle(Vector2.up, wall) && !MovePointOverlapCircle(Vector2.up, separator)) {
            lastInput = Vector2.up;
            animator.enabled = true;
        }
        else if (Input.GetAxisRaw("Vertical") == -1 && !MovePointOverlapCircle(Vector2.down, wall) && !MovePointOverlapCircle(Vector2.down, separator)) {
            lastInput = Vector2.down;
            animator.enabled = true;
        }
    }

    void Reset() {
        if (ScoreManager.lives <= 0) {
            OnGameOver?.Invoke();
            return;
        }

        // Reset position and movement
        transform.position = origin;
        movePoint.position = origin;
        lastInput = Vector2.right;

        // Reset sprite and animation
        transform.eulerAngles = Vector3.zero;
        animator.Play("Player Idle");
        spriteRenderer.sprite = startSprite;

        // Reset movement
        isPlayerDeath = false;
        OnRestart?.Invoke();
    }

    bool MovePointOverlapCircle(Vector3 direction, LayerMask layer) {
        return Physics2D.OverlapCircle(movePoint.position + direction, 0.25f, layer);
    }

    // Expose move point position
    public void SetMovePointPosition(Vector2 position) {
        movePoint.position = position;
    }
}
