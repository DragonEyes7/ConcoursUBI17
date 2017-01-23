using UnityEngine;
using System.Collections.Generic;

public struct RecordState
{
	Vector3 m_Position;
	//int m_AnimState;
	Quaternion m_Rotation;
	//float m_Speed;

	public RecordState(Vector3 position, /*int animState,*/ Quaternion rotation/*, float speed*/)
	{
		m_Position	= position;
		//m_AnimState = animState;
		m_Rotation	= rotation;
		//m_Speed = speed;
	}

	public Vector3 position
	{
		get { return m_Position;  }
		set { m_Position = value;  }
	}

	/*public int animState
	{
		get { return m_AnimState; }
		set { m_AnimState = value; }
	}*/

	public Quaternion rotation
	{
		get { return m_Rotation; }
		set { m_Rotation = value; }
	}

	/*public float speed
	{
		get { return m_Speed; }
		set { m_Speed = value; }
	}*/
}

public class Recorder : MonoBehaviour
{
	[SerializeField]TimeController m_TimeController;

	Dictionary<int, RecordState> m_States = new Dictionary<int, RecordState>();
	Dictionary<int, RecordState> m_Recording;

	//Animator m_Animator;
	Rigidbody m_Rigidbody;

	bool m_IsPlaying = false;
	bool m_IsRecording = true;

    public bool isRecording
    {
        get { return m_IsRecording; }
    }

	void Start()
	{
		//m_Animator = GetComponent<Animator>();
		m_Rigidbody = GetComponent<Rigidbody>();
        m_TimeController = FindObjectOfType<TimeController>();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			m_IsRecording = false;
			SetRecording(m_States);
			m_TimeController.time = 0;
			//m_Animator.SetBool("Transition", false);
		}

		if(Input.GetButtonDown("TimeRewind"))
		{
		    SetTimeRewinding();
		}

		/*if(Input.GetButton("TimeRewind"))
		{
			m_TimeController.isFoward = false;
            int timeToRewind = 3;
            m_TimeController.time -= timeToRewind;
		}*/

		if(Input.GetButtonUp("TimeRewind"))
		{
		    SetTimeForward();
		}
	}

    private void SetTimeForward()
    {
        m_TimeController.isFoward = true;
        m_IsRecording = true;
        m_IsPlaying = false;

        //m_Animator.SetBool("Transition", true);
        if (m_Rigidbody)
        {
            m_Rigidbody.isKinematic = false;
        }
        //m_Animator.SetFloat("GlobalSpeed", m_Animator.GetFloat("GlobalSpeed") * -1f);
    }

    private void SetTimeRewinding()
    {
        SetRecording(m_States);
        m_IsRecording = false;
        m_IsPlaying = true;
        m_TimeController.isFoward = false;
        var timeToRewind = 3;
        m_TimeController.time -= m_TimeController.GetMaxTime(m_TimeController.time - timeToRewind);
    }

    void FixedUpdate()
	{
		if (m_IsRecording)
		{
			m_States[m_TimeController.time] =  new RecordState(transform.position, transform.rotation);
            //m_Animator.GetCurrentAnimatorStateInfo(0).shortNameHash,

            //m_Animator.GetFloat("Speed"));
        }

		if (m_IsPlaying)
		{
            if (m_Recording.ContainsKey(m_TimeController.time))
			{
				PlayState(m_Recording[m_TimeController.time]);
			}
		}

		/*if(m_TimeDebug)
		{
			m_TimeDebug.text = m_TimeController.time.ToString();
		}*/		
	}

	void PlayState(RecordState recordState)
	{
		transform.position = recordState.position;
		//m_Animator.Play(recordState.animState);
		transform.rotation = recordState.rotation;
		//m_Animator.SetFloat("Speed", recordState.speed);
	}

	void SetRecording(Dictionary<int, RecordState> recording)
	{
		m_Recording = new Dictionary<int, RecordState>(recording);
		m_IsPlaying = true;
		if(m_Rigidbody)
		{
			m_Rigidbody.isKinematic = true;
		}		
	}
}
