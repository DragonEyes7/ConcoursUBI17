using System;

[Serializable]
public class Score
{
    public string Name { get; private set; }
    public int Time { get; private set; }

    public Score(string name, int time)
    {
        Name = name;
        Time = time;
    }
}