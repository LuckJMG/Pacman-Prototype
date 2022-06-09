using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    // Fields
    private bool isPlayerDeath = false;
    private int lives = 3;
    private int score;

    // Properties
    public bool IsPlayerDeath { get => isPlayerDeath; set => isPlayerDeath = value; }
    public int Lives { get => lives; private set => lives = value; }
    public int Score { get => score; private set => score = value; }

    [Header("Assets")]
    [SerializeField] private Tilemap points;
    [SerializeField] private AudioClip pacmanChomp;
    [SerializeField] private AudioClip pacmanDeath;

    // Components
    private PlayerManager playerManager;
    private AudioSource audioSource;
    private Animator animator;

    private void Start()
    {
        // Get components
        playerManager = GetComponent<PlayerManager>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (points.HasTile(Vector3Int.FloorToInt(transform.position)))
        {
            // Collision with a point
            points.SetTile(Vector3Int.FloorToInt(transform.position), null);
            score += 10;
            audioSource.PlayOneShot (pacmanChomp);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Death
        if (other.gameObject.tag == "Ghost")
        {
            lives -= 1;
            audioSource.Stop();
            audioSource.PlayOneShot (pacmanDeath);
            isPlayerDeath = true;
            animator.enabled = true;
            animator.Play("Player Death");
        }
    }
}
