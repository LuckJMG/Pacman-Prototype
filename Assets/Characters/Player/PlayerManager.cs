using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    // Components
    public PlayerController PlayerController { get; private set; }
    public ScoreManager ScoreManager { get; private set; }

    private void Start()
    {
        PlayerController = GetComponent<PlayerController>();
        ScoreManager = GetComponent<ScoreManager>();
    }
}
