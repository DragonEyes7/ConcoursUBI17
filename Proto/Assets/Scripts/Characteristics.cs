using UnityEngine;

public class Characteristics : MonoBehaviour
{
    PhotonView m_PhotonView;
    int[] m_Characteristics = new int[6];

	void Start ()
    {
        m_PhotonView = GetComponent<PhotonView>();
        for (int i = 0; i < m_Characteristics.Length; ++i)
        {
            m_Characteristics[i] = Random.Range(0, 3);
        }
	}

    public int[] GetCharacteristics()
    {
        return m_Characteristics;
    }

    public void TargetIdentified()
    {
        GetComponentInChildren<Renderer>().material.color = Color.red;
    }
}