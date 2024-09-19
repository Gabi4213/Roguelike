using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator3 : MonoBehaviour
{
    [System.Serializable]
    public class DungeonRoom
    {
        public GameObject roomPrefab;
        public float spawnChance; // percentage chance of this room being spawned
    }

    public DungeonRoom startingRoom; // Starting room prefab with 100% spawn chance
    public DungeonRoom[] roomPrefabs; // Array of room prefabs with spawn chances
    public int maxRooms = 10; // Limit the number of rooms to generate

    private List<Vector3> occupiedPositions = new List<Vector3>();
    private List<Door> availableDoors = new List<Door>();

    private void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        // Start by spawning the first room
        GameObject startRoom = Instantiate(startingRoom.roomPrefab, Vector3.zero, Quaternion.identity);
        Room3 startRoomController = startRoom.GetComponent<Room3>();
        AddRoomToDungeon(startRoomController);

        // Generate additional rooms
        for (int i = 0; i < maxRooms - 1; i++)
        {
            if (availableDoors.Count == 0) break; // If no available doors, stop generation

            // Randomly pick a door from available doors
            Door chosenDoor = availableDoors[Random.Range(0, availableDoors.Count)];
            availableDoors.Remove(chosenDoor); // Remove this door from the available list

            // Choose a room based on the spawn chance array
            GameObject roomToSpawn = ChooseRoomPrefab();
            Room3 newRoom = Instantiate(roomToSpawn, chosenDoor.spawnPosition, Quaternion.identity).GetComponent<Room3>();

            // Position the new room using the corresponding door
            AlignRoom(newRoom, chosenDoor);
            AddRoomToDungeon(newRoom);
        }

        DisableUnusedDoors();
    }

    GameObject ChooseRoomPrefab()
    {
        float totalChance = 0f;
        foreach (DungeonRoom room in roomPrefabs)
        {
            totalChance += room.spawnChance;
        }

        float randomPoint = Random.value * totalChance;
        foreach (DungeonRoom room in roomPrefabs)
        {
            if (randomPoint < room.spawnChance)
            {
                return room.roomPrefab;
            }
            randomPoint -= room.spawnChance;
        }

        return roomPrefabs[roomPrefabs.Length - 1].roomPrefab; // Fallback
    }

    void AlignRoom(Room3 newRoom, Door connectedDoor)
    {
        // Find the door on the new room that connects to the previous one
        Door newRoomDoor = newRoom.GetOppositeDoor(connectedDoor.doorDirection);
        if (newRoomDoor == null) return;

        // Calculate the spawn position based on the door's position
        Vector3 offset = connectedDoor.transform.position - newRoomDoor.transform.position;
        newRoom.transform.position += offset;
    }

    void AddRoomToDungeon(Room3 roomController)
    {
        occupiedPositions.Add(roomController.transform.position);

        // Add room doors to the available doors list
        foreach (Door door in roomController.doors)
        {
            if (!occupiedPositions.Contains(door.spawnPosition))
            {
                availableDoors.Add(door);
            }
        }
    }

    void DisableUnusedDoors()
    {
        foreach (Door door in availableDoors)
        {
            door.gameObject.SetActive(false); // Disable unused doors
        }
    }
}
