using UnityEngine;

public class Terminal : Interactive
{
    [SerializeField]Door[] m_Doors;

    public override void Interact()
    {
        foreach (Door door in m_Doors)
        {
            door.Interact();
        }

        m_IsActivated = true;
    }
}