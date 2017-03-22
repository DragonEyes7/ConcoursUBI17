using UnityEngine;

public class Blinker : MonoBehaviour
{
    [SerializeField]float _Speed = 0.2f;
    Light _Light;
    bool _BlinkUp = true;

	void Start ()
	{
        _Light = GetComponent<Light>();
        _Light.color = Color.red;
        InvokeRepeating("Blink", 0, _Speed);
	}

    void Blink()
    {
        if(_Light.intensity <= 0 && !_BlinkUp)
        {
            _BlinkUp = true;
        }
        else if( _Light.intensity >= 1 && _BlinkUp)
        {
            _BlinkUp = false;
        }

        _Light.intensity += _BlinkUp ? 0.05f : -0.05f;
    }

    public void Interacted()
    {
        _Light.color = Color.green;
        CancelInvoke("Blink");
    }

    public void Inside()
    {
        CancelInvoke("Blink");
        _Light.color = Color.blue;
        InvokeRepeating("Blink", 0, _Speed);
    }
}
