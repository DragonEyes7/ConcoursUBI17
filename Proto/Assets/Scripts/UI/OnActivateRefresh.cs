using UnityEngine;

public class OnActivateRefresh : MonoBehaviour
{
    private ServerUI _serverUi;
    private void Start()
    {
        _serverUi = FindObjectOfType<ServerUI>();
    }

    private void OnEnable()
    {
        _serverUi.GetServerList();
    }
}