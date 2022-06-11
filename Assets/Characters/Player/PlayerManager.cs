using UnityEngine;

public class PlayerManager : MonoBehaviour {
    public GameManager gameManager;

    // Scripts
    [HideInInspector] public PlayerController playerController;
    [HideInInspector] public ScoreManager scoreManager;

    // Components
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public AudioSource audioSource;
    [HideInInspector] public Animator animator;


    void Awake() {
        // Get scripts
        playerController = GetComponent<PlayerController>();
        scoreManager = GetComponent<ScoreManager>();

        // Get components
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
}
