using UnityEngine;
using System;

public class PlayerController : MonoBehaviour, IMovePointDependable {
    PlayerManager playerManager;

    [Header("Fields")]
    [Range(0f, 10f)] [SerializeField] float speed = 4f;
    bool isPlayerDeath;
    Vector2 lastInput;
    Vector3 origin;

    // Events
    public event Action OnRestart;

    [Header("Assets")]
    [SerializeField] Sprite startSprite;

    [Header("Layers")]
    [SerializeField] LayerMask wall;
    [SerializeField] LayerMask separator;

    [Header("Game Objects")]
    [SerializeField] Transform movePoint;


    void Start() {
        playerManager = GetComponent<PlayerManager>();

        // Subscribe to events
        playerManager.scoreManager.OnPlayerDeath += () => isPlayerDeath = true;

        // Initialize player movement
        origin = transform.position;
        movePoint.parent = null;
    }


    void Update() {
        if (!isPlayerDeath) PlayerMovement();
    }


    void PlayerMovement() {
        // Move towards move point
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, speed * Time.deltaTime);

        DetermineLastInput();

        // Move MovePoint towards last input
        if (Vector3.Distance(transform.position, movePoint.position) <= 0) {
            if (lastInput == Vector2.right && !MovePointOverlapCircle(Vector3.right, wall) && !MovePointOverlapCircle(Vector3.right, separator)) {
                movePoint.position = new Vector3(transform.position.x + 1, transform.position.y, 0);
                transform.eulerAngles = Vector3.zero;
            }
            else if (lastInput == Vector2.left && !MovePointOverlapCircle(Vector3.left, wall) && !MovePointOverlapCircle(Vector3.left, separator)) {
                movePoint.position = new Vector3(transform.position.x - 1, transform.position.y, 0);
                transform.eulerAngles = new Vector3(0, 0, 180);
            }
            else if (lastInput == Vector2.up && !MovePointOverlapCircle(Vector3.up, wall) && !MovePointOverlapCircle(Vector3.up, separator)) {
                movePoint.position = new Vector3(transform.position.x, transform.position.y + 1, 0);
                transform.eulerAngles = new Vector3(0, 0, 90);
            }
            else if (lastInput == Vector2.down && !MovePointOverlapCircle(Vector3.down, wall) && !MovePointOverlapCircle(Vector3.down, separator)) {
                movePoint.position = new Vector3(transform.position.x, transform.position.y - 1, 0);
                transform.eulerAngles = new Vector3(0, 0, 270);
            }
            else {
                movePoint.position = new Vector3(transform.position.x, transform.position.y, 0);
                playerManager.animator.enabled = false;
            }
        }
    }


    void DetermineLastInput() {
        if (Input.GetAxisRaw("Horizontal") == 1 && !MovePointOverlapCircle(Vector3.right, wall) && !MovePointOverlapCircle(Vector3.right, separator)) {
            lastInput = Vector2.right;
            playerManager.animator.enabled = true;
        }
        else if (Input.GetAxisRaw("Horizontal") == -1 && !MovePointOverlapCircle(Vector3.left, wall) && !MovePointOverlapCircle(Vector3.left, separator)) {
            lastInput = Vector2.left;
            playerManager.animator.enabled = true;
        }
        else if (Input.GetAxisRaw("Vertical") == 1 && !MovePointOverlapCircle(Vector3.up, wall) && !MovePointOverlapCircle(Vector3.up, separator)) {
            lastInput = Vector2.up;
            playerManager.animator.enabled = true;
        }
        else if (Input.GetAxisRaw("Vertical") == -1 && !MovePointOverlapCircle(Vector3.down, wall) && !MovePointOverlapCircle(Vector3.down, separator)) {
            lastInput = Vector2.down;
            playerManager.animator.enabled = true;
        }
    }


    void Reset() {
        // Reset position and movement
        transform.position = origin;
        movePoint.position = origin;
        lastInput = Vector3.zero;

        // Reset sprite and animation
        playerManager.animator.enabled = false;
        transform.eulerAngles = Vector3.zero;
        playerManager.spriteRenderer.sprite = startSprite;
        playerManager.animator.Play("Player Idle");

        playerManager.audioSource.Play();

        // Reset movement
        isPlayerDeath = false;
        OnRestart?.Invoke();
    }

    bool MovePointOverlapCircle(Vector3 direction, LayerMask layer) {
        return Physics2D.OverlapCircle(movePoint.position + direction, 0.25f, layer);
    }

    // Expose move point position
    public void SetMovePointPosition(Vector3 position) => movePoint.position = position;
}
