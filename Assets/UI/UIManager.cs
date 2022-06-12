using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour {
    [Header("Game Objects")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Image[] livesImages;
    ScoreManager scoreManager;

    void Start() {
        // Find game objects
        scoreManager = FindObjectOfType<ScoreManager>();

        // Subscribe to events
        if (scoreManager != null) {
            scoreManager.OnGetPoint += OnGetPoint;
            scoreManager.OnLoseLive += OnLoseLive;
        }
    }

    // Update score display
    void OnGetPoint(int score) => scoreText.text = "Score: " + score.ToString();

    // Update lives display
    void OnLoseLive(int lives) {
        if (lives >= 0) livesImages[lives].enabled = false;
    }
}
