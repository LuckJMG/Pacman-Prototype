using UnityEngine;

public class GhostManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    public GameManager GameManager { get => gameManager; private set => gameManager = value; }
}
