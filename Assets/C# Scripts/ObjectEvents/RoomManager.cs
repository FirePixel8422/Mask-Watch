using UnityEngine;


public class RoomManager : UpdateMonoBehaviour
{
    private static RoomManager Instance { get; set; }


    [SerializeField] private Room[] roomPrefabs;
    [SerializeField] private Room[] rooms;
    [SerializeField] private MinMaxFloat eventDelayMinMax;

    public static int CurrentRoomId;
    private static float elapsedPlayTime;
    private static float nextEventTime;


    private void Awake()
    {
        Instance = this;

        int roomCount = roomPrefabs.Length;
        rooms = new Room[roomCount];

        for (int i = 0; i < roomCount; i++)
        {
            rooms[i] = Instantiate(roomPrefabs[i], 50 * i * Vector3.down, Quaternion.identity);
        }

        // Activate the first room on start
        rooms[0].ToggleRoomState();

        // Static Instance??
        // Static Instance??
        // Static Instance??
        this.FindObjectOfType<CameraDisplayManager>().Init();
    }

    protected override void OnUpdate()
    {
        elapsedPlayTime += Time.deltaTime;

        if (elapsedPlayTime >= nextEventTime)
        {
            bool eventExecuted;
            int roomCount = rooms.Length;

            int randomRoomId = Random.Range(0, roomCount);
            int startRoomId = randomRoomId;

            do
            {
                // Trigger a random event in a random room
                eventExecuted = rooms[randomRoomId].ExecuteRandomEvent(elapsedPlayTime);
                randomRoomId.IncrementSmart(roomCount);
            }
            while (eventExecuted == false && startRoomId != randomRoomId);

            nextEventTime = elapsedPlayTime + EzRandom.Range(eventDelayMinMax);
        }
    }

    public static void LoadNextRoom()
    {
        // Toggle current room (off)
        Instance.rooms[CurrentRoomId].ToggleRoomState();

        // Cycle to next room index
        CurrentRoomId.IncrementSmart(Instance.rooms.Length);

        // Toggle next room (on)
        Instance.rooms[CurrentRoomId].ToggleRoomState();
    }
}
