using UnityEngine;
using System.Collections.Generic;

public class GhostController : MonoBehaviour,IMovePointDependable {
    [Header("Fields")]
    [Range(0f, 10f)] public float speed = 4f;
    bool isPlayerDeath;
    bool pause;
    int direction;
    int lastDirection;
    Vector2 origin;

    [Header("Layers")]
    [SerializeField] LayerMask wall;

    // Components
    SpriteRenderer spriteRenderer;
    Animator animator;

    // Game objects
    Transform movePoint;

    void Awake() {
        // Get components
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Find game objects
        movePoint = transform.GetChild(0).GetComponent<Transform>();
    }

    void Start() {
        // Subscribe to events
        GameManager.OnPause += isPause => pause = isPause;
        ScoreManager.OnPlayerDeath += () => isPlayerDeath = true;
        PlayerController.OnRestart += () => isPlayerDeath = false;

        // Initialize
        origin = transform.position;
        movePoint.parent = null;
    }

    void Update() {
        transform.position = Vector2.MoveTowards(transform.position, movePoint.position, speed * Time.deltaTime);

        if (!pause) {
            if (isPlayerDeath) Restart();
            else if (Vector2.Distance(transform.position, movePoint.position) <= 0) {
                animator.enabled = true;
                spriteRenderer.enabled = true;

                RandomDirection();

                animator.SetInteger("direction", direction);
                if (direction == 1) transform.localScale = new Vector3(-1, 1, 1);
                else if (direction == 2) transform.localScale = Vector3.one;
            }
        }
    }

    // Selects a valid random direction
    void RandomDirection() {
        // Reset directions
        List<int> possibleDirections = new List<int>{};

        // Check available possible directions
        if (!Physics2D.OverlapCircle(movePoint.position + Vector3.right, 0.25f, wall)) possibleDirections.Add(1);
        if (!Physics2D.OverlapCircle(movePoint.position + Vector3.left, 0.25f, wall)) possibleDirections.Add(2);
        if (!Physics2D.OverlapCircle(movePoint.position + Vector3.up, 0.25f, wall)) possibleDirections.Add(3);
        if (!Physics2D.OverlapCircle(movePoint.position + Vector3.down, 0.25f, wall)) possibleDirections.Add(4);

        // If there is no other option, go backwards
        if (possibleDirections.Count > 1) possibleDirections.Remove(lastDirection);

        // Select a path in an intersection
        direction = possibleDirections[Random.Range(0, possibleDirections.Count)];

        // Check if future position is available
        if (direction == 1 && !Physics2D.OverlapCircle(movePoint.position + Vector3.right, 0.25f, wall)) {
            lastDirection = 2;
            movePoint.position = new Vector3(transform.position.x + 1, transform.position.y, 0);
        }
        else if (direction == 2 && !Physics2D.OverlapCircle(movePoint.position + Vector3.left, 0.25f, wall)) {
            lastDirection = 1;
            movePoint.position = new Vector3(transform.position.x - 1, transform.position.y, 0);
        }
        else if (direction == 3 && !Physics2D.OverlapCircle(movePoint.position + Vector3.up, 0.25f, wall)) {
            lastDirection = 4;
            movePoint.position = new Vector3(transform.position.x, transform.position.y + 1, 0);
        }
        else if (direction == 4 && !Physics2D.OverlapCircle(movePoint.position + Vector3.down, 0.25f, wall)) {
            lastDirection = 3;
            movePoint.position = new Vector3(transform.position.x, transform.position.y - 1, 0);
        }
    }

    void Restart() {
            // Reset position
            transform.position = origin;
            movePoint.position = origin;

            // Reset animation
            animator.Play("Side");
            transform.localScale = Vector3.one;

            //Disappear in player death
            animator.enabled = false;
            spriteRenderer.enabled = false;
    }

    // Expose move point position
    public void SetMovePointPosition(Vector2 position) {
        movePoint.position = position;
    }
}
