using UnityEngine;

public class Movement : MovementSounds
{
    protected MainRecorder m_Recorder;
	
	new void Start ()
    {
        base.Start();
        m_Recorder = FindObjectOfType<MainRecorder>();
	}
}