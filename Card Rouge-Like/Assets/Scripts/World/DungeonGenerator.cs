using UnityEngine;
using System.Collections.Generic;

public class DungeonGenerator : MonoBehaviour
{
    public Room[] roomPrefabs; // Use Room4 script references instead of GameObject
    public float[] roomSpawnChances; // Array to hold the spawn chances for each room prefab
    public float scalingFactor = 0.05f;
    public Vector2Int[] roomSizes; // Sizes for each room prefab (e.g., 1x1, 2x1, 2x2)
    public int gridWidth = 9;
    public int gridHeight = 8;
    private int maxRooms;
    private Vector2Int startRoomSize = new Vector2Int(1, 1); // Starting room size
    public Room startingRoomPrefab; // Assign this in the Inspector for the starting room
    public Room bossRoomPrefab; // Assign the boss room prefab in the Inspector
    private Vector2Int startRoomPosition = new Vector2Int(0, 0); // Start at (0, 0)
    private List<Vector2Int> availablePositions = new List<Vector2Int>();
    private Dictionary<Vector2Int, Room> placedRooms = new Dictionary<Vector2Int, Room>();
    public Vector3 gridSize = new Vector3(21.4f, 12f, 10f); // Defines the grid size in world space

    void Start()
    {
        maxRooms = Mathf.FloorToInt(Random.Range(2, 3) + 5 + PlayerStatistics.instance.level * 2.6f); // Calculate maxRooms based on level
        AdjustRoomSpawnChances();
        GenerateDungeon();
    }

    void AdjustRoomSpawnChances()
    {
        float total = 0;

        // Adjust room chances based on the current level
        for (int i = 0; i < roomSpawnChances.Length; i++)
        {
            // Decrease chance for 1x1 (first room) and increase for larger rooms
            if (i == 0) // Assuming 1x1 is at index 0
            {
                roomSpawnChances[i] -= scalingFactor * PlayerStatistics.instance.level; // Decrease chance
            }
            else
            {
                roomSpawnChances[i] += scalingFactor * PlayerStatistics.instance.level; // Increase chance for larger rooms
            }

            // Ensure chances stay non-negative
            roomSpawnChances[i] = Mathf.Max(roomSpawnChances[i], 0);
            total += roomSpawnChances[i];
        }

        // Normalize to ensure total is 100%
        for (int i = 0; i < roomSpawnChances.Length; i++)
        {
            roomSpawnChances[i] = (roomSpawnChances[i] / total) * 100;
        }
    }

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
            Vector2Int roomSize = roomSizes[GetWeightedRandomRoomIndex()]; // Pick a room size based on spawn chances
            if (CanPlaceRoom(newPosition, roomSize))
            {
                PlaceRoom(newPosition, roomSize);
            }
        }

        // Place the boss room at the farthest available position
        PlaceBossRoom();

        // After generation, update doors for all placed rooms
        foreach (var room in placedRooms.Values)
        {
            room.UpdateDoors();
        }
    }

    void PlaceBossRoom()
    {
        // Find the position farthest from the starting room (0, 0)
        Vector2Int bossRoomPosition = new Vector2Int(-1, -1);
        float maxDistance = -1f;

        foreach (var pos in availablePositions)
        {
            float distance = Vector2Int.Distance(startRoomPosition, pos);
            if (distance > maxDistance)
            {
                maxDistance = distance;
                bossRoomPosition = pos;
            }
        }

        if (bossRoomPosition != new Vector2Int(-1, -1))
        {
            PlaceRoom(bossRoomPosition, new Vector2Int(1, 1), bossRoomPrefab); // Use a suitable size for the boss room
        }
    }

    private int GetWeightedRandomRoomIndex()
    {
        float total = 0;
        // Sum up the chances and normalize them
        for (int i = 0; i < roomSpawnChances.Length; i++)
        {
            total += roomSpawnChances[i] / 100f; // Divide by 100 to convert to a proportion
        }

        float randomPoint = Random.value * total; // Get a random point in the range of total
        for (int i = 0; i < roomSpawnChances.Length; i++)
        {
            // Compare with the normalized chance
            if (randomPoint < (roomSpawnChances[i] / 100f)) // Normalize for the comparison
            {
                return i;
            }
            randomPoint -= (roomSpawnChances[i] / 100f);
        }

        return 0; // Fallback to the first room if something goes wrong
    }

    void PlaceRoom(Vector2Int position, Vector2Int size, Room roomPrefab = null)
    {
        // Use the provided prefab for the starting room
        if (roomPrefab == null)
        {
            roomPrefab = roomPrefabs[System.Array.IndexOf(roomSizes, size)];
        }

        // Instantiate room at the calculated position
        Vector3 roomPosition = new Vector3(position.x * gridSize.x, position.y * gridSize.y, 0);
        Room room = Instantiate(roomPrefab, roomPosition, Quaternion.identity);
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

    void UpdateAdjacentRooms(Vector2Int position, Room room, Vector2Int size)
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
                    Room neighbor = placedRooms[rightPos];
                    if (!room.rightRooms.Contains(neighbor) && neighbor != room)
                    {
                        room.rightRooms.Add(neighbor);
                        neighbor.leftRooms.Add(room);
                    }
                }

                // Check Left Neighbor
                Vector2Int leftPos = new Vector2Int(currentPos.x - 1, currentPos.y);
                if (placedRooms.ContainsKey(leftPos))
                {
                    Room neighbor = placedRooms[leftPos];
                    if (!room.leftRooms.Contains(neighbor) && neighbor != room)
                    {
                        room.leftRooms.Add(neighbor);
                        neighbor.rightRooms.Add(room);
                    }
                }

                // Check Top Neighbor
                Vector2Int topPos = new Vector2Int(currentPos.x, currentPos.y + 1);
                if (placedRooms.ContainsKey(topPos))
                {
                    Room neighbor = placedRooms[topPos];
                    if (!room.topRooms.Contains(neighbor) && neighbor != room)
                    {
                        room.topRooms.Add(neighbor);
                        neighbor.bottomRooms.Add(room);
                    }
                }

                // Check Bottom Neighbor
                Vector2Int bottomPos = new Vector2Int(currentPos.x, currentPos.y - 1);
                if (placedRooms.ContainsKey(bottomPos))
                {
                    Room neighbor = placedRooms[bottomPos];
                    if (!room.bottomRooms.Contains(neighbor) && neighbor != room)
                    {
                        room.bottomRooms.Add(neighbor);
                        neighbor.topRooms.Add(room);
                    }
                }
            }
        }
    }

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

    void AddAvailablePosition(Vector2Int position)
    {
        if (position.x >= 0 && position.x < gridWidth && position.y >= 0 && position.y < gridHeight && !placedRooms.ContainsKey(position))
        {
            if (!availablePositions.Contains(position))
                availablePositions.Add(position);
        }
    }
}
