using System.Collections;
using Unity.Mathematics;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class AnomalyPhysicsGrowEvent : AnomalyEvent
{
    [SerializeField] private Vector3 scale;
    [SerializeField] private float growTime = 1f;

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
        StartCoroutine(GrowOverTime());
    }
    protected override void OnReported()
    {
        StopAllCoroutines();
    }

    private IEnumerator GrowOverTime()
    {
        Vector3 cScale = transform.localScale;
        float growStep = Vector3.Distance(cScale, scale);

        float elapsedTime = 0f;
        while (elapsedTime < growTime)
        {
            elapsedTime += Time.deltaTime;

            cScale = Vector3.MoveTowards(cScale, scale, growStep / growTime * Time.deltaTime);
            transform.localScale = cScale;

            yield return null;
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireMesh(GetComponent<MeshFilter>().sharedMesh, 0, transform.position, transform.rotation, scale);
    }
}