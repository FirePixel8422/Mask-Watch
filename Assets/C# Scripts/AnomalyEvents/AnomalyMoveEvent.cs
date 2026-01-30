using System.Collections;
using UnityEngine;


public class AnomalyMoveEvent : AnomalyEvent
{
    [SerializeField] private Transformation transformation = Transformation.Identity;
    [SerializeField] private float transformTime = 2;


    protected override void OnExecute()
    {
        StartCoroutine(MoveCycle());
    }
    protected override void OnReported()
    {
        StopAllCoroutines();
    }

    private IEnumerator MoveCycle()
    {
        Transformation cTransformation = new Transformation(transform);

        float moveStep = Vector3.Distance(cTransformation.Position, transformation.Position) / transformTime;
        float rotStep = Vector3.Distance(cTransformation.RotationEuler, transformation.RotationEuler) / transformTime;
        float scaleStep = Vector3.Distance(cTransformation.Scale, transformation.Scale) / transformTime;

        float elapsed = 0;

        do
        {
            yield return null;
            elapsed += Time.deltaTime;

            cTransformation.Position = Vector3.MoveTowards(cTransformation.Position, transformation.Position, moveStep * Time.deltaTime);
            cTransformation.RotationEuler = Vector3.MoveTowards(cTransformation.RotationEuler, transformation.RotationEuler, rotStep * Time.deltaTime);
            cTransformation.Scale = Vector3.MoveTowards(cTransformation.Scale, transformation.Scale, scaleStep * Time.deltaTime);

            cTransformation.ApplyToTransform(transform);
        }
        while (elapsed < transformTime);
    }
    

    private void OnDrawGizmosSelected()
    {
        if (GetComponent<MeshFilter>() == null) return;

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