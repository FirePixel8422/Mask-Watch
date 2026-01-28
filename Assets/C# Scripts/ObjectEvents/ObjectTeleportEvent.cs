using UnityEngine;


public class ObjectTeleportEvent : ObjectEvent
{
    [SerializeField] private Transformation[] transformations = new Transformation[1] { Transformation.Identity };


    protected override void OnExecute()
    {
        transformations.SelectRandom().ApplyToTransform(transform);
    }

    private void OnDrawGizmosSelected()
    {
        if (transformations == null) return;

        Vector3 parentPos = transform.parent != null ? transform.parent.position : Vector3.zero;

        for (int i = 0; i < transformations.Length; i++)
        {
            Gizmos.DrawWireCube(parentPos + transformations[i].Position, Vector3.one * 0.25f);
        }
    }
}