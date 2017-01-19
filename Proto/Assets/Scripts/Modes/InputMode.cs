static public class InputMode
{
	public enum LockState
	{
		SOFTLOCK,
		HARDLOCK,
		AIMING,
		SIZE
	};

	static public bool isKeyboardMode { get; set; }
	static public bool isInMenu { get; set; }
	static public LockState LockMode { get; set; }
}
