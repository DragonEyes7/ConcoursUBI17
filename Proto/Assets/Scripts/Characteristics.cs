using UnityEngine;

public class Characteristics : MonoBehaviour
{
    int[] m_Characteristics = new int[6];

	void Start ()
    {
        for(int i = 0; i < m_Characteristics.Length; ++i)
        {
            m_Characteristics[i] = Random.Range(0, 3);
        }
	}
	
	void Update ()
    {
		
	}

    public int[] GetCharacteristics()
    {
        return m_Characteristics;
    }
}