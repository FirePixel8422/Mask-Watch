using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;



public class Room : MonoBehaviour
{
    private bool isRoomActive;
    private List<ObjectEvent> roomEvents;


    private void Awake()
    {
        roomEvents = GetComponentsInChildren<ObjectEvent>(true).ToList();
    }

    /// <summary>
    /// Toggle the active state of the room and update its internal isActive tracking state
    /// </summary>
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
            ObjectEvent cRoomEvent = roomEvents[i];

            if (cRoomEvent.TryExecute(isRoomActive, elapsedPlayTime, out bool ranOutOfExections))
            {
                if (ranOutOfExections)
                {
                    roomEvents.RemoveAtSwapBack(i);
                }
                return true;
            }
        }
        // No event executed, none met the requirements
        return false;
    }
}