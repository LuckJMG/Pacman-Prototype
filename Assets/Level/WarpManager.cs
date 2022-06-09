using UnityEngine;

public class WarpManager : MonoBehaviour
{
    // Constructors
    // private enum DirectionOffset {up, down, right, left}

    [Header("Fields")]
    [SerializeField] private int direction;
    // [SerializeField] private DirectionOffset directionOffset;
    private Vector3 destinationCoordinates;

    [Header("Game Objects")]
    [SerializeField] private WarpManager destination;

    private void Start()
    {
        float xDestination = destination.transform.position.x + destination.direction;
        float yDestination = destination.transform.position.y;

        destinationCoordinates = new Vector3(xDestination, yDestination, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IMovePointDependable iMovePointDependable = other.GetComponent<IMovePointDependable>();
        if (iMovePointDependable != null) { iMovePointDependable.SetMovePointPosition(destinationCoordinates); }
        other.transform.position = destinationCoordinates;
    }
}
