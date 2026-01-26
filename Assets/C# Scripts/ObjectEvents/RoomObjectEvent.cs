using UnityEngine;


public abstract class RoomObjectEvent : MonoBehaviour
{
    public RoomEventRequirements Requirements;
    public RoomEventExecuteOptions ExecuteOptions;

    public bool IsActive = false;


    public bool TryExecute(bool isRoomActive, float elapsedPlayTime, out bool ranOutOfExecutions)
    {
        // Confirm execution conditions
        if ((Requirements.AllowIfRoomActive || isRoomActive == false) ||
            Requirements.TimeRequirement > elapsedPlayTime)
        {
            ranOutOfExecutions = default;
            return false;
        }

        ExecuteOptions.MaxExecutionCount -= 1;
        ranOutOfExecutions = ExecuteOptions.IsOutOfExecutions;

        OnExecute();
        IsActive = true;

        return true;
    }
    protected virtual void OnExecute() { }

    public bool TryReport(float delay)
    {
        if (IsActive)
        {
            // Report after delay
            this.Invoke(() =>
            {
                IsActive = false;
                OnReported();
            },
            delay);

            return true;
        }
        return false;
    }
    protected virtual void OnReported() { }
}