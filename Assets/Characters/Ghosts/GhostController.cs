using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour,IMovePointDependable
{
    [Header("Fields")]
    [Range(0f, 10f)] [SerializeField] private float speed = 4f;
    // [SerializeField] private float timer = 60;
    // [HideInInspector] public bool Vulnerable = false;
    private int direction;
    private int lastDirection;
    private Vector3 origin;
    private List<int> possibleDirections = new List<int>{};

    [Header("Layers")]
    [SerializeField] private LayerMask wall;

    [Header("Game Objects")]
    [SerializeField] private PlayerManager playerManager;
    [SerializeField] private Transform movePoint;

    // Components
    private GhostManager ghostManager;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private void Start()
    {
        // Get components
        ghostManager = GetComponent<GhostManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Start attributes;
        origin = transform.position;
        movePoint.parent = null;
    }

    private void Update()
    {
        bool isPlayerDeath = ghostManager.GameManager.PlayerManager.ScoreManager.IsPlayerDeath;

        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, speed * Time.deltaTime);

        /*
        if (playerManager.PowerUp)
        {
            Vulnerable = true;
        }
        */

        if (isPlayerDeath)
        {
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
        else if (Vector3.Distance(transform.position, movePoint.position) <= 0)
        {
            animator.enabled = true;
            spriteRenderer.enabled = true;

            RandomDirection();

            /*
            // Manage animation
            if (Vulnerable)
            {
                if (timer > 0)
                {
                    animator.Play("Vulnerable First Phase");
                    timer -= 1;
                }
                else
                {
                    Vulnerable = false;
                    animator.Play("Side");
                    // Set timer main relation with power up player
                }
            }
            */
            animator.SetInteger("direction", direction);
            if (direction == 1) transform.localScale = new Vector3(-1, 1, 1);
            else if (direction == 2) transform.localScale = Vector3.one;
        }

    }

    // Selects a valid random direction
    private void RandomDirection()
    {
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
        if (direction == 1 && !Physics2D.OverlapCircle(movePoint.position + Vector3.right, 0.25f, wall))
        {
            lastDirection = 2;
            movePoint.position = new Vector3(transform.position.x + 1, transform.position.y, 0);
        }
        else if (direction == 2 && !Physics2D.OverlapCircle(movePoint.position + Vector3.left, 0.25f, wall))
        {
            lastDirection = 1;
            movePoint.position = new Vector3(transform.position.x - 1, transform.position.y, 0);
        }
        else if (direction == 3 && !Physics2D.OverlapCircle(movePoint.position + Vector3.up, 0.25f, wall))
        {
            lastDirection = 4;
            movePoint.position = new Vector3(transform.position.x, transform.position.y + 1, 0);
        }
        else if (direction == 4 && !Physics2D.OverlapCircle(movePoint.position + Vector3.down, 0.25f, wall))
        {
            lastDirection = 3;
            movePoint.position = new Vector3(transform.position.x, transform.position.y - 1, 0);
        }
    }

    // Expose move point position
    public void SetMovePointPosition(Vector3 position) { movePoint.position = position; }
}
