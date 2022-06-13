using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Collections;

public class ScoreManager : MonoBehaviour {
    // Fields
    int score;
    int lives = 3;
    int pointScore = 10;
    int powerUpScore = 50;
    int ghostScore = 200;

    // Events
    public static event Action OnPlayerDeath;
    public static event Action OnPowerUp;
    public static event Action OnEatGhost;
    public static event Action<int> OnGetPoint;
    public static event Action<int> OnLoseLive;

    [Header("Assets")]
    [SerializeField] Tilemap points;
    [SerializeField] AudioClip pacmanChomp;
    [SerializeField] AudioClip pacmanDeath;

    // Components
    Animator animator;
    AudioSource audioSource;

    void Awake() {
        // Get components
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
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

        audioSource.Stop();
        audioSource.PlayOneShot(pacmanDeath);

        OnPlayerDeath?.Invoke();

        animator.enabled = true;
        animator.Play("Player Death");
    }
}
