using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

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
	Movement m_Movement;

	bool isPlaying = false;
	bool isRecording = true;

	void Start()
	{
		//m_Animator = GetComponent<Animator>();
		m_Rigidbody = GetComponent<Rigidbody>();
        m_Movement = GetComponent<Movement>();
        m_TimeController = FindObjectOfType<TimeController>();
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			isRecording = false;
			SetRecording(m_States);
			m_TimeController.time = 0;
			//m_Animator.SetBool("Transition", false);
		}

		if(Input.GetButtonDown("TimeRewind"))
		{
			SetRecording(m_States);
			if(m_Movement)
			{
				m_Movement.SetControl(false);
			}
			isRecording = false;
			isPlaying = true;
			//m_Animator.SetBool("Transition", false);
			//m_Animator.SetFloat("GlobalSpeed", m_Animator.GetFloat("GlobalSpeed") * -1f);
		}

		if(Input.GetButton("TimeRewind"))
		{
			m_TimeController.isFoward = false;	
		}

		if(Input.GetButtonUp("TimeRewind"))
		{
			m_TimeController.isFoward = true;
			isRecording = true;
			isPlaying = false;

            if (m_Movement)
            {
                m_Movement.SetControl(true);
            }
            //m_Animator.SetBool("Transition", true);
			if(m_Rigidbody)
			{
				m_Rigidbody.isKinematic = false;
			}
			//m_Animator.SetFloat("GlobalSpeed", m_Animator.GetFloat("GlobalSpeed") * -1f);
		}
	}

	void FixedUpdate()
	{
		if (isRecording)
		{
			m_States[m_TimeController.time] =  new RecordState(transform.position, transform.rotation);
            //m_Animator.GetCurrentAnimatorStateInfo(0).shortNameHash,

            //m_Animator.GetFloat("Speed"));
        }

		if (isPlaying)
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
		isPlaying = true;
		if(m_Rigidbody)
		{
			m_Rigidbody.isKinematic = true;
		}		
	}
}
