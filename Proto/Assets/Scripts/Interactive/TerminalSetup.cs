using UnityEngine;

public class TerminalSetup : MonoBehaviour
{
    [SerializeField]Door[] m_Doors;

    public Door[] GetDoors()
    {
        return m_Doors;
    }
}
