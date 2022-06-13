using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour {
    [Header("Game Objects")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Image[] livesImages;

    void Start() {
        // Subscribe to events
        ScoreManager.OnGetPoint += OnGetPoint;
        ScoreManager.OnLoseLive += OnLoseLive;
    }

    // Update score display
    void OnGetPoint(int score) {
        scoreText.text = "Score: " + score.ToString();
    }

    // Update lives display
    void OnLoseLive(int lives) {
        if (lives >= 0) livesImages[lives].enabled = false;
    }
}
