using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]float _CameraSpeed = 50f;
    [SerializeField]float _ZoomLimit = 30f;
    [SerializeField]float _ZoomSpeed = 50f;
    [SerializeField]float _YMin = -40f;
    [SerializeField]float _YMax = 50f;
    [SerializeField] float _XMin;
    [SerializeField]float _XMax;
    [SerializeField]bool _YLock = true;
    [SerializeField]bool _XLock = true;
    Vector2 _Input;
    Camera _Camera;

    float _CurrentX;
    float _CurrentY;
    float _MaxZoomOut;

    AudioSource AS_Move, AS_Zoom, AS_ServoStop;

    bool _IsMoving = false, _IsZooming = false;

    void OnEnable()
    {
        if (!_Camera)
        {
            _Camera = GetComponentInChildren<Camera>();
            _MaxZoomOut = _Camera.fieldOfView;
            ResetPosition();
        }

        if (!AS_Move)
        {
            AudioSource[] sounds = GetComponents<AudioSource>();
            AS_Move = sounds[0];
            AS_Zoom = sounds[1];
            AS_ServoStop = sounds[2];
        }

        _Camera.fieldOfView = _MaxZoomOut;

        UpdatePosition();
    }

    void Update()
    {
        _Input.Set(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Move();
        if (_IsMoving)
        {
            if (_Input.magnitude == 0)
            {
                AS_ServoStop.Play();
                AS_Move.Stop();
                _IsMoving = false;
            }
        }
        else
        {
            if (_Input.magnitude != 0)
            {
                AS_Move.Play();
                AS_ServoStop.Stop();
                _IsMoving = true;
            }
        }

        if (InputMode.isKeyboardMode)
        {
            Zoom(Input.GetAxis("Mouse ScrollWheel") * -_ZoomSpeed * 25);
        }
        else
        {
            Zoom(Input.GetAxis("Zoom") * _ZoomSpeed);
        }
    }

    void LateUpdate()
    {
        UpdatePosition();
    }

    void Move()
    {
        _CurrentX += _Input.x * (_Camera.fieldOfView / _MaxZoomOut) * Time.deltaTime * _CameraSpeed;
        _CurrentY += _Input.y * (_Camera.fieldOfView / _MaxZoomOut) * Time.deltaTime * _CameraSpeed;

        if (_YLock)
        {
            _CurrentY = Mathf.Clamp(_CurrentY, _YMin, _YMax);
        }

        if (_XLock)
        {
            _CurrentX = Mathf.Clamp(_CurrentX, _XMin, _XMax);
        }
    }

    void UpdatePosition()
    {
        transform.rotation = Quaternion.Euler(_CurrentY, _CurrentX, 0);
    }

    void Zoom(float zooming)
    {
        if (_IsZooming)
        {
            if (zooming == 0.0f)
            {
                AS_Zoom.Stop();
                _IsZooming = false;
            }
        }
        else
        {
            if (zooming != 0.0f)
            {
                AS_Zoom.Play();
                _IsZooming = true;
            }
        }

        _Camera.fieldOfView += zooming * Time.deltaTime;
        if (_Camera.fieldOfView > _MaxZoomOut)
        {
            _Camera.fieldOfView = _MaxZoomOut;
            AS_Zoom.Stop();
        }

        if (_Camera.fieldOfView < _ZoomLimit)
        {
            _Camera.fieldOfView = _ZoomLimit;
            AS_Zoom.Stop();
        }
    }

    public void ResetPosition()
    {
        _CurrentX = transform.eulerAngles.y < _XMin ? _XMin : transform.eulerAngles.y;
        _CurrentY = transform.eulerAngles.x < _YMin ? _YMin : transform.eulerAngles.x;
        UpdatePosition();
    }
}