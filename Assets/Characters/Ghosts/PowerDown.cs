using System.ComponentModel;
using UnityEngine;

public class PowerDown : MonoBehaviour
{
    // Fields
    private bool powerDown = false;

    // Components
    private GhostManager ghostManager;

    private void Start()
    {
        ghostManager = GetComponent<GhostManager>();
    }

    private void Update()
    {
        // powerDown = ghostManager.PowerDown;
    }
}
