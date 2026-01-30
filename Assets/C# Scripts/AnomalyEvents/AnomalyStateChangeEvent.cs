using System;
using UnityEngine;



public class AnomalyStateChangeEvent : AnomalyEvent
{
    public StateChangeType stateChangeType = StateChangeType.HideOnExecute;
    public bool HideOnExecute => stateChangeType == StateChangeType.HideOnExecute;


    private Renderer[] renderers;
    private Collider[] colliders;


    protected override void Awake()
    {
        base.Awake();

        renderers = GetComponentsInChildren<Renderer>();
        if (TryGetComponent(out Renderer renderer))
        {
            Array.Resize(ref renderers, renderers.Length + 1);
            renderers[^1] = renderer;
        }

        colliders = GetComponentsInChildren<Collider>();
        if (TryGetComponent(out Collider collider))
        {
            Array.Resize(ref renderers, colliders.Length + 1);
            colliders[^1] = collider;
        }

        UpdateRenderers(HideOnExecute);
    }

    protected override void OnExecute()
    {
        UpdateRenderers(HideOnExecute == false);
    }
    protected override void OnReported()
    {
        UpdateRenderers(HideOnExecute);
    }

    private void UpdateRenderers(bool activeState)
    {
        bool state = activeState != HideOnExecute;

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].enabled = state;
        }
        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = state;
        }
    }


    public enum StateChangeType : byte
    {
        HideOnExecute,
        ShowOnExecute
    }
}
