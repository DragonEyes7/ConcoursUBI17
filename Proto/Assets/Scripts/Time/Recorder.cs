using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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
    private RecordState previousState;

    PhotonView m_PhotonView;

	//Animator m_Animator;
	Rigidbody m_Rigidbody;

    private int _time;

	bool m_IsPlaying = false;
	bool m_IsRecording = true;

    public bool IsRecording
    {
        get { return m_IsRecording; }
    }

    public void SetTimeController(TimeController timecontroller)
    {
        m_TimeController = timecontroller;
    }

	void Start()
	{
		m_Rigidbody = GetComponent<Rigidbody>();
        m_TimeController = m_TimeController == null ? FindObjectOfType<TimeController>() : m_TimeController;
	    m_TimeController.Tick.Suscribe(DoOnTick);
	    DoOnTick(0);
        m_PhotonView = GetComponent<PhotonView>();
	}

	void Update()
	{
		if (Input.GetButtonDown("TimeRewind"))
	    {
	        SetTimeRewinding();
	        SetTimeForward();
            FindObjectOfType<GameManager>().ClearAgentClues();
	    }
	}

    private void SetTimeForward()
    {
        m_TimeController.isFoward = true;
        m_IsRecording = true;
        m_IsPlaying = false;

        if (m_Rigidbody)
        {
            m_Rigidbody.isKinematic = false;
        }
    }

    private void SetTimeRewinding()
    {
        SetRecording(m_States);
        m_IsRecording = false;
        m_IsPlaying = true;
        m_TimeController.isFoward = false;
        Debug.Log("Object reading of time : " + _time);
        var timeToRewind = GetMaxTime(3);
        _time -= timeToRewind;
        m_PhotonView.RPC("DoRewind", PhotonTargets.All, _time);
    }

    [PunRPC]
    void DoRewind(int key)
    {
        m_TimeController.time = key;
        if (m_Recording.ContainsKey(key))
        {
            PlayState(m_Recording[key]);
        }
        else
        {
            PlayState(m_Recording.Last().Key < key ? previousState : FindClosestState(key));
        }
    }

    private RecordState FindClosestState(int key)
    {
        var keys = new List<int>(m_Recording.Keys);
        var index = keys.BinarySearch(key);
        //~ = Bitwise NOT
        index = ~index - 1;
        if (index < 0) index = 0;
        if(!m_Recording.ContainsKey(index))Debug.Log("Using previous state, Dictionnary did not contain proper key : " + index + " Dictionnary count : " + m_Recording.Count);
        return !m_Recording.ContainsKey(index) ? previousState : m_Recording[index];
    }

    private int DoOnTick(int time)
	{
		if (m_IsRecording)
		{
		    var curState = new RecordState(transform.position, transform.rotation);
		    if (!curState.Equals(previousState))
		    {
		        previousState = curState;
		        m_States[time] =  new RecordState(transform.position, transform.rotation);
		    }
        }
	    _time = time;
        return 0;
	}

	void PlayState(RecordState recordState)
	{
		transform.position = recordState.position;
		transform.rotation = recordState.rotation;
	}

    private void SetRecording(IDictionary<int, RecordState> recording)
	{
		m_Recording = new Dictionary<int, RecordState>(recording);
		m_IsPlaying = true;
		if(m_Rigidbody)
		{
			m_Rigidbody.isKinematic = true;
		}		
	}

    private int GetMaxTime(int newTime)
    {
        return _time - newTime >= 0 ? newTime : _time;
    }
}
