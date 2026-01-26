using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;



public class Room : MonoBehaviour
{
    private bool isRoomActive;

    [SerializeField] private List<RoomObjectEvent> roomEvents;


    private void Awake()
    {
        roomEvents = GetComponentsInChildren<RoomObjectEvent>(true).ToList();
    }

    // Toggle the active state of the room and update its internal isActive tracking state
    public void ToggleRoomState()
    {
        isRoomActive = !isRoomActive;
    }

    public bool ExecuteRandomEvent(float elapsedPlayTime)
    {
        // POSSIBLE PERFORMANCE ISSUE IF MANY EVENTS - MAY NEED OPTIMIZATION LATER
        // POSSIBLE PERFORMANCE ISSUE IF MANY EVENTS - MAY NEED OPTIMIZATION LATER
        // POSSIBLE PERFORMANCE ISSUE IF MANY EVENTS - MAY NEED OPTIMIZATION LATER
        roomEvents.Shuffle();
        int roomCount = roomEvents.Count;

        for (int i = 0; i < roomCount; i++)
        {
            RoomObjectEvent cRoomEvent = roomEvents[i];

            if (cRoomEvent.Requirements.AllowIfRoomActive == isRoomActive &&
                cRoomEvent.Requirements.TimeRequirement < elapsedPlayTime)
            {
                cRoomEvent.Execute();

                if (cRoomEvent.ExecuteOptions.IsOutOfExecutions)
                {
                    roomEvents.RemoveAtSwapBack(i);
                }
                // Event executed successfully
                return true;
            }
        }
        // No event executed, none met the requirements
        return false;
    }
}