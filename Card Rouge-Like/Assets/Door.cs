using UnityEngine;

public class Door : MonoBehaviour
{
    public enum DoorDirection { Up, Down, Left, Right }
    public DoorDirection doorDirection;
    public Vector3 spawnPosition; // This should be set according to the direction (e.g., Up is +Y)
    public Room4 currentRoom;

    public Door adjacentDoor;
    public Transform oppositeDoorPos;
    public bool adjustPosition;

    private void Start()
    {
        switch (doorDirection)
        {
            case DoorDirection.Up:
                spawnPosition = transform.position + new Vector3(0, 1, 0);
                break;
            case DoorDirection.Down:
                spawnPosition = transform.position + new Vector3(0, -1, 0);
                break;
            case DoorDirection.Left:
                spawnPosition = transform.position + new Vector3(-1, 0, 0);
                break;
            case DoorDirection.Right:
                spawnPosition = transform.position + new Vector3(1, 0, 0);
                break;
        }

        if (adjustPosition && adjacentDoor) 
        {
            switch (doorDirection)
            {
                case DoorDirection.Up:
                    transform.position = adjacentDoor.oppositeDoorPos.position + new Vector3(0, 0, 0);
                    break;
                case DoorDirection.Down:
                    transform.position = adjacentDoor.oppositeDoorPos.position + new Vector3(0, 0, 0);
                    break;
                case DoorDirection.Left:
                    transform.position = adjacentDoor.oppositeDoorPos.position + new Vector3(0, 0, 0);
                    break;
                case DoorDirection.Right:
                    transform.position = adjacentDoor.oppositeDoorPos.position + new Vector3(0, 0, 0);
                    break;
            }
        }


    }
}
