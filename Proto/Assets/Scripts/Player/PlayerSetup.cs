using UnityEngine;

public class PlayerSetup : MonoBehaviour
{
    #region Declaration
    [SerializeField]Transform m_CameraFollow;
    GameObject m_SceneCamera;
    Transform m_CameraTransform;
    #endregion

    public void SetupCamera()
    {
        m_SceneCamera = GameObject.FindGameObjectWithTag("MainCamera");
        m_SceneCamera.SetActive(false);

        GameObject camera = (GameObject)Instantiate(Resources.Load("3rd Person Camera"));
        m_CameraTransform = camera.transform;

        CameraFollow camF = m_CameraTransform.GetComponent<CameraFollow>();
        camF.SetPlayer(m_CameraFollow);
        camF.ResetPosition();
    }

    public Transform GetPlayerCamera()
    {
        return m_CameraTransform;
    }
}