using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;


public class Room : MonoBehaviour
{
    public string RoomName;

    [SerializeField] private bool isRoomActive;
    private List<AnomalyEvent> anomalyEvents;


    private void Awake()
    {
        anomalyEvents = GetComponentsInChildren<AnomalyEvent>(true).ToList();

        int anomalyCount = anomalyEvents.Count;
        for (int i = 0; i < anomalyCount; i++)
        {
            if (anomalyEvents[i].IsChainEvent)
            {
                anomalyEvents.RemoveAtSwapBack(i);

                anomalyCount -= 1;
                i -= 1;
            }
        }
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
        anomalyEvents.Shuffle();
        int roomCount = anomalyEvents.Count;

        for (int i = 0; i < roomCount; i++)
        {
            AnomalyEvent cRoomEvent = anomalyEvents[i];

            if (cRoomEvent.TryExecute(isRoomActive && CameraDisplayManager.IsSwappingCamera == false, elapsedPlayTime))
            {
                anomalyEvents.RemoveAtSwapBack(i);
                return true;
            }
        }
        // No event executed, none met the requirements
        return false;
    }
}