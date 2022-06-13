using UnityEngine;
using System.Collections;

public class Vulnerable : MonoBehaviour {
    [Header("Fields")]
    [Range(.1f, 20f)] [SerializeField] float timeFirstPhase = 5f;
    [Range(.1f, 20f)] [SerializeField] float timeSecondPhase = 3f;
    [HideInInspector] public bool isVulnerable;
    [HideInInspector] public static int ghostEaten;
    Vector2 origin;
    IEnumerator currentCoroutine;

    // Components
    Animator animator;

    [Header("Game Objects")]
    [SerializeField] Transform movePoint;

    void Awake() {
        animator = GetComponent<Animator>();
    }

    void Start() {
        // Subscribe to events
        ScoreManager.OnPowerUp += () => {
            currentCoroutine = OnPowerUp();
            StartCoroutine(currentCoroutine);
        };

        origin = transform.position;
    }

    IEnumerator OnPowerUp() {
        isVulnerable = true;
        animator.Play("Vulnerable First Phase");
        yield return new WaitForSeconds(timeFirstPhase);
        animator.Play("Vulnerable Second Phase");
        yield return new WaitForSeconds(timeSecondPhase);
        animator.Play("Side");
        isVulnerable = false;
    }

    public void OnEaten() {
        ghostEaten += 1;
        if (currentCoroutine != null) StopCoroutine(currentCoroutine);
        animator.Play("Side");
        isVulnerable = false;
        transform.position = origin;
        movePoint.position = origin;
    }
}
