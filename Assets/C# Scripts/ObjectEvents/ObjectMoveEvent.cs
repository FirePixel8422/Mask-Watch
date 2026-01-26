using System.Collections;
using UnityEngine;


public class ObjectMoveEvent : RoomObjectEvent
{
    [SerializeField] private Transformation[] transformations = new Transformation[1] { Transformation.Identity };
    [SerializeField] private float transformTime;

    private Transformation startTransformation;


    protected override void OnExecute()
    {
        // Store start transform
        startTransformation = Transformation.FromTransform(transform);
        StartCoroutine(MoveCycle());
    }
    protected override void OnReported()
    {
        StopAllCoroutines();
        // Reset transform;
        startTransformation.ApplyToTransform(transform);
    }

    private IEnumerator MoveCycle()
    {
        Transformation targetTransformation = transformations.SelectRandom();
        Transformation cTransformation = Transformation.FromTransform(transform);

        float moveStep = Vector3.Distance(cTransformation.Position, targetTransformation.Position) / transformTime;
        float rotStep = Vector3.Distance(cTransformation.RotationEuler, targetTransformation.RotationEuler) / transformTime;
        float scaleStep = Vector3.Distance(cTransformation.Scale, targetTransformation.Scale) / transformTime;

        float elapsed = 0;

        do
        {
            yield return null;
            elapsed += Time.deltaTime;

            cTransformation.Position = Vector3.MoveTowards(cTransformation.Position, targetTransformation.Position, moveStep * Time.deltaTime);
            cTransformation.RotationEuler = Vector3.MoveTowards(cTransformation.RotationEuler, targetTransformation.RotationEuler, rotStep * Time.deltaTime);
            cTransformation.Scale = Vector3.MoveTowards(cTransformation.Scale, targetTransformation.Scale, scaleStep * Time.deltaTime);

            cTransformation.ApplyToTransform(transform);
        }
        while (elapsed < transformTime);
    }
    

    private void OnDrawGizmosSelected()
    {
        if (transformations == null) return;

        for (int i = 0; i < transformations.Length; i++)
        {
            Gizmos.DrawWireCube(transform.parent.position + transformations[i].Position, Vector3.one * 0.25f);
        }
    }
}