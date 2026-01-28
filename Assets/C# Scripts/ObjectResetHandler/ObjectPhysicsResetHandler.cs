using UnityEngine;


public class ObjectPhysicsResetHandler : ObjectResetHandler
{
    private Rigidbody rb;


    public override void OnStart()
    {
        base.OnStart();
        rb = GetComponent<Rigidbody>();
    }

    public override void OnReset()
    {
        rb.isKinematic = true;
        base.OnReset();
    }
}