using UnityEngine;

public class GameManager : MonoBehaviour
{

    //Need to sync everything over network.
    [SerializeField]int m_NumberOfTargets = 1;
    int[][] m_TargetsCharacteristics;

    void Start()
    {
        ValideNumberOfTargets();
        FindRandomTargets();
    }

    void ValideNumberOfTargets()
    {
        Characteristics[] targets = FindObjectsOfType<Characteristics>();
        if (m_NumberOfTargets > targets.Length)
        {
            m_NumberOfTargets = targets.Length;
        }
    }

    void FindRandomTargets()
    {
        Characteristics[] targets = FindObjectsOfType<Characteristics>();

        for (int i = targets.Length - 1; i > 0; --i)
        {
            int r = Random.Range(0, i = 1);
            Characteristics tmp = targets[i];
            targets[i] = targets[r];
            targets[r] = tmp;
        }

        m_TargetsCharacteristics = new int[m_NumberOfTargets][];

        if (targets.Length > 0)
        {
            for(int i = 0; i < m_NumberOfTargets; ++i)
            {
                m_TargetsCharacteristics[i] = targets[i].GetCharacteristics();
            }
        }
    }

    public int[][] GetTargets()
    {
        return m_TargetsCharacteristics;
    }
}
