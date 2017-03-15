using UnityEngine;

public class OnActivateRefresh : MonoBehaviour
{
    private ServerUI _serverUi;
    private void Start()
    {
        _serverUi = FindObjectOfType<ServerUI>();
        _serverUi.GetServerList();
    }

    private void OnEnable()
    {
        if(_serverUi)_serverUi.GetServerList();
    }
}