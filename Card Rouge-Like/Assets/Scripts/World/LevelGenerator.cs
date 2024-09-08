using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] roomPrefabs; // Array of room prefabs to instantiate
    public List<float> roomPrefabChances; // List of probabilities for each prefab (0 to 100%)

    public Vector2[] roomCountsPerLevel; // Array to store min and max room counts per level
    public int currentLevel = 0; // The current dungeon level
    private List<Room> placedRooms = new List<Room>(); // List of placed rooms to track them
    private HashSet<Vector3Int> occupiedPositions = new HashSet<Vector3Int>(); // Track positions of placed rooms using integer grid

    public int maxRetryAttempts = 10; // Max number of retries for generating the dungeon
    private int currentAttemps;

    void Start()
    {
        GenerateDungeonWithRetries();
    }

    // Retry mechanism for dungeon generation
    void GenerateDungeonWithRetries()
    {
        currentAttemps = 0;
        bool success = false;

        // Retry until successful or max attempts reached
        while (!success && currentAttemps < maxRetryAttempts)
        {
            ClearPreviousGeneration(); // Clear the previous generation
            GenerateDungeon();

            if (CheckForOverlaps())
            {
                Debug.LogWarning("Dungeon generation failed. Retrying...");
                currentAttemps++;
            }
            else
            {
                success = true;
                Debug.Log("Dungeon generated successfully!");
            }
        }

        if (!success)
        {
            Debug.LogError("Failed to generate dungeon after maximum retries.");
        }
    }

    // Clear any previously placed rooms and reset the occupied positions
    void ClearPreviousGeneration()
    {
        foreach (Room room in placedRooms)
        {
            Destroy(room.gameObject);
        }

        placedRooms.Clear();
        occupiedPositions.Clear();
    }

    // Check if any rooms are overlapping
    bool CheckForOverlaps()
    {
        HashSet<Vector3Int> checkedPositions = new HashSet<Vector3Int>();

        foreach (Room room in placedRooms)
        {
            Vector3Int roomPos = Vector3Int.RoundToInt(room.transform.position);
            if (checkedPositions.Contains(roomPos))
            {
                // If a position is found more than once, there's an overlap
                Debug.LogError($"Overlap detected at position {roomPos}");
                return true;
            }
            checkedPositions.Add(roomPos);
        }

        return false; // No overlaps detected
    }

    void GenerateDungeon()
    {
        // Get the min and max room count for the current level
        int minRooms = (int)roomCountsPerLevel[currentLevel].x;
        int maxRooms = (int)roomCountsPerLevel[currentLevel].y;

        // Randomly choose how many rooms to generate within the range
        int roomCount = Random.Range(minRooms, maxRooms + 1);

        // Create the first room at a starting position (e.g., at (0, 0))
        Vector3 startPos = Vector3.zero;
        Room startRoom = InstantiateRoom(startPos);
        placedRooms.Add(startRoom);
        occupiedPositions.Add(Vector3Int.RoundToInt(startPos)); // Mark the starting position as occupied

        // Generate the remaining rooms
        for (int i = 1; i < roomCount; i++)
        {
            bool roomPlaced = false;
            int attemptCount = 0; // To avoid infinite loops

            // Try to place the room by finding a valid position
            while (!roomPlaced && attemptCount < 100)
            {
                Room randomRoom = GetRandomPlacedRoom(); // Get a random placed room
                RoomDirection availableDirection = GetRandomAvailableDirection(randomRoom); // Get a random available direction

                if (availableDirection != RoomDirection.None) // Check if there's an available direction
                {
                    Vector3 newRoomPos;
                    Room newRoom;
                    if (TryPlaceRoom(randomRoom, availableDirection, out newRoom, out newRoomPos))
                    {
                        placedRooms.Add(newRoom);
                        occupiedPositions.Add(Vector3Int.RoundToInt(newRoomPos)); // Mark the new room's position as occupied
                        roomPlaced = true;
                    }
                }
                attemptCount++;
            }

            if (attemptCount >= 50)
            {
                Debug.LogWarning("Could not place room after 50 attempts");
            }
        }

        // After generating all rooms, disable unused doors
        foreach (Room room in placedRooms)
        {
            room.DisableUnusedDoors();
        }
    }

    // Tries to place a room in the specified direction from the given room
    bool TryPlaceRoom(Room currentRoom, RoomDirection direction, out Room newRoom, out Vector3 newRoomPos)
    {
        newRoom = InstantiateRoom(Vector3.zero); // Instantiate the new room temporarily
        RoomDoor newRoomDoor;
        RoomDoor currentRoomDoor;

        switch (direction)
        {
            case RoomDirection.Top:
                currentRoomDoor = currentRoom.doorTop;
                newRoomDoor = newRoom.doorBottom;
                newRoomPos = AlignRooms(currentRoomDoor, newRoomDoor);
                break;
            case RoomDirection.Bottom:
                currentRoomDoor = currentRoom.doorBottom;
                newRoomDoor = newRoom.doorTop;
                newRoomPos = AlignRooms(currentRoomDoor, newRoomDoor);
                break;
            case RoomDirection.Left:
                currentRoomDoor = currentRoom.doorLeft;
                newRoomDoor = newRoom.doorRight;
                newRoomPos = AlignRooms(currentRoomDoor, newRoomDoor);
                break;
            case RoomDirection.Right:
                currentRoomDoor = currentRoom.doorRight;
                newRoomDoor = newRoom.doorLeft;
                newRoomPos = AlignRooms(currentRoomDoor, newRoomDoor);
                break;
            default:
                newRoomPos = Vector3.zero;
                return false;
        }

        // Round to integer grid to avoid floating point issues
        Vector3Int gridPos = Vector3Int.RoundToInt(newRoomPos);
        if (occupiedPositions.Contains(gridPos))
        {
            Debug.LogError($"Room placement failed due to overlap at position {newRoomPos}");
            Destroy(newRoom.gameObject); // Destroy the room if it's overlapping
            return false;
        }

        newRoom.transform.position = newRoomPos;
        ConnectRooms(currentRoom, newRoom, direction); // Set the connected rooms
        return true;
    }

    // Aligns the rooms based on their doors
    Vector3 AlignRooms(RoomDoor currentRoomDoor, RoomDoor newRoomDoor)
    {
        Vector3 doorOffset = currentRoomDoor.transform.position - newRoomDoor.transform.position;
        return newRoomDoor.transform.parent.position + doorOffset;
    }

    // Connect rooms by setting the references in the Room script
    void ConnectRooms(Room currentRoom, Room newRoom, RoomDirection direction)
    {
        switch (direction)
        {
            case RoomDirection.Top:
                currentRoom.roomAbove = newRoom;
                newRoom.roomBelow = currentRoom;
                currentRoom.doorTop.isConnected = true;
                newRoom.doorBottom.isConnected = true;
                break;
            case RoomDirection.Bottom:
                currentRoom.roomBelow = newRoom;
                newRoom.roomAbove = currentRoom;
                currentRoom.doorBottom.isConnected = true;
                newRoom.doorTop.isConnected = true;
                break;
            case RoomDirection.Left:
                currentRoom.roomLeft = newRoom;
                newRoom.roomRight = currentRoom;
                currentRoom.doorLeft.isConnected = true;
                newRoom.doorRight.isConnected = true;
                break;
            case RoomDirection.Right:
                currentRoom.roomRight = newRoom;
                newRoom.roomLeft = currentRoom;
                currentRoom.doorRight.isConnected = true;
                newRoom.doorLeft.isConnected = true;
                break;
        }
    }

    // Method to instantiate a new room at a specific position
    Room InstantiateRoom(Vector3 position)
    {
        GameObject chosenRoomPrefab = ChooseRoomPrefab(); // Choose a room prefab based on probabilities
        GameObject newRoomObj = Instantiate(chosenRoomPrefab, position, Quaternion.identity);
        return newRoomObj.GetComponent<Room>();
    }

    // Method to choose a room prefab based on probabilities
    GameObject ChooseRoomPrefab()
    {
        float total = 0;
        foreach (float chance in roomPrefabChances)
        {
            total += chance;
        }

        float randomValue = Random.Range(0, total);
        float cumulative = 0;

        for (int i = 0; i < roomPrefabChances.Count; i++)
        {
            cumulative += roomPrefabChances[i];
            if (randomValue < cumulative)
            {
                return roomPrefabs[i];
            }
        }

        return roomPrefabs[0]; // Default return to prevent errors
    }

    // Get a random available direction from the given room
    RoomDirection GetRandomAvailableDirection(Room room)
    {
        List<RoomDirection> availableDirections = new List<RoomDirection>();

        if (room.HasAvailableDirection(RoomDirection.Top)) availableDirections.Add(RoomDirection.Top);
        if (room.HasAvailableDirection(RoomDirection.Bottom)) availableDirections.Add(RoomDirection.Bottom);
        if (room.HasAvailableDirection(RoomDirection.Left)) availableDirections.Add(RoomDirection.Left);
        if (room.HasAvailableDirection(RoomDirection.Right)) availableDirections.Add(RoomDirection.Right);

        if (availableDirections.Count > 0)
        {
            return availableDirections[Random.Range(0, availableDirections.Count)];
        }

        return RoomDirection.None;
    }

    // Get a random placed room from the list
    Room GetRandomPlacedRoom()
    {
        return placedRooms[Random.Range(0, placedRooms.Count)];
    }
}
