using UnityEngine;


[System.Serializable]
public struct RoomEventRequirements
{
    [Tooltip("Allow room event if room is not being watched")]
    public bool AllowIfRoomActive;

    [Tooltip("Only allow room event after time threshold is passed")]
    public float TimeRequirement;
}