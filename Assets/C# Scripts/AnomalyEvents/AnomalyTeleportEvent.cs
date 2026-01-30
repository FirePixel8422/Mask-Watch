using UnityEngine;


public class AnomalyTeleportEvent : AnomalyEvent
{
    [SerializeField] private Transformation transformation = Transformation.Identity;


    protected override void OnExecute()
    {
        transformation.ApplyToTransform(transform);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireMesh(GetComponent<MeshFilter>().sharedMesh, 0, transformation.Position , transformation.Rotation, transformation.Scale);
    }


#if UNITY_EDITOR

    [ContextMenu("Set Current Transformation")]
    private void SetCurrentTransformation()
    {
        transformation = new Transformation(transform);
    }
#endif
}