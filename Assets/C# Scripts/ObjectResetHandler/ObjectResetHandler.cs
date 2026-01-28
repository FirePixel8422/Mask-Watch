using UnityEngine;



public class ObjectResetHandler : MonoBehaviour
{
    private Transformation startTransformation;


    public virtual void OnStart()
    {
        startTransformation = Transformation.FromTransform(transform);
    }

    public virtual void OnReset()
    {
        startTransformation.ApplyToTransform(transform);
    }
}