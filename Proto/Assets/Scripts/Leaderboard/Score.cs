using System;

[Serializable]
public class Score
{
    public string Name { get; private set; }
    public int Time { get; private set; }
    public int Penality { get; private set; }

    public Score(string name, int time, int penality)
    {
        Name = name;
        Time = time;
        Penality = penality;
    }
}