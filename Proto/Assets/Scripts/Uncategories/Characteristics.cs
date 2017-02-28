using UnityEngine;

public class Characteristics : MonoBehaviour
{
    [SerializeField]int[] m_Characteristics = new int[3];

    void Start ()
    {
        transform.rotation = new Quaternion(0,Random.Range(0f,1f),0f,1f);
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
                GM.ValidateTarget(gameObject.GetComponent<NPCWalkScript>().NPCID);
            }            
        }        
    }
}