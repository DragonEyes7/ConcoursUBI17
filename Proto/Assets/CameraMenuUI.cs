using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMenuUI : MonoBehaviour {

    private Transform _arrow;
    private Text[] _textAreas; 
    private CamerasController _cameraController;

    private void OnEnable()
    {
        _cameraController = FindObjectOfType<CamerasController>();
        _textAreas = GetComponentsInChildren<Text>();
        foreach (Transform child in transform)
        {
            if (child.name.IndexOf("pivotArrow", StringComparison.InvariantCultureIgnoreCase) > -1) _arrow = child;
            
        }
        List<string> groups = _cameraController.GetCameraGroupList();
        for (int i = 0; i < groups.Count; i++)
        {
            _textAreas[i].text = groups[i];
        }
        

        StartCoroutine(ReadInput());
    }

    private IEnumerator ReadInput()
    {
        float pivotRotation = Mathf.Atan2(-Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * Mathf.Rad2Deg;

        if (Input.GetButtonDown("Action"))
        {
            SwitchCamera(pivotRotation);
            gameObject.SetActive(false);
        }

        //Debug.Log("Rot: " + Mathf.Atan2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * Mathf.Rad2Deg);

        _arrow.rotation = Quaternion.Euler(0f, 0f, pivotRotation);

        yield return new WaitForSecondsRealtime(0.01f);
        StartCoroutine(ReadInput());
    }

    public void SwitchCamera(float selectionAngle)
    {
        //1. Get the selected Group
        //2. Get the first cam from the group
        //3. Switch the selected camera
    }

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
     
    //OnDisable, ReadInput
}
