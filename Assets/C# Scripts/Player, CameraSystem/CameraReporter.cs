using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;


public class CameraReporter : UpdateMonoBehaviour
{
    [SerializeField] private float reportStartTime = 1;
    [SerializeField] private float reportDecayTime = 0.25f;

    [SerializeField] private float reportPendingTime = 5;
    [SerializeField] private float succesfullReportCleanupTime = 5;
    [SerializeField] private float failedReportCleanupTime = 5;

    [SerializeField] private Image image;
    [SerializeField] private Animator selectionAnim;

    [SerializeField] private GameObject fixingAnomalyTextObj;
    [SerializeField] private GameObject noAnomalyFoundTextObj;

    private static float selectionProcess;
    private bool isReporting;
    public static bool IsTryingToReport => selectionProcess > 0;


    protected override void OnUpdate()
    {
        if (Application.isFocused == false) return;

        if (selectionProcess == 0)
        {
            image.transform.position = Input.mousePosition;
        }

        if (isReporting == false)
        {
            selectionProcess = Input.GetMouseButton(0) ?
                math.saturate(selectionProcess + Time.deltaTime / reportStartTime) :
                math.saturate(selectionProcess - Time.deltaTime / reportDecayTime);

            image.fillAmount = selectionProcess;

            if (selectionProcess == 1)
            {
                isReporting = true;
                selectionAnim.SetBool("Reporting", true);

                bool hasFoundAnomaly = false;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(image.transform.position), out RaycastHit hitInfo))
                {
                    if (hitInfo.transform.TryGetComponentInParentRecursively(true, out AnomalyEvent roomObjEvent))
                    {
                        hasFoundAnomaly = roomObjEvent.TryReport(reportPendingTime);
                    }
                }
                this.Invoke(reportPendingTime, () =>
                {
                    ResolveAnomalyReport(hasFoundAnomaly);
                });
            }
        }
    }

    private void ResolveAnomalyReport(bool hasFoundAnomaly)
    {
        image.fillAmount = 0;

        float reportCleanupTime = hasFoundAnomaly ? succesfullReportCleanupTime : failedReportCleanupTime;

        // Show report result on static screen
        CameraDisplayManager.Instance.SwapToStaticScreen(reportCleanupTime);

        fixingAnomalyTextObj.SetActive(hasFoundAnomaly);
        noAnomalyFoundTextObj.SetActive(hasFoundAnomaly == false);

        Invoke(nameof(EndReportCycle), reportCleanupTime);
    }
    private void EndReportCycle()
    {
        isReporting = false;
        selectionAnim.SetBool("Reporting", false);

        fixingAnomalyTextObj.SetActive(false);
        noAnomalyFoundTextObj.SetActive(false);

        selectionProcess = 0;
    }
}