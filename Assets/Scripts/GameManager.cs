using UnityEngine;
using System;

public class GameManager : MonoBehaviour {
    [SerializeField] GameObject pauseMenu;
    bool pause;

    static public Action<bool> OnPause;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            pause = pause ? false : true;
            pauseMenu.SetActive(pause);
            OnPause?.Invoke(pause);
        }
    }
}
