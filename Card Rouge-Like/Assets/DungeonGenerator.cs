using UnityEngine;
using System.Collections.Generic;

public class DungeonGenerator : MonoBehaviour
{
    public Room4[] roomPrefabs; // Use Room4 script references instead of GameObject
    public Vector2Int[] roomSizes; // Sizes for each room prefab (e.g., 1x1, 2x1, 2x2)
    public int gridWidth = 9;
    public int gridHeight = 8;
    public int maxRooms = 20;
    private Vector2Int startRoomSize = new Vector2Int(1, 1); // Starting room size
    public Room4 startingRoomPrefab; // Assign this in the Inspector for the starting room
    private Vector2Int startRoomPosition = new Vector2Int(0, 0); // Start at (0, 0)
    private List<Vector2Int> availablePositions = new List<Vector2Int>();
    private Dictionary<Vector2Int, Room4> placedRooms = new Dictionary<Vector2Int, Room4>();
    public Vector3 gridSize = new Vector3(21.4f, 12f, 10f); // Defines the grid size in world space

    void Start()
    {
        GenerateDungeon();
    }

    /// <summary>
    /// Main method to generate the dungeon layout.
    /// </summary>
    void GenerateDungeon()
    {
        // Place the starting room at (0, 0)
        PlaceRoom(startRoomPosition, startRoomSize, startingRoomPrefab);

        // Place other rooms
        for (int i = 0; i < maxRooms - 1; i++)
        {
            if (availablePositions.Count == 0)
                break;

            Vector2Int newPosition = availablePositions[Random.Range(0, availablePositions.Count)];
            Vector2Int roomSize = roomSizes[Random.Range(0, roomSizes.Length)]; // Randomly pick a room size
            if (CanPlaceRoom(newPosition, roomSize))
            {
                PlaceRoom(newPosition, roomSize);
            }
        }

        // After generation, update doors for all placed rooms
        foreach (var room in placedRooms.Values)
        {
            room.UpdateDoors();
        }
    }

    /// <summary>
    /// Places a room at the specified position with the given size.
    /// </summary>
    /// <param name="position">Bottom-left position of the room on the grid.</param>
    /// <param name="size">Size of the room (width x height).</param>
    /// <param name="roomPrefab">The prefab of the room to instantiate.</param>
    void PlaceRoom(Vector2Int position, Vector2Int size, Room4 roomPrefab = null)
    {
        // Use the provided prefab for the starting room
        if (roomPrefab == null)
        {
            roomPrefab = roomPrefabs[System.Array.IndexOf(roomSizes, size)];
        }

        // Instantiate room at the calculated position
        Vector3 roomPosition = new Vector3(position.x * gridSize.x, position.y * gridSize.y, 0);
        Room4 room = Instantiate(roomPrefab, roomPosition, Quaternion.identity);
        placedRooms[position] = room;

        // Mark all tiles occupied by this room
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector2Int occupiedPos = new Vector2Int(position.x + x, position.y + y);
                placedRooms[occupiedPos] = room;
                availablePositions.Remove(occupiedPos);
            }
        }

        // Update room references to adjacent rooms
        UpdateAdjacentRooms(position, room, size);

        // Add neighboring positions for future room placements
        AddAvailablePositions(position, size);
    }

    /// <summary>
    /// Updates the adjacent room references for the newly placed room by iterating through each edge tile.
    /// </summary>
    /// <param name="position">Position of the newly placed room.</param>
    /// <param name="room">The newly placed Room4 instance.</param>
    /// <param name="size">Size of the newly placed room.</param>
    void UpdateAdjacentRooms(Vector2Int position, Room4 room, Vector2Int size)
    {
        // Iterate through each tile on the perimeter of the room
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector2Int currentPos = new Vector2Int(position.x + x, position.y + y);

                // Check Right Neighbor
                Vector2Int rightPos = new Vector2Int(currentPos.x + 1, currentPos.y);
                if (placedRooms.ContainsKey(rightPos))
                {
                    Room4 neighbor = placedRooms[rightPos];
                    if (!room.rightRooms.Contains(neighbor) && neighbor != room)
                    {
                        room.rightRooms.Add(neighbor);
                        neighbor.leftRooms.Add(room);
                        Debug.Log($"{room.name} connected to {neighbor.name} on the Right.");
                    }
                }

                // Check Left Neighbor
                Vector2Int leftPos = new Vector2Int(currentPos.x - 1, currentPos.y);
                if (placedRooms.ContainsKey(leftPos))
                {
                    Room4 neighbor = placedRooms[leftPos];
                    if (!room.leftRooms.Contains(neighbor) && neighbor != room)
                    {
                        room.leftRooms.Add(neighbor);
                        neighbor.rightRooms.Add(room);
                        Debug.Log($"{room.name} connected to {neighbor.name} on the Left.");
                    }
                }

                // Check Top Neighbor
                Vector2Int topPos = new Vector2Int(currentPos.x, currentPos.y + 1);
                if (placedRooms.ContainsKey(topPos))
                {
                    Room4 neighbor = placedRooms[topPos];
                    if (!room.topRooms.Contains(neighbor) && neighbor != room)
                    {
                        room.topRooms.Add(neighbor);
                        neighbor.bottomRooms.Add(room);
                        Debug.Log($"{room.name} connected to {neighbor.name} on the Top.");
                    }
                }

                // Check Bottom Neighbor
                Vector2Int bottomPos = new Vector2Int(currentPos.x, currentPos.y - 1);
                if (placedRooms.ContainsKey(bottomPos))
                {
                    Room4 neighbor = placedRooms[bottomPos];
                    if (!room.bottomRooms.Contains(neighbor) && neighbor != room)
                    {
                        room.bottomRooms.Add(neighbor);
                        neighbor.topRooms.Add(room);
                        Debug.Log($"{room.name} connected to {neighbor.name} on the Bottom.");
                    }
                }
            }
        }
    }

    /// <summary>
    /// Checks if a room can be placed at the specified position with the given size.
    /// </summary>
    /// <param name="position">Bottom-left position of the room on the grid.</param>
    /// <param name="size">Size of the room (width x height).</param>
    /// <returns>True if the room can be placed; otherwise, false.</returns>
    bool CanPlaceRoom(Vector2Int position, Vector2Int size)
    {
        // Check if all tiles that would be occupied by this room are free
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector2Int checkPos = new Vector2Int(position.x + x, position.y + y);
                if (placedRooms.ContainsKey(checkPos) || checkPos.x < 0 || checkPos.x >= gridWidth || checkPos.y < 0 || checkPos.y >= gridHeight)
                {
                    return false;
                }
            }
        }
        return true;
    }

    /// <summary>
    /// Adds all available positions around the newly placed room for future placements.
    /// </summary>
    /// <param name="position">Position of the newly placed room.</param>
    /// <param name="size">Size of the newly placed room.</param>
    void AddAvailablePositions(Vector2Int position, Vector2Int size)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector2Int currentPos = new Vector2Int(position.x + x, position.y + y);

                // Add Right Position
                Vector2Int rightPos = new Vector2Int(currentPos.x + 1, currentPos.y);
                AddAvailablePosition(rightPos);

                // Add Left Position
                Vector2Int leftPos = new Vector2Int(currentPos.x - 1, currentPos.y);
                AddAvailablePosition(leftPos);

                // Add Top Position
                Vector2Int topPos = new Vector2Int(currentPos.x, currentPos.y + 1);
                AddAvailablePosition(topPos);

                // Add Bottom Position
                Vector2Int bottomPos = new Vector2Int(currentPos.x, currentPos.y - 1);
                AddAvailablePosition(bottomPos);
            }
        }
    }

    /// <summary>
    /// Adds a position to the list of available positions if it's within grid bounds and unoccupied.
    /// </summary>
    /// <param name="position">Position to add.</param>
    void AddAvailablePosition(Vector2Int position)
    {
        if (position.x >= 0 && position.x < gridWidth && position.y >= 0 && position.y < gridHeight && !placedRooms.ContainsKey(position))
        {
            if (!availablePositions.Contains(position))
                availablePositions.Add(position);
        }
    }
}
