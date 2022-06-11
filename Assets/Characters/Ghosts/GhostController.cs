using UnityEngine;
using System.Collections.Generic;

public class GhostController : MonoBehaviour,IMovePointDependable {
    GhostManager ghostManager;

    [Header("Fields")]
    [Range(0f, 10f)] [SerializeField] float speed = 4f;
    bool isPlayerDeath;
    int direction;
    int lastDirection;
    Vector3 origin;
    List<int> possibleDirections = new List<int>{};

    [Header("Layers")]
    [SerializeField] LayerMask wall;

    [Header("Game Objects")]
    [SerializeField] Transform movePoint;


    void Start() {
        ghostManager = GetComponent<GhostManager>();

        // Subscribe to events
        ghostManager.gameManager.playerManager.scoreManager.OnPlayerDeath += () => isPlayerDeath = true;
        ghostManager.gameManager.playerManager.playerController.OnRestart += () => isPlayerDeath = false;

        // Start attributes;
        origin = transform.position;
        movePoint.parent = null;
    }


    void Update() {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, speed * Time.deltaTime);

        if (isPlayerDeath) Restart();
        else if (Vector3.Distance(transform.position, movePoint.position) <= 0) {
            ghostManager.animator.enabled = true;
            ghostManager.spriteRenderer.enabled = true;

            RandomDirection();

            ghostManager.animator.SetInteger("direction", direction);
            if (direction == 1) transform.localScale = new Vector3(-1, 1, 1);
            else if (direction == 2) transform.localScale = Vector3.one;
        }
    }


    // Selects a valid random direction
    void RandomDirection() {
        // Reset directions
        possibleDirections.Clear();

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
            ghostManager.animator.Play("Side");
            transform.localScale = Vector3.one;

            //Disappear in player death
            ghostManager.animator.enabled = false;
            ghostManager.spriteRenderer.enabled = false;
    }


    // Expose move point position
    public void SetMovePointPosition(Vector3 position) => movePoint.position = position;
}
