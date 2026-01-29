using UnityEngine;


public class ObjectPhysicsResetHandler : ObjectResetHandler
{
    protected Rigidbody rb;


    public ObjectPhysicsResetHandler(Rigidbody rb, Transform transform) : base(transform)
    {
        this.rb = rb;
        this.transform = transform;
    }

    public override void OnReset()
    {
        rb.isKinematic = true;
        base.OnReset();
    }
}