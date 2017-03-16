using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerableMovement : Interactive {

    //USE ONLY WITH SHOP2 for now

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

    }

    void OnTriggerEnter(Collider c)
    {
        if (active)
        {
            tablette.transform.Rotate(new Vector3(0, 0, 1), 90);
            tablette.transform.position += new Vector3(-1.75f,0,-1.5f);
            active = false;
        }
        
    }
    
    public override void MoveObject()
    {
        m_IsActivated = !m_IsActivated;
        _AudioSource.Play();
    }

    public override void ResetObject()
    {
        m_IsActivated = false;
    }
}
