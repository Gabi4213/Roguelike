using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPlayerCheck : MonoBehaviour
{
    public Room3 room;

    // Check if player enters the room
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            room.isPlayerInRoom = true;
            room.PlayerInRoom();
        }
    }

    // Check if player exits the room
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            room.isPlayerInRoom = false;
            room.PlayerNotInRoom();
        }
    }
}
