using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ObjectPhysicsResetHandler))]
public class ObjectPhysicsEvent : ObjectEvent
{
    [SerializeField] private Vector3 linearVelocity;
    [SerializeField] private Vector3 angularVelocity;
    [SerializeField] private MinMaxFloat randomForceMultiplier;

    private Rigidbody rb;


    protected override void Awake()
    {
        base.Awake();

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    protected override void OnExecute()
    {
        rb.isKinematic = false;

        float forceMultiplier = EzRandom.Range(randomForceMultiplier);
        rb.AddForce(linearVelocity * forceMultiplier, ForceMode.VelocityChange);
        rb.AddTorque(angularVelocity * forceMultiplier, ForceMode.VelocityChange);
    }
    protected override void OnReported()
    {

    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + linearVelocity);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + angularVelocity * 0.5f);
    }
}