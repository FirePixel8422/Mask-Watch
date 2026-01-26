using UnityEngine;


public class RoomObjectEvent : MonoBehaviour
{
    public RoomEventRequirements Requirements;
    public RoomEventExecuteOptions ExecuteOptions;

    protected bool isManipulated = false;


    public void Execute()
    {
        ExecuteOptions.MaxExecutionCount -= 1;
        isManipulated = true;

        OnExecute();
    }
    protected virtual void OnExecute() { }

    public bool TryReport(float reportTime)
    {
        if (isManipulated)
        {
            // Report after delay
            this.Invoke(() =>
            {
                isManipulated = false;
                OnReported();
            },
            reportTime);

            return true;
        }
        return false;
    }
    protected virtual void OnReported() { }
}