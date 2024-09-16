using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Room roomPrefab; // Array of room prefabs to instantiate
    public int numberOfRooms;

    public Vector2[] roomCountsPerLevel; // Array to store min and max room counts per level
    public int currentLevel = 0; // The current dungeon level
    private List<Room> placedRooms = new List<Room>(); // List of placed rooms to track them
    private HashSet<Vector3Int> occupiedPositions = new HashSet<Vector3Int>(); // Track positions of placed rooms using integer grid


    private void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        // Spawn the first room
        Room firstRoom = Instantiate(roomPrefab, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        placedRooms.Add(firstRoom);

        Room currentRoom = firstRoom;

        // Loop to spawn additional rooms
        for (int i = 0; i < numberOfRooms; i++)
        {
            bool roomPlaced = false;

            // Attempt to place a new room until successful
            while (!roomPlaced)
            {
                // Pick a random spawn direction (0=up, 1=left, 2=down, 3=right)
                int randomNum = Random.Range(0, 4);

                // Check if the chosen spawn position is empty
                if (currentRoom.isEmpty[randomNum])
                {
                    // Determine the opposite spawn position
                    int oppositePosition = GetOppositeSpawnPosition(randomNum);

                    // Spawn the new room
                    Room newRoom = Instantiate(roomPrefab, currentRoom.spawnPositions[randomNum].position, Quaternion.identity);

                    // Adjust the new room's position using the opposite side
                    newRoom.transform.position = currentRoom.spawnPositions[randomNum].position - (newRoom.spawnPositions[oppositePosition].position - newRoom.transform.position);

                    // Mark both positions as no longer empty
                    currentRoom.isEmpty[randomNum] = false;
                    newRoom.isEmpty[oppositePosition] = false;

                    // Add the new room to the list of placed rooms
                    placedRooms.Add(newRoom);

                    // Move to the newly placed room for the next iteration
                    currentRoom = newRoom;

                    // Mark the room as placed and exit the while loop
                    roomPlaced = true;
                }
            }
        }
    }

    // Helper function to get the opposite spawn position
    int GetOppositeSpawnPosition(int position)
    {
        switch (position)
        {
            case 0: return 2; // Up -> Down
            case 1: return 3; // Left -> Right
            case 2: return 0; // Down -> Up
            case 3: return 1; // Right -> Left
            default: return -1; // Invalid position
        }
    }


}