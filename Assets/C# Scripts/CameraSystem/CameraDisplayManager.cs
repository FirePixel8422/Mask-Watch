using System.Collections;
using UnityEngine;


public class CameraDisplayManager : UpdateMonoBehaviour
{
    public static CameraDisplayManager Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }


    [SerializeField] private GameObject staticScreenCamHolder;

    [SerializeField] private MinMaxFloat camSwapDelayMinMax = new MinMaxFloat(0.5f, 1f);

    private Camera monitorCamera;
    private CameraController[] gameCameras;

    private bool isSwappingCamera;


    
    public void Init()
    {
        monitorCamera = GetComponentInChildren<Camera>();
        gameCameras = this.FindObjectsOfType<CameraController>(true);

        if (gameCameras.Length == 0)
        {
            DebugLogger.LogError("No game cameras found for CameraDisplay.");
            return;
        }
        SwapToStaticScreen(camSwapDelayMinMax.min);
    }
    protected override void OnUpdate()
    {
        if (isSwappingCamera || CameraReporter.IsReporting) return;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SwapToNextCameraLR(true);
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            SwapToNextCameraLR(false);
        }
    }

    private void SwapToNextCameraLR(bool reversed)
    {
        RoomManager.SwapToNextRoom(reversed);
        SwapToStaticScreen(EzRandom.Range(camSwapDelayMinMax));
    }

    public void SwapToStaticScreen(float duration)
    {
        if (isSwappingCamera)
        {
            // Kill any existing routines to avoid multiple actions stacking up
            StopAllCoroutines();
        }
        isSwappingCamera = true;

        monitorCamera.transform.SetParent(staticScreenCamHolder.transform, false, false);

        this.Invoke(duration , () =>
        {
            SwapToCamera(RoomManager.CurrentRoomId);
            isSwappingCamera = false;
        });
    }
    public void SwapToCamera(int camId)
    {
        monitorCamera.transform.SetParent(gameCameras[camId].CameraHolder, false, false);
    }
}
