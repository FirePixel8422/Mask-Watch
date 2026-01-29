using UnityEngine;


public class RoomManager : UpdateMonoBehaviour
{
    private static RoomManager Instance { get; set; }


    [SerializeField] private Room[] rooms;
    [SerializeField] private MinMaxFloat eventDelayMinMax;

    public static int CurrentRoomId;
    public static float ElapsedPlayTime;

    private static float nextEventTime;


    private void Start()
    {
        Instance = this;

        int roomCount = transform.childCount;
        rooms = new Room[roomCount];

        for (int i = 0; i < roomCount; i++)
        {
            rooms[i] = transform.GetChild(i).GetComponent<Room>();
            rooms[i].transform.position = 50 * i * Vector3.down;
        }

        // Activate the first room on start
        rooms[0].ToggleRoomState();

        CameraDisplayManager.Instance.Init();
    }

    protected override void OnUpdate()
    {
        ElapsedPlayTime += Time.deltaTime;

        if (ElapsedPlayTime >= nextEventTime)
        {
            bool eventExecuted;
            int roomCount = rooms.Length;

            int randomRoomId = Random.Range(0, roomCount);
            int startRoomId = randomRoomId;

            do
            {
                // Trigger a random event in a random room
                eventExecuted = rooms[randomRoomId].ExecuteRandomEvent(ElapsedPlayTime);
                randomRoomId.IncrementSmart(roomCount);
            }
            while (eventExecuted == false && startRoomId != randomRoomId);

            nextEventTime = ElapsedPlayTime + EzRandom.Range(eventDelayMinMax);
        }
    }

    public static void SwapToNextRoom(bool reversed)
    {
        // Toggle current room (off)
        Instance.rooms[CurrentRoomId].ToggleRoomState();

        // Cycle to next room index
        CurrentRoomId.AddSmart(reversed ? 1 : -1, Instance.rooms.Length);

        // Toggle next room (on)
        Instance.rooms[CurrentRoomId].ToggleRoomState();

        // Update HUD
        HUD.UpdateRoomName(Instance.rooms[CurrentRoomId].RoomName);
    }
}
