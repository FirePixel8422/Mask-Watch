using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SimpleSettings : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text resolutionText;
    public Toggle fullscreenToggle;
    public Button resolutionUpButton;
    public Button resolutionDownButton;

    [Header("16:9 Resolutions")]
    public Vector2Int[] resolutions = new Vector2Int[]
    {
        new Vector2Int(1280, 720),
        new Vector2Int(1600, 900),
        new Vector2Int(1920, 1080),
        new Vector2Int(2560, 1440)
    };

    private int currentResolutionIndex;

    const string RES_KEY = "resolutionIndex";
    const string FULLSCREEN_KEY = "fullscreen";

    void Start()
    {
        // Load prefs
        currentResolutionIndex = PlayerPrefs.GetInt(RES_KEY, 2);
        bool fullscreen = PlayerPrefs.GetInt(FULLSCREEN_KEY, 1) == 1;

        fullscreenToggle.isOn = fullscreen;
        Screen.fullScreen = fullscreen;

        // Button listeners
        resolutionUpButton.onClick.AddListener(ResolutionUp);
        resolutionDownButton.onClick.AddListener(ResolutionDown);
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);

        ApplyResolution();
    }

    void ResolutionUp()
    {
        if (currentResolutionIndex < resolutions.Length - 1)
            currentResolutionIndex++;

        ApplyResolution();
        SaveResolution();
    }

    void ResolutionDown()
    {
        if (currentResolutionIndex > 0)
            currentResolutionIndex--;

        ApplyResolution();
        SaveResolution();
    }

    void ApplyResolution()
    {
        Vector2Int res = resolutions[currentResolutionIndex];
        Screen.SetResolution(res.x, res.y, Screen.fullScreen);
        resolutionText.text = res.x + " x " + res.y;
    }

    void SaveResolution()
    {
        PlayerPrefs.SetInt(RES_KEY, currentResolutionIndex);
        PlayerPrefs.Save();
    }

    public void SetFullscreen(bool value)
    {
        Screen.fullScreen = value;
        PlayerPrefs.SetInt(FULLSCREEN_KEY, value ? 1 : 0);
        PlayerPrefs.Save();
    }
    public void QuitGame()
    {
        Application.Quit();

        // Voor in de Unity Editor (anders lijkt het alsof hij niets doet)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

}
