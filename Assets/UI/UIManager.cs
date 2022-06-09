using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    // Fields
    private int previousLives;

    [Header("Game Objects")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image[] livesImages;

    private void Start()
    {
        previousLives = gameManager.PlayerManager.ScoreManager.Lives;
    }

    private void Update()
    {
        int lives = gameManager.PlayerManager.ScoreManager.Lives;
        int score = gameManager.PlayerManager.ScoreManager.Score;

        // Update lives display
        if (previousLives != lives)
        {
            livesImages[lives].enabled = false;
            previousLives = lives;
        }

        // Update score display
        scoreText.text = "Score: " + score.ToString();
    }
}
