using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager inputManager;

    void Awake()
    {
        if(inputManager == null)
        {
            DontDestroyOnLoad(gameObject);
            inputManager = this;
        }
        else if(inputManager != this)
        {
            Destroy(gameObject);
        }

		InputMode.isKeyboardMode = true;
	}

    void Update()
    {
        if(InputMode.isKeyboardMode)
        {
            if (Input.GetKeyDown("joystick button 0")
                || Input.GetKeyDown("joystick button 1")
                || Input.GetKeyDown("joystick button 2")
                || Input.GetKeyDown("joystick button 3")
                || Input.GetAxisRaw("ControllerHorizontal") != 0
                || Input.GetAxisRaw("ControllerVertical") != 0
                || Input.GetAxisRaw("CameraHorizontal") != 0
                || Input.GetAxisRaw("CameraVertical") != 0)
            {
                InputMode.isKeyboardMode = false;
            }
        }
    }
	
	void OnGUI ()
    {
        if ((Event.current.type == EventType.KeyDown || Event.current.type == EventType.MouseMove) && !InputMode.isKeyboardMode)
        {
            InputMode.isKeyboardMode = true;
        }
    }

    void OnApplicationQuit()
    {
        Debug.Log("Application ending after " + Time.time + " seconds");
    }
}
