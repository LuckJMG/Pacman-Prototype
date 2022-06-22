using UnityEngine;
using System.Collections;
using TMPro;

public class Vulnerable : MonoBehaviour {
    [Header("Fields")]
    [Range(.1f, 20f)] [SerializeField] float timeFirstPhase = 5f;
    [Range(.1f, 20f)] [SerializeField] float timeSecondPhase = 3f;
    [Range(.1f, 10f)] [SerializeField] float slowdown = 1.5f;
    [HideInInspector] public bool isVulnerable;
    [HideInInspector] public static int ghostEaten;
    [HideInInspector] public static float totalTimeVulnerability;
    Vector2 origin;
    IEnumerator vulnerable;

    // Components
    GhostController ghostController;
    Animator animator;

    [Header("Game Objects")]
    [SerializeField] GameObject ghostEatenPointsPrefab;
    Transform movePoint;

    void Awake() {
        // Get components
        ghostController = GetComponent<GhostController>();
        animator = GetComponent<Animator>();

        // Find game objects
        movePoint = transform.GetChild(0).GetComponent<Transform>();
    }

    void Start() {
        // Subscribe to events
        FindObjectOfType<ScoreManager>().OnPowerUp += () => {
            vulnerable = OnPowerUp();
            StartCoroutine(vulnerable);
        };

        // Initialize
        totalTimeVulnerability = timeFirstPhase + timeSecondPhase;
        origin = transform.position;
    }

    IEnumerator OnPowerUp() {
        // First phase
        isVulnerable = true;
        animator.Play("Vulnerable First Phase");
        ghostController.speed /= slowdown;
        yield return new WaitForSeconds(timeFirstPhase);

        // Second phase
        animator.Play("Vulnerable Second Phase");
        yield return new WaitForSeconds(timeSecondPhase);

        // End of power up
        ghostController.speed *= slowdown;
        ghostEaten = 0;
        animator.Play("Side");
        isVulnerable = false;
    }

    void DisplayPoints() {
        float pointsDurationOnScreen = 1f;

        // Instantiate points display
        GameObject ghostEatenPointsInstance = Instantiate(ghostEatenPointsPrefab, transform.position, Quaternion.identity);
        TextMeshPro ghostEatenPointsText = ghostEatenPointsInstance.GetComponent<TextMeshPro>();
        ghostEatenPointsText.text = (ghostEaten * ScoreManager.ghostScore).ToString();

        Destroy(ghostEatenPointsInstance, pointsDurationOnScreen);
    }

    public void OnEaten() {
        // Update points
        ghostEaten += 1;
        DisplayPoints();

        // Reset state
        if (vulnerable != null) StopCoroutine(vulnerable);
        animator.Play("Side");
        isVulnerable = false;
        ghostController.speed *= slowdown;
        transform.position = origin;
        movePoint.position = origin;
    }
}
