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

    public override void Interact()
    {
        _interactiveObjectRecorder.ObjectInteraction(!_interactiveObjectRecorder.GetStatus(), moveMoreThanOnce);
    }

    void OnTriggerExit(Collider c)
    {
        if(active)Interact();
    }
    
    public override void MoveObject()
    {
        m_IsActivated = !m_IsActivated;
        //_AudioSource.Play();
        tablette.transform.Rotate(new Vector3(rotation.x, 0, 0), rotation.x);
        tablette.transform.Rotate(new Vector3(0, rotation.y, 0), rotation.y);
        tablette.transform.Rotate(new Vector3(0, 0, rotation.z), rotation.z);
        active = false;
        m_IsActivated = true;

    }

    public override void ResetObject()
    {
        if (!active)
        {
            tablette.transform.Rotate(new Vector3(rotation.x, 0, 0), -rotation.x);
            tablette.transform.Rotate(new Vector3(0, rotation.y, 0), -rotation.y);
            tablette.transform.Rotate(new Vector3(0, 0, rotation.z), -rotation.z);
        }
        active = true;
        m_IsActivated = false;
    }
}
