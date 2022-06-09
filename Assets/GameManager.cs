using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private PlayerManager playerManager;

    // Managers access
    public PlayerManager PlayerManager { get => playerManager; private set => playerManager = value; }
}
