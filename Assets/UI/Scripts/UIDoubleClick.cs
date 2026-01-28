using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UIDoubleClick : MonoBehaviour, IPointerClickHandler
{
    [Tooltip("Max tijd tussen 2 klikken om als dubbelklik te tellen (seconden).")]
    public float doubleClickTime = 0.3f;

    public UnityEvent onDoubleClick;

    private float lastClickTime = -999f;

    public void OnPointerClick(PointerEventData eventData)
    {
        // Alleen linker muisknop
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        // Als 2e klik snel genoeg na de 1e komt -> dubbelklik
        if (Time.unscaledTime - lastClickTime <= doubleClickTime)
        {
            lastClickTime = -999f; // reset zodat triple-click niet 2x triggert
            onDoubleClick?.Invoke();
        }
        else
        {
            lastClickTime = Time.unscaledTime;
        }
    }
}
