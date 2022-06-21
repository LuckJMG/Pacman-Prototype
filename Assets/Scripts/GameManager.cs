using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

public class GameManager : MonoBehaviour {
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject gameOverScreen;
    bool pause;

    static public Action<bool> OnPause;
    static public Action OnGameOver;

    void Awake() {
        // Restart static events
        OnPause = null;
        OnGameOver = null;
    }

    void Start() {
        // Subscribe to events
        PlayerController.OnGameOver += () => StartCoroutine(GameOver());
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            pause = pause ? false : true;
            pauseMenu.SetActive(pause);
            OnPause?.Invoke(pause);
        }
    }

    IEnumerator GameOver() {
        float gameOverScreenTime = 2f;

        // Display game over
        gameOverScreen.SetActive(true);
        OnGameOver?.Invoke();
        yield return new WaitForSeconds(gameOverScreenTime);

        SceneManager.LoadScene("MainMenu");
    }
}
