using System.Collections;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class ObjectPhysicsEvent : ObjectEvent
{
    [SerializeField] private Vector3 linearVelocity;
    [SerializeField] private Vector3 angularVelocity;
    [SerializeField] private MinMaxFloat randomForceMultiplier;

    private Transformation startTransformation;
    private Rigidbody rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    protected override void OnExecute()
    {
        // Store start transform
        startTransformation = Transformation.FromTransform(transform);
        rb.isKinematic = false;

        float forceMultiplier = EzRandom.Range(randomForceMultiplier);
        rb.AddForce(linearVelocity * forceMultiplier, ForceMode.VelocityChange);
        rb.AddTorque(angularVelocity * forceMultiplier, ForceMode.VelocityChange);
    }
    protected override void OnReported()
    {
        // Reset transform;
        rb.isKinematic = true;
        startTransformation.ApplyToTransform(transform);
    }
}