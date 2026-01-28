using UnityEngine;

public class ObjectStateChange : ObjectEvent
{
    public StateChangeType stateChangeType = StateChangeType.HideOnExecute;
    public bool HideOnExecute => stateChangeType == StateChangeType.HideOnExecute;



    protected override void Awake()
    {
        base.Awake();
        gameObject.SetActive(HideOnExecute);
    }

    protected override void OnExecute()
    {
        gameObject.SetActive(HideOnExecute == false);
    }
    protected override void OnReported()
    {
        gameObject.SetActive(HideOnExecute);
    }


    public enum StateChangeType : byte
    {
        HideOnExecute,
        ShowOnExecute
    }
}
