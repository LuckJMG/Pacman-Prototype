using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
    public void StartButton() {
        SceneManager.LoadScene("Level");
    }

    public void QuitButton() {
        Application.Quit();
    }
}
