using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {
    public void MainMenuButton() {
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartButton() {
        SceneManager.LoadScene("Level");
    }

    public void QuitButton() {
        Application.Quit();
    }
}
