using System;
using UnityEngine;

public class ClockUI : MonoBehaviour
{
    private Transform _seconds;
    private Transform _minutes;
    private Transform _arrow;
    [SerializeField] private int _speed = 10;

    private void Start()
    {
        foreach (Transform child in transform)
        {
            if (_seconds != null && _minutes != null) break;
            if (child.name.IndexOf("pivotSeconds", StringComparison.InvariantCultureIgnoreCase) > -1) _seconds = child;
            else if (child.name.IndexOf("pivotMinutes", StringComparison.InvariantCultureIgnoreCase) > -1) _minutes = child;
            else if (child.name.IndexOf("pivotArrow", StringComparison.InvariantCultureIgnoreCase) > -1) _arrow = child;
        }
    }

    private void Update()
    {
        RotateLikeAClock();
    }

    private bool a = true;
    private void RotateLikeAClock()
    {
        var seconds = 15;
        if(a)
        _seconds.Rotate(0f, 0f, -(360 / 60 * seconds + 2));

        a = false;

        //_seconds.Rotate(Vector3.forward * Time.deltaTime * (_speed * 12));
        _arrow.Rotate(Vector3.forward * Time.deltaTime * (_speed * 12));
        _minutes.Rotate(Vector3.forward * Time.deltaTime * _speed);
    }
}