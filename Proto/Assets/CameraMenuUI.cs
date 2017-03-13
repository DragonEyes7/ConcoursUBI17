using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMenuUI : MonoBehaviour {

    private CamerasController _cameraController;

    private void OnEnable()
    {
        _cameraController = FindObjectOfType<CamerasController>();
        List<string> groups = _cameraController.GetCameraGroupList();
        foreach (string s in groups)
        {

        }
        
        //StartCoroutine(ReadInput());
    }
    
    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
     
    //OnEnable, OnDisable, ReadInput
}
