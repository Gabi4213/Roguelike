using UnityEngine;

public class Door : MonoBehaviour
{
    public enum DoorDirection { Up, Down, Left, Right }
    public DoorDirection doorDirection;
    public Vector3 spawnPosition; // This should be set according to the direction (e.g., Up is +Y)

    private void Awake()
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
    }
}
