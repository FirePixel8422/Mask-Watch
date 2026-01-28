using UnityEngine;


[System.Serializable]
public struct RoomEventExecuteOptions
{
    public bool AllowDuplicates;
    [Range(1, 10)]
    public int MaxExecutionCount;

    public bool IsOutOfExecutions => MaxExecutionCount == 0;
}