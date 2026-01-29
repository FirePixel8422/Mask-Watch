using UnityEngine;


public class ObjectResetHandler
{
    protected Transformation startTransformation;
    protected Transform transform;


    public ObjectResetHandler(Transform transform)
    {
        startTransformation = new Transformation(transform);
        this.transform = transform;
    }

    public virtual void OnReset()
    {
        startTransformation.ApplyToTransform(transform);
    }
}