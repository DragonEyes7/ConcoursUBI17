using UnityEngine;

public class Characteristics : MonoBehaviour
{
    [SerializeField]int[] m_Characteristics = new int[3];
    [SerializeField]Transform[] m_Parts;
    GameManager m_GameManager;

    void Start ()
    {
        m_GameManager = FindObjectOfType<GameManager>();
        transform.rotation = new Quaternion(0,Random.Range(0f,1f),0f,1f);

        for(int i = 0; i < m_Parts.Length; ++i)
        {
            m_Parts[i].GetComponent<Renderer>().material = m_GameManager.GetPartMaterial(i, m_Characteristics[i]);
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

    void OnDestroy()
    {
        GameManager GM = FindObjectOfType<GameManager>();
        if(GM)
        {
            if(GM.isMaster)
            {
                GM.ValidateTarget(m_Characteristics);
            }            
        }        
    }
}