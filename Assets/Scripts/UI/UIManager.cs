using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour {
    [Header("Game Objects")]
    [SerializeField] TextMeshProUGUI scoreText;
    Image[] livesImages = new Image[3];

    void Awake() {
        livesImages = transform.GetChild(1).GetComponentsInChildren<Image>();
    }

    void Start() {
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();

        // Subscribe to events
        scoreManager.OnGetPoint += OnGetPoint;
        scoreManager.OnLoseLive += OnLoseLive;
    }

    // Update score display
    void OnGetPoint(int score) {
        scoreText.text = "Score: " + score.ToString();
    }

    // Update lives display
    void OnLoseLive(int lives) {
        if (livesImages[lives] != null) {
            if (lives >= 0) livesImages[lives].enabled = false;
        }
    }
}
