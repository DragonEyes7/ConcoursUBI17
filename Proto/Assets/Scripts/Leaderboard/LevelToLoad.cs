using UnityEngine;

public class LevelToLoad : MonoBehaviour
{
    private string _levelToLoad = "Tuto";
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (!PhotonNetwork.inRoom) return;

        if (PhotonNetwork.room.CustomProperties.ContainsKey("levelName"))
        {
            _levelToLoad = (string)PhotonNetwork.room.CustomProperties["levelName"];
        }
    }

    public string GetLevelToLoad()
    {
        return _levelToLoad;
    }
}
