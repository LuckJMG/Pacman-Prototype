using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour {
    // Fields
    int finalScore;
    bool pause;
    bool gameOver;

    // Game Objects
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject gameOverScreen;

    // Events
    static public Action<bool> OnPause;
    static public Action OnGameOver;

    void Awake() {
        // Restart static events
        OnPause = null;
        OnGameOver = null;
    }

    void Start() {
        // Subscribe to events
        FindObjectOfType<ScoreManager>().OnGetPoint += score => finalScore = score;
        FindObjectOfType<PlayerController>().OnGameOver += () => GameOver();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            pause = pause ? false : true;
            pauseMenu.SetActive(pause);
            OnPause?.Invoke(pause);
        }

        if (Input.GetKeyDown(KeyCode.Return) && gameOver) {
            SceneManager.LoadScene("MainMenu");
        }
    }

    void GameOver() {
        // Display game over
        TextMeshProUGUI textScore = gameOverScreen.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        textScore.text = "Your Score: " + finalScore.ToString();
        gameOverScreen.SetActive(true);
        OnGameOver?.Invoke();
        gameOver = true;
    }
}
