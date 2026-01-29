using UnityEngine;


public abstract class AnomalyEvent : MonoBehaviour
{
    public RoomEventRequirements Requirements;
    public float anomalySeverity = 1f;

    private bool isActive = false;
    protected ObjectResetHandler resetHandler;


    protected virtual void Awake()
    {
        resetHandler = new ObjectResetHandler(transform);
    }

    public bool TryExecute(bool isRoomActive, float elapsedPlayTime)
    {
        // Confirm execution conditions
        if (Requirements.ConfirmVisibilityRequirement(isRoomActive) && Requirements.TimeRequirement < elapsedPlayTime)
        {
            OnExecute();
            isActive = true;
            return true;
        }
        return false;
    }
    protected virtual void OnExecute() { }

    public bool TryReport(float delay)
    {
        if (isActive)
        {
            // Report after delay
            this.Invoke(delay, () =>
            {
                isActive = false;
                resetHandler.OnReset();
                OnReported();
            });

            return true;
        }
        return false;
    }
    protected virtual void OnReported() { }


#if UNITY_EDITOR
    [ContextMenu("DEBUG: Force Execute Anomaly Event")]
    public void DEBUG_ForceExecute()
    {
        if (Application.isPlaying == false) return;

        OnExecute();
        isActive = true;
    }
#endif
}