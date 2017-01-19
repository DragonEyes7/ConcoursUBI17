using UnityEngine;
using UnityEngine.UI;

public class InsideObject : MonoBehaviour
{
	[SerializeField]private Image m_InsideObject;

	void Start()
	{
		m_InsideObject.enabled = false;
	}

	void OnTriggerEnter(Collider other)
	{
		m_InsideObject.enabled = true;
	}

	void OnTriggerExit(Collider other)
	{
		m_InsideObject.enabled = false;
	}
}