using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIDoubleClick : MonoBehaviour, IPointerClickHandler
{
    [Header("Double Click")]
    public float doubleClickTime = 0.3f;
    public UnityEvent onDoubleClick;

    private float lastClickTime = -999f;

    [Header("Delayed Activate (for Button)")]
    [SerializeField] private float activateDelay = 0.2f;
    [SerializeField] private GameObject[] objectsToActivate;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        if (Time.unscaledTime - lastClickTime <= doubleClickTime)
        {
            lastClickTime = -999f;
            onDoubleClick?.Invoke();
        }
        else
        {
            lastClickTime = Time.unscaledTime;
        }
    }

    // Hang deze aan je Button OnClick()
    public void ActivateObjectsAfterDelay()
    {
        CancelInvoke(nameof(DoActivate));
        Invoke(nameof(DoActivate), activateDelay);
    }

    private void DoActivate()
    {
        if (objectsToActivate == null) return;

        for (int i = 0; i < objectsToActivate.Length; i++)
        {
            var go = objectsToActivate[i];
            if (go != null) go.SetActive(true);
        }
    }
}
