using System;

public class BakeTime
{
    /// <summary>焼き時間</summary>
    private readonly float Value;
    public BakeTime(float time)
    {
        if (time<0f)
        {
            throw new ArgumentException($"不正な値です{time}");
        }
        Value = time;
    }
}
