using UnityEngine;

public class WarpManager : MonoBehaviour {
    // Constructors
    enum DirectionOffset {up = 1, down = -1, right = 2, left = -2}

    [Header("Fields")]
    [SerializeField] DirectionOffset directionOffset;
    Vector2 destinationCoordinates;

    [Header("Game Objects")]
    [SerializeField] WarpManager destination;

    void Start() {
        destinationCoordinates = CalculateDestinationCoordinates(destination);
    }

    void OnTriggerEnter2D(Collider2D other) {
        // If has a move point, teleport the move point
        IMovePointDependable iMovePointDependable = other.GetComponent<IMovePointDependable>();
        if (iMovePointDependable != null) iMovePointDependable.SetMovePointPosition(destinationCoordinates);

        // Teleport object
        other.transform.position = destinationCoordinates;
    }

    Vector2 CalculateDestinationCoordinates(WarpManager destination) {
        // Variables
        float xDestination;
        float yDestination;

        // Check if destination is in the vertical or horizontal axis
        if (directionOffset == DirectionOffset.up || directionOffset == DirectionOffset.down) {
            xDestination = destination.transform.position.x;
            yDestination = destination.transform.position.y + (int)destination.directionOffset;
        }
        else {
            xDestination = destination.transform.position.x + (int)destination.directionOffset / 2;
            yDestination = destination.transform.position.y;
        }

        return new Vector2(xDestination, yDestination);
    }
}
