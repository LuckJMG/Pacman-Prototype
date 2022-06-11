using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public GameManager gameManager;

    // Components
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Animator animator;


    void Awake() {
        // Get Components
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
}
