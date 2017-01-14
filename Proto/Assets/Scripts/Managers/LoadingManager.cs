using UnityEngine;

public class LoadingManager : MonoBehaviour
{
    public void ConnectionCompleted()
    {
        gameObject.SetActive(false);
    }
}