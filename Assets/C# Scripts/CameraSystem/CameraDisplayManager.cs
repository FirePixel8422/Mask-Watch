using System.Collections;
using UnityEngine;


public class CameraDisplayManager : UpdateMonoBehaviour
{
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
            DebugLogger.LogWarning("No game cameras found for CameraDisplay.");
            return;
        }
        StartCoroutine(SwapCameraCycle());
    }

    protected override void OnUpdate()
    {
        if (isSwappingCamera) return;

        if (Input.GetKeyDown(KeyCode.M))
        {
            isSwappingCamera = true;
            StartCoroutine(SwapCameraCycle());
        }
    }

    private IEnumerator SwapCameraCycle()
    {
        RoomManager.LoadNextRoom();

        monitorCamera.transform.SetParent(staticScreenCamHolder.transform, false, false);

        yield return new WaitForSeconds(EzRandom.Range(camSwapDelayMinMax));

        SwapToCamera(RoomManager.CurrentRoomId);

        isSwappingCamera = false;
    }

    private void SwapToCamera(int camId)
    {
        monitorCamera.transform.SetParent(gameCameras[camId].CameraHolder, false, false);
    }
}
