using System.Collections.Generic;
using UnityEngine;



public class Room : MonoBehaviour
{
    private bool isRoomActive;

    private IRoomObjectEvent[] roomEvents;


    private void Awake()
    {
        roomEvents = GetComponentsInChildren<IRoomObjectEvent>();
    }


    // Toggle the active state of the room and update its internal isActive tracking state
    public void ToggleRoomState()
    {
        isRoomActive = !isRoomActive;

        gameObject.SetActive(isRoomActive);
    }

    public void ExecuteRandomEvent()
    {
        for (int i = 0; i < roomEvents.Length; i++)
        {
            //if(roomEvents.)
        }
    }
}