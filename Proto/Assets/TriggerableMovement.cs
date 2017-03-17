using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerableMovement : Interactive {

    [SerializeField]
    Vector3 rotation;
    [SerializeField]
    bool moveMoreThanOnce;
    [SerializeField]
    GameObject tablette;
    bool active = true;

    InteractiveObjectRecorder _interactiveObjectRecorder;
    Action _previousAction;
    AudioSource _AudioSource;
    // Use this for initialization
    new void Start () {
        base.Start();
        _AudioSource = GetComponent<AudioSource>();
        m_SelectMat = Resources.Load<Material>("MAT_OutlineAgent");
        _interactiveObjectRecorder = GetComponent<InteractiveObjectRecorder>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void Interact()
    {
        _interactiveObjectRecorder.ObjectInteraction(!_interactiveObjectRecorder.GetStatus(), moveMoreThanOnce);
    }

    void OnTriggerExit(Collider c)
    {
        if (active)
        {
            tablette.transform.Rotate(new Vector3(rotation.x, 0, 0), rotation.x);
            tablette.transform.Rotate(new Vector3(0, rotation.y, 0), rotation.y);
            tablette.transform.Rotate(new Vector3(0, 0, rotation.z), rotation.z);
            active = false;
        }
        
    }
    
    public override void MoveObject()
    {
        m_IsActivated = !m_IsActivated;
        //_AudioSource.Play();
    }

    public override void ResetObject()
    {
        tablette.transform.Rotate(new Vector3(rotation.x, 0, 0), -rotation.x);
        tablette.transform.Rotate(new Vector3(0, rotation.y, 0), -rotation.y);
        tablette.transform.Rotate(new Vector3(0, 0, rotation.z), -rotation.z);
        m_IsActivated = true;
    }
}
