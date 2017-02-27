 using UnityEngine;

public static class TimeStopper
{
    public static void StopTime()
    {
        Time.timeScale = 0f;
    }

    public static void StartTime()
    {
        Time.timeScale = 1f;
    }
}