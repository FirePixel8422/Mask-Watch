


[System.Serializable]
public struct RoomEventExecuteOptions
{
    public bool AllowDuplicates;
    public int MaxExecutionCount;

    public bool IsOutOfExecutions => MaxExecutionCount == 0;
}