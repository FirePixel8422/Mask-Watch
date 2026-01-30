using System;
using UnityEngine;



public class AnomalyStateChangeEvent : AnomalyEvent
{
    public StateChangeType stateChangeType = StateChangeType.HideOnExecute;
    public bool HideOnExecute => stateChangeType == StateChangeType.HideOnExecute;


    public Renderer[] renderers;
    public Collider[] colliders;


    protected override void Awake()
    {
        base.Awake();

        renderers = GetComponentsInChildren<Renderer>();

        colliders = GetComponentsInChildren<Collider>();

        UpdateRenderers(HideOnExecute);
    }

    protected override void OnExecute()
    {
        UpdateRenderers(!HideOnExecute);
    }
    protected override void OnReported()
    {
        UpdateRenderers(HideOnExecute);
    }

    private void UpdateRenderers(bool activeState)
    {
        bool colliderState = activeState != HideOnExecute;

        if(renderers != null)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                renderers[i].enabled = activeState;
            }
        }
        if (colliders != null)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                colliders[i].enabled = colliderState;
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = HideOnExecute ? Color.red : Color.green;

        MeshFilter[] mfs = GetComponentsInChildren<MeshFilter>();
        if (TryGetComponent(out MeshFilter meshFilter))
        {
            Array.Resize(ref mfs, mfs.Length + 1);
            mfs[^1] = meshFilter;
        }

        for (int i = 0; i < mfs.Length; i++)
        {
            Gizmos.DrawWireMesh(mfs[i].sharedMesh, 0, mfs[i].transform.position, mfs[i].transform.rotation, mfs[i].transform.lossyScale);
        }
    }


    public enum StateChangeType : byte
    {
        HideOnExecute,
        ShowOnExecute
    }
}
