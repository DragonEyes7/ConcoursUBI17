using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMenuUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void Toggle()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
     
    //OnEnable, OnDisable, ReadInput
}
