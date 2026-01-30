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
        Transformation parentTransformation = transform.parent != null ? new Transformation(transform.parent) : Transformation.Identity;
        Gizmos.DrawWireMesh(GetComponent<MeshFilter>().sharedMesh, 0,
            parentTransformation.Position + transformation.Position,
            parentTransformation.Rotation * transformation.Rotation,
            Vector3.Cross(parentTransformation.Scale, transformation.Scale));
    }


#if UNITY_EDITOR

    [ContextMenu("Set Current Transformation")]
    private void SetCurrentTransformation()
    {
        transformation = new Transformation(transform);
    }
#endif
}