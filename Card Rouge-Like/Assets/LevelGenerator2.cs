using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator2 : MonoBehaviour
{
    public GameObject[] roomPrefabs;    // Array of different room prefabs
    public int maxRooms = 10;           // Max number of rooms
    public Vector2Int gridSize = new Vector2Int(10, 10);  // The size of the grid for room placement

    private Dictionary<Vector2Int, Room2> spawnedRooms = new Dictionary<Vector2Int, Room2>(); // Dictionary to store spawned rooms and their positions

    public GameObject startingRoomPrefab;  // Starting room

    // Directional vectors for room connection
    private Vector2Int[] directions = new Vector2Int[]
    {
        Vector2Int.up,    // Up (north)
        Vector2Int.down,  // Down (south)
        Vector2Int.left,  // Left (west)
        Vector2Int.right  // Right (east)
    };

    void Start()
    {
        GenerateLevel();
    }

    void GenerateLevel()
    {
        // Start with the initial room
        Vector2Int startPosition = Vector2Int.zero;
        SpawnRoom(startingRoomPrefab, startPosition);

        // Generate the rest of the rooms
        for (int i = 0; i < maxRooms - 1; i++)
        {
            // Get a random room
            GameObject randomRoomPrefab = roomPrefabs[Random.Range(0, roomPrefabs.Length)];

            // Get a random spawned room and attempt to spawn a new one next to it
            Room2 currentRoom = GetRandomSpawnedRoom();
            Vector2Int newRoomPosition = GetRandomAdjacentPosition(currentRoom.gridPosition, randomRoomPrefab);

            // Make sure the new room position is within bounds and hasn't been used yet
            if (!spawnedRooms.ContainsKey(newRoomPosition) && IsWithinGrid(newRoomPosition))
            {
                SpawnRoom(randomRoomPrefab, newRoomPosition);
            }
            else
            {
                i--;  // Retry if the room couldn't be placed
            }
        }
    }

    void SpawnRoom(GameObject roomPrefab, Vector2Int position)
    {
        // Get the Room2 component to access the room size
        Room2 roomComponent = roomPrefab.GetComponent<Room2>();

        // Use room size defined in the Room2 script
        Vector2 roomSize = roomComponent.roomSize;

        // Adjust room spacing based on the room size
        Vector3 spawnPosition = new Vector3(position.x * roomSize.x, position.y * roomSize.y, 0);

        GameObject roomInstance = Instantiate(roomPrefab, spawnPosition, Quaternion.identity);
        roomComponent = roomInstance.GetComponent<Room2>();  // Get the component after instantiating
        roomComponent.gridPosition = position;

        spawnedRooms.Add(position, roomComponent);
        ConnectAdjacentRooms(position);  // Connect this room to its neighbors
    }

    Room2 GetRandomSpawnedRoom()
    {
        List<Room2> roomList = new List<Room2>(spawnedRooms.Values);
        return roomList[Random.Range(0, roomList.Count)];
    }

    Vector2Int GetRandomAdjacentPosition(Vector2Int roomPosition, GameObject roomPrefab)
    {
        // Get the Room2 component to access the room size
        Room2 roomComponent = roomPrefab.GetComponent<Room2>();
        Vector2 roomSize = roomComponent.roomSize;

        Vector2Int direction = directions[Random.Range(0, directions.Length)];

        // Multiply by the room size to ensure proper spacing for rooms of varying sizes
        return roomPosition + new Vector2Int(Mathf.RoundToInt(direction.x * roomSize.x), Mathf.RoundToInt(direction.y * roomSize.y));
    }

    void ConnectAdjacentRooms(Vector2Int roomPosition)
    {
        Room2 room = spawnedRooms[roomPosition];

        foreach (Vector2Int dir in directions)
        {
            Vector2Int adjacentPosition = roomPosition + dir;

            if (spawnedRooms.ContainsKey(adjacentPosition))
            {
                Room2 adjacentRoom = spawnedRooms[adjacentPosition];

                // Connect rooms in both directions
                room.ConnectRoom(dir);
                adjacentRoom.ConnectRoom(-dir);
            }
        }
    }

    bool IsWithinGrid(Vector2Int position)
    {
        return position.x >= -gridSize.x / 2 && position.x <= gridSize.x / 2 &&
               position.y >= -gridSize.y / 2 && position.y <= gridSize.y / 2;
    }
}
