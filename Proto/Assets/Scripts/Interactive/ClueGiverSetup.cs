using UnityEngine;

public class ClueGiverSetup : MonoBehaviour
{
    [SerializeField]string[] _PartsName;

    public string[] GetPartsName()
    {
        return _PartsName;
    }
}
