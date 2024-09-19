using UnityEngine;
using System.Collections.Generic;

public class DungeonGenerator : MonoBehaviour
{
    public GameObject[] roomPrefabs;
    public Vector2Int[] roomSizes; // Sizes for each room prefab (e.g., 1x1, 2x1, 2x2)
    public int gridWidth = 9;
    public int gridHeight = 8;
    public int maxRooms = 20;
    private Vector2Int startRoom = new Vector2Int(4, 3); // Start in center of 9x8 grid
    private List<Vector2Int> availablePositions = new List<Vector2Int>();
    private Dictionary<Vector2Int, GameObject> placedRooms = new Dictionary<Vector2Int, GameObject>();
    public Vector3 gridSize = new Vector3(21.4f, 12f, 10f); // Defines the grid size in world space

    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        // Place the start room
        PlaceRoom(startRoom, new Vector2Int(1, 1));

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

        // Optionally: Place boss or special rooms far from the start
    }

    void PlaceRoom(Vector2Int position, Vector2Int size)
    {
        // Randomly select a room prefab that matches the size
        int prefabIndex = System.Array.IndexOf(roomSizes, size);
        if (prefabIndex < 0) return;

        GameObject roomPrefab = roomPrefabs[prefabIndex];

        // Instantiate room at calculated position
        GameObject room = Instantiate(roomPrefab, new Vector3(position.x * gridSize.x, position.y * gridSize.y, 0), Quaternion.identity);

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

        // Add neighboring positions for future room placements
        AddAvailablePosition(new Vector2Int(position.x + size.x, position.y));
        AddAvailablePosition(new Vector2Int(position.x - 1, position.y));
        AddAvailablePosition(new Vector2Int(position.x, position.y + size.y));
        AddAvailablePosition(new Vector2Int(position.x, position.y - 1));
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

    void AddAvailablePosition(Vector2Int position)
    {
        if (position.x >= 0 && position.x < gridWidth && position.y >= 0 && position.y < gridHeight && !placedRooms.ContainsKey(position))
        {
            availablePositions.Add(position);
        }
    }
}
