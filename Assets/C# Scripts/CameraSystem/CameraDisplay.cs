using System.Collections;
using UnityEngine;


public class CameraDisplay : UpdateMonoBehaviour
{
    [SerializeField] private GameObject staticScreenCamHolder;

    [SerializeField] private MinMaxFloat camSwapDelayMinMax = new MinMaxFloat(0.5f, 1f);

    private Camera monitorCamera;
    private CameraController[] gameCameras;

    private int selectedCameraIndex;
    private bool isSwappingCamera;


    private void Start()
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
        selectedCameraIndex.IncrementSmart(gameCameras.Length);

        monitorCamera.transform.SetParent(staticScreenCamHolder.transform, false, false);

        yield return new WaitForSeconds(EzRandom.Range(camSwapDelayMinMax));

        SwapToCamera(selectedCameraIndex);

        isSwappingCamera = false;
    }

    private void SwapToCamera(int camId)
    {
        monitorCamera.transform.SetParent(gameCameras[camId].CameraHolder, false, false);
    }
}
