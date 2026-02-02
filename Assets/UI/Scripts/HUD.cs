using UnityEngine;
using TMPro;


public class HUD : UpdateMonoBehaviour
{
    public static HUD Instance;
    private void Awake()
    {
        Instance = this;
    }


    [SerializeField] private TextMeshProUGUI timeTextObj;
    [SerializeField] private TextMeshProUGUI roomTextObj;

    [SerializeField] private TextMeshProUGUI youWinTextObj;
    [SerializeField] private TextMeshProUGUI youLoseTextObj;

    [SerializeField] private float timeMultiplier;

    public SceneReloader reloader;
    [SerializeField] private float winLoseDelay = 3;

    public float GamePercentage => 360 / RoomManager.ElapsedPlayTime * timeMultiplier;

    protected override void OnUpdate()
    {
        float elapsed = RoomManager.ElapsedPlayTime * timeMultiplier;

        // total "minutes" in your game-time
        int totalMinutes = Mathf.FloorToInt(elapsed);

        // snap to 10-minute blocks
        int snappedMinutes = (totalMinutes / 10) * 10;

        int hours = snappedMinutes / 60;
        int minutes = snappedMinutes % 60;

        if (hours >= 6)
        {
            WinGame();
        }

        timeTextObj.text = $"{hours:00}:{minutes:00}";
    }

    public void WinGame()
    {
        CameraHealthHandler.Instance.CurrentAnomalySeverity = -10;

        youWinTextObj.enabled = true;

        this.Invoke(winLoseDelay, () =>
        {
            reloader.ReloadScene();
        });
    }
    public void LoseGame()
    {
        youLoseTextObj.enabled = true;

        this.Invoke(winLoseDelay, () =>
        {
            reloader.ReloadScene();
        });
    }

    public static void UpdateRoomName(string roomName)
    {
        Instance.roomTextObj.text = roomName;
    }
}