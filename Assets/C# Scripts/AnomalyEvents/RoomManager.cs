using Fire_Pixel.Utility;
using UnityEngine;


public class RoomManager : MonoBehaviour
{
    public static RoomManager Instance { get; private set; }


    [SerializeField] private Room[] rooms;
    [SerializeField] private MinMaxFloat anomalySpawnInterval;
    [SerializeField] private NativeSampledAnimationCurve spawnCurveOverTime = NativeSampledAnimationCurve.Default;

    public static int CurrentRoomId;
    public static float ElapsedPlayTime;

    private static float nextEventTime;


    public void Init()
    {
        Instance = this;

        int roomCount = transform.childCount;
        rooms = new Room[roomCount];

        for (int i = 0; i < roomCount; i++)
        {
            rooms[i] = transform.GetChild(i).GetComponent<Room>();
        }

        // Activate the first room on start
        rooms[0].ToggleRoomState();

        CameraDisplayManager.Instance.Init();

        spawnCurveOverTime.Bake();
        ElapsedPlayTime = 0;
        nextEventTime = 0;
        UpdateScheduler.RegisterUpdate(OnUpdate);
    }

    private void OnUpdate()
    {
        if (CameraDisplayManager.IsSwappingCamera) return;

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

            nextEventTime = ElapsedPlayTime + EzRandom.Range(anomalySpawnInterval);// * spawnCurveOverTime.Evaluate(HUD.Instance.GamePercentage);
        }
    }

    public void SwapToNextRoom(bool reversed)
    {
        // Toggle current room (off)
        rooms[CurrentRoomId].ToggleRoomState();

        // Cycle to next room index
        CurrentRoomId.AddSmart(reversed ? 1 : -1, rooms.Length);

        // Toggle next room (on)
        rooms[CurrentRoomId].ToggleRoomState();

        // Update HUD
        HUD.UpdateRoomName(rooms[CurrentRoomId].RoomName);
    }

    private void OnDestroy()
    {
        UpdateScheduler.UnRegisterUpdate(OnUpdate);
        spawnCurveOverTime.Dispose();
    }
}
