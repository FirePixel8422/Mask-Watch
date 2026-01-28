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
        System.DateTime now = System.DateTime.Now;
        timeTextObj.text = now.ToString("HH:mm:ss");
    }

    public static void UpdateRoomName(string roomName)
    {
        Instance.roomTextObj.text = roomName;
    }
}