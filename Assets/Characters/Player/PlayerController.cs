using UnityEngine;
using System;

public class PlayerController : MonoBehaviour, IMovePointDependable {
    [Header("Fields")]
    [Range(0f, 10f)] [SerializeField] float speed = 4f;
    bool isPlayerDeath;
    Vector2 lastInput;
    Vector3 origin;

    // Events
    public static event Action OnRestart;

    [Header("Assets")]
    [SerializeField] Sprite startSprite;

    [Header("Layers")]
    [SerializeField] LayerMask wall;
    [SerializeField] LayerMask separator;

    // Components
    SpriteRenderer spriteRenderer;
    Animator animator;
    AudioSource audioSource;

    [Header("Game Objects")]
    [SerializeField] Transform movePoint;

    void Awake() {
        // Get components
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start() {
        // Subscribe to events
        ScoreManager.OnPlayerDeath += () => isPlayerDeath = true;

        // Initialize player movement
        origin = transform.position;
        movePoint.parent = null;
    }

    void Update() {
        if (!isPlayerDeath) PlayerMovement();
    }

    void PlayerMovement() {
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
        // Reset position and movement
        transform.position = origin;
        movePoint.position = origin;
        lastInput = Vector2.zero;

        // Reset sprite and animation
        animator.enabled = false;
        transform.eulerAngles = Vector3.zero;
        spriteRenderer.sprite = startSprite;
        animator.Play("Player Idle");

        audioSource.Play();

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
