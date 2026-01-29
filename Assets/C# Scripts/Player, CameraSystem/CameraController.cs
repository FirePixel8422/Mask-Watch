using UnityEngine;


public class CameraController : UpdateMonoBehaviour
{
    [SerializeField] private Transform rotatePoint;
    [SerializeField] private Transform cameraHolder;
    public Transform CameraHolder => cameraHolder;

    [SerializeField] private MinMaxFloat rotClampX, rotClampY;


    public void RotateCamera(Vector2 mouseDelta, float sensitivity)
    {
        Vector2 cRot = rotatePoint.localEulerAngles;

        cRot.x += mouseDelta.x * sensitivity;
        cRot.y -= mouseDelta.y * sensitivity;

        cRot.x = Mathf.Clamp(cRot.x, rotClampX.min, rotClampX.max);
        cRot.y = Mathf.Clamp(cRot.y, rotClampY.min, rotClampY.max);

        transform.localRotation = Quaternion.Euler(cRot.y, cRot.x, 0f);
    }

    private void OnDrawGizmos()
    {
        Vector3 forward = CameraHolder.rotation * Vector3.forward;
        const float GIZMO_LENGTH = 1;

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(CameraHolder.position, 0.05f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(CameraHolder.position, CameraHolder.position + forward * GIZMO_LENGTH);

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(
            CameraHolder.position,
            CameraHolder.position +
            Quaternion.AngleAxis(rotClampX.min, CameraHolder.up) * forward * GIZMO_LENGTH
        );
        Gizmos.DrawLine(
            CameraHolder.position,
            CameraHolder.position +
            Quaternion.AngleAxis(rotClampX.max, CameraHolder.up) * forward * GIZMO_LENGTH
        );

        Gizmos.DrawLine(
            CameraHolder.position,
            CameraHolder.position +
            Quaternion.AngleAxis(rotClampY.min, CameraHolder.right) * forward * GIZMO_LENGTH
        );
        Gizmos.DrawLine(
            CameraHolder.position,
            CameraHolder.position +
            Quaternion.AngleAxis(rotClampY.max, CameraHolder.right) * forward * GIZMO_LENGTH
        );
    }
}
