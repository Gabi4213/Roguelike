using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    //public RoomDoor doorTop, doorBottom, doorLeft, doorRight;

    // New variables to track connected rooms
    public Room roomAbove;
    public Room roomBelow;
    public Room roomLeft;
    public Room roomRight;

    public List<bool> roomsFree = new List<bool>();



    // New transforms representing the positions where rooms can be placed
    public Transform[] spawnPositions;
    public bool[] isEmpty;


}

public enum RoomDirection
{
    None,
    Top,
    Bottom,
    Left,
    Right
}
