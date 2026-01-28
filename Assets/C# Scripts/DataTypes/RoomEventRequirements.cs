using UnityEngine;


[System.Serializable]
public struct RoomEventRequirements
{
    [Tooltip("Allow room event if room is not being watched")]
    public RoomVisibilyRequirement VisibilyRequirement;

    public bool ConfirmVisibilityRequirement(bool isRoomActive)
    {
        return VisibilyRequirement switch
        {
            RoomVisibilyRequirement.None => true,
            RoomVisibilyRequirement.OnlyIfRoomIsActive => isRoomActive == true,
            RoomVisibilyRequirement.OnlyIfRoomIsNotActive => isRoomActive == false,
            _ => false,
        };
    }

    [Tooltip("Only allow room event after time threshold is passed")]
    public float TimeRequirement;
}