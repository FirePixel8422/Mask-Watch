using UnityEngine;
using TMPro;



public class HUD : UpdateMonoBehaviour
{
    private static HUD Instance;
    private void Awake()
    {
        Instance = this;
    }


    [SerializeField] private TextMeshProUGUI timeTextObj;
    [SerializeField] private TextMeshProUGUI roomTextObj;


    protected override void OnUpdate()
    {
        float elapsed = RoomManager.ElapsedPlayTime;

        // total "minutes" in your game-time
        int totalMinutes = Mathf.FloorToInt(elapsed);

        // snap to 10-minute blocks
        int snappedMinutes = (totalMinutes / 10) * 10;

        int hours = snappedMinutes / 60;
        int minutes = snappedMinutes % 60;

        timeTextObj.text = $"{hours:00}:{minutes:00}";
    }

    public static void UpdateRoomName(string roomName)
    {
        Instance.roomTextObj.text = roomName;
    }
}