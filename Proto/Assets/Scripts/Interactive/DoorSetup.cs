using UnityEngine;

public class DoorSetup : MonoBehaviour
{
    [SerializeField] private bool _isOpen;
    [SerializeField] private bool _isLock = true;

    public bool isOpen()
    {
        return _isOpen;
    }

    public bool isLock()
    {
        return _isLock;
    }
}