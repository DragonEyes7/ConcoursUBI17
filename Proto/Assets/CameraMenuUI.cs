using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMenuUI : MonoBehaviour {

    private Transform _arrow;
    private Text[] _textAreas; 
    private CamerasController _cameraController;
    private List<string> _cameraGroups;
    private GameObject _currentCam;

    private void OnEnable()
    {
        _cameraController = FindObjectOfType<CamerasController>();
        _textAreas = GetComponentsInChildren<Text>();
        _currentCam = _cameraController.GetActiveCamera();
        _currentCam.GetComponent<CameraMovement>().enabled = false;
        foreach (Transform child in transform)
        {
            if (child.name.IndexOf("pivotArrow", StringComparison.InvariantCultureIgnoreCase) > -1) _arrow = child;
            
        }
        _cameraGroups = _cameraController.GetCameraGroupList();
        for (int i = 0; i < _cameraGroups.Count; i++)
        {
            _textAreas[i].text = _cameraGroups[i];
        }
        

        StartCoroutine(ReadInput());
    }

    private void OnDisable()
    {
        _currentCam.GetComponent<CameraMovement>().enabled = true;
    }

    private IEnumerator ReadInput()
    {
        float pivotRotation = Mathf.Atan2(-Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * Mathf.Rad2Deg;

        if (Input.GetButtonDown("Action"))
        {
            SwitchCamera(-pivotRotation);
            gameObject.SetActive(false);
        }
        
        _arrow.rotation = Quaternion.Euler(0f, 0f, pivotRotation);

        yield return new WaitForSecondsRealtime(0.01f);
        StartCoroutine(ReadInput());
    }

    public void SwitchCamera(float selectionAngle)
    {
        Debug.Log("Angle: " + selectionAngle);
        //1. Get the selected Group
        //Turn negatives into positive
        if (selectionAngle < 0) selectionAngle = 360 + selectionAngle;
        //selectionAngle = selectionAngle % 360;
        int groupNumber = (int)selectionAngle / 45;
        if (groupNumber < _cameraController.GetCameraGroupList().Count)
        {
            //2. Get the first cam from the group
            GameObject nextCamera = _cameraController.GetGroupCameras(_cameraGroups[groupNumber])[0];
            //3. Switch the selected camera
            _cameraController.SetActiveCamera(nextCamera, _currentCam);
            _currentCam = nextCamera;
        }
    }

    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
     
    //OnDisable, ReadInput
}
