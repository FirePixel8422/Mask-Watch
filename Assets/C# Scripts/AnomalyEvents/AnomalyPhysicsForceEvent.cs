using UnityEngine;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class AnomalyPhysicsForceEvent : AnomalyEvent
{
    [SerializeField] private Vector3 linearVelocity;
    [SerializeField] private Vector3 angularVelocity;

    [SerializeField] private float linearForceMultiplier;
    [SerializeField] private float angularForceMultiplier;

    private Rigidbody rb;


    protected override void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        resetHandler = new ObjectPhysicsResetHandler(rb, transform);
    }

    protected override void OnExecute()
    {
        rb.isKinematic = false;

        rb.AddForce(linearVelocity.normalized * linearForceMultiplier, ForceMode.VelocityChange);
        rb.AddTorque(angularVelocity.normalized * angularForceMultiplier, ForceMode.VelocityChange);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + linearVelocity);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + angularVelocity * 0.5f);
    }
}