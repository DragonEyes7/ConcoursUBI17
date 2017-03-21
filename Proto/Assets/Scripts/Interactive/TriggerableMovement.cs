using UnityEngine;

public class TriggerableMovement : Interactive
{
    [SerializeField]Vector3 rotation;
    [SerializeField]bool moveMoreThanOnce;
    [SerializeField]GameObject tablette;

    InteractiveObjectRecorder _interactiveObjectRecorder;
    Action _previousAction;
    AudioSource _AudioSource;

    new void Start ()
    {
        base.Start();
        _AudioSource = GetComponent<AudioSource>();
        m_SelectMat = Resources.Load<Material>("MAT_OutlineAgent");
        _interactiveObjectRecorder = GetComponent<InteractiveObjectRecorder>();
    }

    public override void Interact()
    {
        _interactiveObjectRecorder.ObjectInteraction(!_interactiveObjectRecorder.GetStatus());
    }

    void OnTriggerStay(Collider c)
    {
        if (!m_IsActivated && c.GetComponent<AgentActions>()) Interact();
    }
    
    public override void MoveObject()
    {
        _AudioSource.Play();
        Debug.Log("MoveObject");
        tablette.transform.localEulerAngles = rotation;
        m_IsActivated = true;
    }

    public override void ResetObject()
    {
        Debug.Log("ResetObject");
        tablette.transform.localEulerAngles = Vector3.zero;
        m_IsActivated = false;
    }
}
