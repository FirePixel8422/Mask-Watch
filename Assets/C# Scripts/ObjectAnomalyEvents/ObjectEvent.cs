using UnityEngine;


public abstract class ObjectEvent : MonoBehaviour
{
    public RoomEventRequirements Requirements;
    public RoomEventExecuteOptions ExecuteOptions = new RoomEventExecuteOptions() { AllowDuplicates = false, MaxExecutionCount = 1 };

    public bool IsActive = false;

    private ObjectResetHandler resetHandler;


    protected virtual void Awake()
    {
        if (TryGetComponent(out resetHandler))
        {
            resetHandler.OnStart();
        }
    }

    public bool TryExecute(bool isRoomActive, float elapsedPlayTime, out bool ranOutOfExecutions)
    {
        // Confirm execution conditions
        if (Requirements.ConfirmVisibilityRequirement(isRoomActive) == false ||
            Requirements.TimeRequirement > elapsedPlayTime ||
            IsActive && ExecuteOptions.AllowDuplicates == false)
        {
            ranOutOfExecutions = false;
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
            this.Invoke(delay, () =>
            {
                IsActive = false;
                resetHandler.OnReset();
                OnReported();
            });

            return true;
        }
        return false;
    }
    protected virtual void OnReported() { }
}