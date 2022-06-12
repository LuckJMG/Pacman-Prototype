using UnityEngine;
using UnityEngine.Tilemaps;
using System;

public class ScoreManager : MonoBehaviour {
    // Fields
    [HideInInspector] public int lives = 3;
    [HideInInspector] public int score;

    // Events
    public event Action OnPlayerDeath;
    public event Action<int> OnGetPoint;
    public event Action<int> OnLoseLive;

    [Header("Assets")]
    [SerializeField] Tilemap points;
    [SerializeField] AudioClip pacmanChomp;
    [SerializeField] AudioClip pacmanDeath;

    // Components
    Animator animator;
    AudioSource audioSource;

    void Start() {
        // Get components
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        if (points.HasTile(Vector3Int.FloorToInt(transform.position))) GetPoint();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Ghost") Death();
    }

    void GetPoint() {
        points.SetTile(Vector3Int.FloorToInt(transform.position), null);
        score += 10;
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
