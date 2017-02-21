using UnityEngine;
using System.Collections.Generic;

public class TargetLock: MonoBehaviour
{
	Transform m_Target;
	Transform m_PlayerTransform;
	List<Transform> m_TargetList;

	Material[] m_TargetMaterials;
	Material[][] m_TargetDefaultMaterial;
	Material m_SoftLock;
	Material m_HardLock;

	float m_MaxDistance = 20f;
	bool m_TargetLock = false;
	bool m_SwitchTargetActivated = false;

	void Start ()
	{
		m_TargetList = new List<Transform>();
		m_SoftLock = Resources.Load<Material>("MAT_OutlineOrange");
		m_HardLock = Resources.Load<Material>("MAT_OutlineRed");
		m_TargetMaterials = new Material[2];
		m_TargetDefaultMaterial = new Material[10][];
	}
	
	void Update ()
	{
		if (m_Target && Input.GetButtonDown("TargetLock")) //Input not recognize
		{
			if (!m_TargetLock)
			{
				HardLock();
				m_TargetLock = true;
			}
			else
			{
				Unlock();
				m_TargetLock = false;
			}

			//transform.LookAt(m_Target);
			/*if (InputMode.LockMode == InputMode.LockState.HARDLOCK)
			{
				m_FollowTarget = !m_FollowTarget;
			}*/
		}

		if (m_TargetList.Count > 0 && m_TargetLock && !m_SwitchTargetActivated)
		{
			m_SwitchTargetActivated = true;
			if (Input.GetAxis("SwitchTarget") < 0)
			{
				//PreviousTarget
				SwitchTarget(-1);
			}

			if (Input.GetAxis("SwitchTarget") > 0)
			{
				//NextTarget
				SwitchTarget(+1);
			}

			if (m_Target)
			{
				if (Vector3.Distance(m_Target.position, m_PlayerTransform.position) > m_MaxDistance)
				{
					SwitchTarget(+1);
				}
			}
		}

		if (Input.GetAxis("SwitchTarget") == 0)
		{
			m_SwitchTargetActivated = false;
		}

		/*if (m_FollowTarget)
		{
			//transform.LookAt(m_Target);
			Vector3 target = new Vector3(m_Target.position.x, 0f, m_Target.position.z);

			transform.LookAt(target);
		}*/
	}

	void OnTriggerEnter(Collider other)
	{
		if (!m_PlayerTransform || other.transform == m_PlayerTransform)
			return;

		TargetManager TM = other.GetComponent<TargetManager>();

		if (TM)
		{
			if(!m_TargetList.Contains(other.transform))
			{
				m_TargetList.Add(other.transform);
			}

			if(!m_TargetLock)
			{
				if (!m_Target && Vector3.Distance(other.transform.position, m_PlayerTransform.position) < m_MaxDistance)
				{
					m_Target = other.transform;
					SoftLock();

					//Debug.Log("Target Aquired");
				}

				if (m_Target && Vector3.Distance(other.transform.position, m_PlayerTransform.transform.position) < Vector3.Distance(m_Target.transform.position, m_PlayerTransform.transform.position))
				{
					Unlock();
					m_Target = other.transform;
					SoftLock();
				}
			}
					
			//Debug.Log("Target Found:" + m_Target);
		}
	}

	void OnTriggerStay(Collider other)
	{
		if (!m_PlayerTransform || other.transform == m_PlayerTransform)
			return;

		if(!m_Target)
		{
			TargetManager TM = other.GetComponent<TargetManager>();

			if (TM)
			{
				if (Vector3.Distance(other.transform.position, m_PlayerTransform.position) < m_MaxDistance
					&& TM.IsActive())
				{
					m_Target = other.transform;
					SoftLock();
				}
			}
		}
		else
		{
			if(!m_Target.GetComponent<TargetManager>().IsActive())
			{
				m_TargetList.Remove(m_Target);
				Unlock();
			}
		}
		/*if (other.gameObject.GetInstanceID() != m_PlayerTransform.gameObject.GetInstanceID())
		{
			TargetManager TM = other.GetComponent<TargetManager>();

			if (TM && m_Target)
			{
				if (Vector3.Distance(other.transform.position, m_PlayerTransform.position) < Vector3.Distance(m_Target.transform.position, m_PlayerTransform.position))
				{
					Unlock();

					m_Target = other.transform;

					SoftLock();

					Debug.Log("Target Changed");
				}
			}
		}*/
	}

	void SortTargetsByDistance()
	{
		for(int i = 0; i < m_TargetList.Count; ++i)
		{
			if(Vector3.Distance(m_TargetList[i].position, m_PlayerTransform.position) > m_MaxDistance)
			{
				m_TargetList.RemoveAt(i);
			}
		}

		if(m_TargetList.Count <= 0)
		{
			Unlock();
			m_TargetLock = false;
		}

		m_TargetList.Sort
		(
			delegate (Transform t1, Transform t2)
			{
				return (Vector3.Distance(t1.position, m_PlayerTransform.position).CompareTo(Vector3.Distance(t2.position, m_PlayerTransform.position)));
			}
		);
	}

	void SwitchTarget(int value)
	{
		SortTargetsByDistance();

		if(m_TargetList.Count > 0)
		{
			int targetIndex = m_TargetList.IndexOf(m_Target);

			targetIndex += value;
			if (targetIndex >= m_TargetList.Count)
			{
				targetIndex = 0;
			}
			else if (targetIndex < 0)
			{
				targetIndex = m_TargetList.Count - 1;
			}

			Unlock();
			m_Target = m_TargetList[targetIndex];
			HardLock(true);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (!m_PlayerTransform || other.transform == m_PlayerTransform)
			return;

		if (other.transform == m_Target && !m_TargetLock)
		{
			TargetManager TM = other.GetComponent<TargetManager>();

			if (TM)
			{
				Unlock();
			}
			
			//m_Target = null;
			//Debug.Log("Target Lost:" + m_Target);
		}
	}

	void SoftLock()
	{
		Renderer[] rends = FindRenderers();

		if(rends.Length > m_TargetDefaultMaterial.Length)
		{
			m_TargetDefaultMaterial = new Material[rends.Length][];
		}

		if (rends.Length > 0)
		{
			for(int i = 0; i < rends.Length; ++i)
			{
				m_TargetDefaultMaterial[i] = rends[i].materials;
				m_TargetMaterials[0] = m_TargetDefaultMaterial[i][0];
                m_TargetMaterials[1] = m_SoftLock;
                //m_TargetMaterials[1] = (m_Target.GetComponent<TutoPvPFaction>().m_TeamID == m_PlayerTransform.GetComponent<TutoPvPFaction>().m_TeamID) ? m_SoftLockFriendly : m_SoftLock;
                rends[i].materials = m_TargetMaterials;
			}
		}
	}

	void HardLock(bool switchTarget = false)
	{
		Renderer[] rends = FindRenderers();

		if (rends.Length > m_TargetDefaultMaterial.Length)
		{
			m_TargetDefaultMaterial = new Material[rends.Length][];
		}

		if (rends.Length > 0)
		{
			for (int i = 0; i < rends.Length; ++i)
			{
				if(switchTarget)
				{
					m_TargetDefaultMaterial[i] = rends[i].materials;
				}
				m_TargetMaterials[0] = m_TargetDefaultMaterial[i][0];
                m_TargetMaterials[1] = m_HardLock;
                //m_TargetMaterials[1] = (m_Target.GetComponent<TutoPvPFaction>().m_TeamID == m_PlayerTransform.GetComponent<TutoPvPFaction>().m_TeamID) ? m_HardLockFriendly : m_HardLock;
                rends[i].materials = m_TargetMaterials;
			}
		}
	}

	void Unlock()
	{
		Renderer[] rends = FindRenderers();

		if (rends.Length > m_TargetDefaultMaterial.Length)
		{
			m_TargetDefaultMaterial = new Material[rends.Length][];
		}

		if (rends.Length > 0)
		{
			for (int i = 0; i < rends.Length; ++i)
			{
                rends[i].materials = m_TargetDefaultMaterial[i];
            }
		}

		m_Target = null;
	}

	Renderer[] FindRenderers()
	{
		Renderer[] rends = m_Target.GetComponents<Renderer>();
		List<Renderer> rendsReturn = new List<Renderer>();

		if (rends.Length <= 0)
		{
			rends = m_Target.GetComponentsInChildren<Renderer>();

			foreach (Renderer rend in rends)
			{
				if (rend.tag == "NoOutline")
				{
					continue;
				}
				rendsReturn.Add(rend);
			}
		}
		else
		{
			foreach (Renderer rend in rends)
			{
				if (rend.tag == "NoOutline")
				{
					continue;
				}
				rendsReturn.Add(rend);
			}
		}

		return rendsReturn.ToArray();
	}

	public void SetPlayerTransform(Transform player)
	{
		m_PlayerTransform = player;
	}

	public Transform GetTarget()
	{
		return m_Target;
	}

	public Transform GetTargetHardLock()
	{
		if(m_TargetLock)
		{
			return m_Target;
		}

		return m_PlayerTransform;
	}
}
