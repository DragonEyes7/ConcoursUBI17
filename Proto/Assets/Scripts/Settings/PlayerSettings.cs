public static class PlayerSettings
{
    public enum ControllerScheme
    {
        XBOX = 0,
        PS4 = 1,
        Last = PS4,
        Length
    };
    #region Declaration
    static int m_AccountID;
    static float m_CameraDistance = 3f;
    static float[] m_CameraSpeed = { 150f, 100f }; //0 = mouse 1 = gamepad
    static float m_MasterVolume = 0.5f;
    static float m_MusicVolume = 0.5f;
    static float m_SFXVolume = 0.5f;
    static float m_MyFootStepVolume = 1f;
    static float m_OtherFootStepVolume = 0.5f;
    static int m_AudioSpeakerMode = 2;
    static int m_ControllerID = 0;
    static bool m_IsBanned = false;
    #endregion

    #region Getters / Setters
    public static int AccountID
    {
        get { return m_AccountID; }
        set { m_AccountID = value; }
    }

    public static float CameraDistance
	{
		get { return m_CameraDistance; }
		set { m_CameraDistance = value; }
	}

    public static float GetCameraSpeed(int mouse0)
    {
        return m_CameraSpeed[mouse0];
    }

    public static void SetCameraSpeed(int mouse0, float amount)
    {
        m_CameraSpeed[mouse0] = amount;
    }

    public static float MasterVolume
    {
        get { return m_MasterVolume; }
        set { m_MasterVolume = value; }
    }

    public static float MusicVolume
    {
        get { return m_MusicVolume; }
        set { m_MusicVolume = value; }
    }

    public static float SFXVolume
    {
        get { return m_SFXVolume; }
        set { m_SFXVolume = value; }
    }

    public static float MyFootStepVolume
    {
        get { return m_MyFootStepVolume; }
        set { m_MyFootStepVolume = value; }
    }

    public static float OtherFootStepVolume
    {
        get { return m_OtherFootStepVolume; }
        set { m_OtherFootStepVolume = value; }
    }

    public static int AudioSpeakerMode
    {
        get { return m_AudioSpeakerMode; }
        set { m_AudioSpeakerMode = value; }
    }


    public static int ControllerID
    {
        get { return m_ControllerID; }
        set { m_ControllerID = value; }
    }

    public static bool isBanned
    {
        get { return m_IsBanned; }
        set { m_IsBanned = value; }
    }
    #endregion

    public static string AudioSpeakerModeString()
    {
        string text = "";

        switch (m_AudioSpeakerMode)
        {
            case 0:
                text = "Raw";
                break;
            case 1:
                text = "Mono";
                break;
            case 2:
                text = "Stereo";
                break;
            case 3:
                text = "Quad";
                break;
            case 4:
                text = "Surround";
                break;
            case 5:
                text = "5.1";
                break;
            case 6:
                text = "7.1";
                break;
            case 7:
                text = "Prologic";
                break;
            default:
                text = "AudioModeNotFound";
                break;
        }

        return text;
    }

    public static string ControllerModeString()
    {
        string text = "";

        switch (m_ControllerID)
        {
            case 0:
                text = "XBOX";
                break;
            case 1:
                text = "PS4";
                break;
            default:
                text = "ControllerModeNotFound";
                break;
        }

        return text;
    }
}
