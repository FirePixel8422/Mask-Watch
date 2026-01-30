using UnityEngine;


public abstract class AnomalyEvent : MonoBehaviour
{
    public RoomEventRequirements Requirements;
    public float anomalySeverity = 1f;

    private bool isActive = false;
    protected ObjectResetHandler resetHandler;

    [SerializeField] private AnomalyEvent chainEvent;
    public bool IsChainEvent;


    protected virtual void Awake()
    {
        resetHandler = new ObjectResetHandler(transform);
    }

    public bool TryExecute(bool isRoomActive, float elapsedPlayTime)
    {
        if (chainEvent != null)
        {
            chainEvent.TryExecute(isRoomActive, elapsedPlayTime);
        }
        // Confirm execution conditions
        if (Requirements.ConfirmVisibilityRequirement(isRoomActive) && Requirements.TimeRequirement < elapsedPlayTime)
        {
            OnExecute();
            CameraHealthHandler.Instance.CurrentAnomalySeverity += anomalySeverity;
            isActive = true;

            return true;
        }
        return false;
    }
    protected virtual void OnExecute() { }

    public bool TryReport(float delay)
    {
        if (chainEvent != null)
        {
            chainEvent.TryReport(delay);
        }
        if (isActive)
        {
            // Report after delay
            this.Invoke(delay, () =>
            {
                CameraHealthHandler.Instance.CurrentAnomalySeverity -= anomalySeverity;
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

        if (chainEvent != null)
        {
            chainEvent.DEBUG_ForceExecute();
        }
        OnExecute();
        CameraHealthHandler.Instance.CurrentAnomalySeverity += anomalySeverity;
        isActive = true;
    }
#endif
}