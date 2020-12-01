using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "RoomDataContainer", menuName = "RoomDataContainer", order = 1)]
public class RoomDataContainer : ScriptableObject
{
    public RoomData[] allRoomsData;
} 