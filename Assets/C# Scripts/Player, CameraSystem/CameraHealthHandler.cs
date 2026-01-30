using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;



public class CameraHealthHandler : UpdateMonoBehaviour
{
    public static CameraHealthHandler Instance { get; private set; }


    [SerializeField] private NativeSampledAnimationCurve cameraStrengthCurve = NativeSampledAnimationCurve.Default;
    [SerializeField] private Material cameraNoiseMaterial;

    [SerializeField] private float noiseChangeSpeed = 0.1f;

    [SerializeField] private float maxAnomalySeverity = 25;
    [SerializeField] private float currentAnomalySeverity;

    private float noiseStrength;
    private float targetNoiseStrength;

    private static readonly int NOISE_STRENGTH_ID = Shader.PropertyToID("_Alpha");

    public float CurrentAnomalySeverity
    {
        get => currentAnomalySeverity;
        set
        {
            currentAnomalySeverity = value;

            noiseStrength = cameraStrengthCurve.Evaluate(math.saturate(currentAnomalySeverity / maxAnomalySeverity));
        }
    }


    private void Awake()
    {
        Instance = this;
        cameraStrengthCurve.Bake();

        cameraNoiseMaterial.SetFloat(NOISE_STRENGTH_ID, 0);
    }
    protected override void OnUpdate()
    {
        if (CameraDisplayManager.IsSwappingCamera) return;

        targetNoiseStrength = Mathf.MoveTowards(targetNoiseStrength, noiseStrength, noiseChangeSpeed * Time.deltaTime);
        cameraNoiseMaterial.SetFloat(NOISE_STRENGTH_ID, targetNoiseStrength);

        if (targetNoiseStrength >= 1)
        {
            HUD.Instance.LoseGame();
        }
    }

    private void OnDestroy()
    {
        cameraNoiseMaterial.SetFloat(NOISE_STRENGTH_ID, 0);
        cameraStrengthCurve.Dispose();
    }
}