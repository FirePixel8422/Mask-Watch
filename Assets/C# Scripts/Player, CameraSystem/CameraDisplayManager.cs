using UnityEngine;


public class CameraDisplayManager : UpdateMonoBehaviour
{
    public static CameraDisplayManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }


    [SerializeField] private GameObject staticScreenCamHolder;
    [SerializeField] private Transform[] cameraHolders;

    [SerializeField] private MinMaxFloat camSwapDelayMinMax = new MinMaxFloat(0.5f, 1f);

    private Camera monitorCamera;
    public static bool IsSwappingCamera;


    
    public void Init()
    {
        monitorCamera = GetComponentInChildren<Camera>(true);

        if (cameraHolders.Length == 0)
        {
            DebugLogger.LogError("No game cameras found for CameraDisplay.");
            return;
        }
        SwapToStaticScreen(camSwapDelayMinMax.min);
    }
    protected override void OnUpdate()
    {
        if (IsSwappingCamera || CameraReporter.IsTryingToReport) return;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SwapToNextCameraLR(true);
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            SwapToNextCameraLR(false);
        }
    }

    public void SwapToNextCameraLR(bool reversed)
    {
        RoomManager.Instance.SwapToNextRoom(reversed);
        SwapToStaticScreen(EzRandom.Range(camSwapDelayMinMax));
    }

    public void SwapToStaticScreen(float duration)
    {
        if (IsSwappingCamera)
        {
            // Kill any existing routines to avoid multiple actions stacking up
            StopAllCoroutines();
        }
        IsSwappingCamera = true;

        monitorCamera.transform.SetParent(staticScreenCamHolder.transform, false, false);

        this.Invoke(duration, () =>
        {
            SwapToCamera(RoomManager.CurrentRoomId);
            IsSwappingCamera = false;
        });
    }
    public void SwapToCamera(int camId)
    {
        monitorCamera.transform.SetParent(cameraHolders[camId], false, false);
    }
}
