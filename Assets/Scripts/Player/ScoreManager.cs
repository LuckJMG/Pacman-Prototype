using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class ScoreManager : MonoBehaviour {
    // Fields
    int score;
    int pointScore = 10;
    int powerUpScore = 50;
    static public int lives;
    [HideInInspector] public static int ghostScore = 200;

    // Events
    public static Action OnPlayerDeath;
    public static Action OnPowerUp;
    public static Action OnEatGhost;
    public static Action<int> OnGetPoint;
    public static Action<int> OnLoseLive;

    [Header("Assets")]
    [SerializeField] Tilemap points;
    [SerializeField] AudioClip pacmanChomp;
    [SerializeField] AudioClip pacmanDeath;
    [SerializeField] AudioClip ghostEaten;

    // Components
    Animator animator;
    AudioSource audioSource;

    void Awake() {
        // Get components
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        // Restart static events
        OnPlayerDeath = null;
        OnPowerUp = null;
        OnEatGhost = null;
        OnGetPoint = null;
        OnLoseLive = null;

        // Initialize fields
        lives = 3;
    }

    void Update() {
        if (points.HasTile(Vector3Int.FloorToInt(transform.position))) GetPoint();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Ghost") GhostCollision(other);
        if (other.gameObject.tag == "PowerUp") PowerUp(other);
    }

    void GhostCollision(Collider2D ghostCollider) {
        Vulnerable ghost = ghostCollider.GetComponent<Vulnerable>();
        OnEatGhost += ghost.OnEaten;

        if (ghost != null) {
            if (ghost.isVulnerable) {
                OnEatGhost?.Invoke();
                audioSource.PlayOneShot(ghostEaten);
                score += ghostScore * Vulnerable.ghostEaten;
                OnGetPoint?.Invoke(score);
            }
            else Death();
        }

        OnEatGhost -= ghost.OnEaten;
    }

    void PowerUp(Collider2D powerUpCollider) {
        score += powerUpScore;
        OnGetPoint?.Invoke(score);
        OnPowerUp?.Invoke();
        Destroy(powerUpCollider.gameObject);
    }

    void GetPoint() {
        points.SetTile(Vector3Int.FloorToInt(transform.position), null);
        score += pointScore;
        OnGetPoint?.Invoke(score);
        audioSource.PlayOneShot(pacmanChomp);
    }

    void Death() {
        lives -= 1;
        OnLoseLive?.Invoke(lives);

        audioSource.PlayOneShot(pacmanDeath);

        OnPlayerDeath?.Invoke();

        animator.Play("Player Death");
        animator.enabled = true;
    }
}
