using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {
    [Header("Assets")]
    [SerializeField] AudioClip siren;
    [SerializeField] AudioClip powerUp;
    [SerializeField] AudioClip scaredGhosts;

    // Components
    AudioSource audioSource;

    void Awake() {
        // Get components
        audioSource = GetComponent<AudioSource>();
    }

    void Start() {
        // Subscribe to events
        ScoreManager.OnPlayerDeath += () => audioSource.Stop();
        ScoreManager.OnPowerUp += () => StartCoroutine(OnPowerUp());
        PlayerController.OnRestart += () => audioSource.Play();
    }

    IEnumerator OnPowerUp() {
        audioSource.PlayOneShot(powerUp);

        audioSource.clip = scaredGhosts;
        audioSource.Play();
        yield return new WaitForSeconds(Vulnerable.totalTimeVulnerability);

        audioSource.clip = siren;
        audioSource.Play();
    }
}
