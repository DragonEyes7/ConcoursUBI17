using System;
using UnityEngine;

[Serializable]
public class InteractiveObject
{
    [SerializeField]GameObject _Prefab;
	[SerializeField]Transform _SpawnPosition;
    [SerializeField]Transform[] _Path;

    public GameObject Prefab()
    {
        return _Prefab;
    }

    public Transform SpawnPosition()
    {
        return _SpawnPosition;
    }

    public Transform[] Path()
    {
        return _Path;
    }
}
