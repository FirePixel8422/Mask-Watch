using UnityEngine;


public class RoomManager : UpdateMonoBehaviour
{
    private static RoomManager Instance { get; set; }

    [SerializeField] private Room[] rooms;
    private static int cRoomId;


    private void Awake()
    {
        Instance = this;

        for (int i = 0; i < rooms.Length; i++)
        {
            Instantiate(rooms[i], 50 * i * Vector3.down, Quaternion.identity);
        }

        // Activate the first room on start
        rooms[0].ToggleRoomState();
    }

    public static void LoadNextRoom()
    {
        // Toggle current room (off)
        Instance.rooms[cRoomId].ToggleRoomState();

        // Cycle to next room index
        cRoomId.IncrementSmart(Instance.rooms.Length);

        // Toggle next room (on)
        Instance.rooms[0].ToggleRoomState();
    }
}
