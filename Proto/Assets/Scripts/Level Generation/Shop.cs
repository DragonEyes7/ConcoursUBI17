using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] Transform[] _Doors;
    [SerializeField] Transform[] _Cameras;
    [SerializeField] Transform[] _ClueGivers;
    [SerializeField] Transform[] _Terminals;
    [SerializeField] InteractiveObject[] _InteractiveObjects;
    [SerializeField] Transform _DoorsParent;
    [SerializeField] Transform _CamerasParent;
    [SerializeField] Transform _ClueGiversParent;
    [SerializeField] Transform _TerminalsParent;

    public void Setup ()
    {
		for(int i = 0; i < _Doors.Length; ++i)
        {
            GameObject door = PhotonNetwork.Instantiate("Door", _Doors[i].position, _Doors[i].rotation, 0);
            door.name = transform.name + "_Door_" + i;
            door.transform.SetParent(_DoorsParent);
            Destroy(_Doors[i].gameObject);
        }

        for (int i = 0; i < _Cameras.Length; ++i)
        {
            GameObject camera = PhotonNetwork.Instantiate("SecurityCam", _Cameras[i].position, _Cameras[i].rotation, 0);
            camera.name = transform.name + "_SecurityCamera_" + i;
            camera.transform.SetParent(_CamerasParent.GetChild(i).transform);
            Destroy(_Cameras[i].gameObject);
        }

        for (int i = 0; i < _ClueGivers.Length; ++i)
        {
            GameObject clueGiver = PhotonNetwork.Instantiate("ClueGiver", _ClueGivers[i].position, _ClueGivers[i].rotation, 0);
            clueGiver.name = transform.name + "_ClueGiver_" + i;
            clueGiver.transform.SetParent(_ClueGiversParent);
            Destroy(_ClueGivers[i].gameObject);
        }

        for (int i = 0; i < _Terminals.Length; ++i)
        {
            GameObject terminal = PhotonNetwork.Instantiate("Terminal", _Terminals[i].position, _Terminals[i].rotation, 0);
            terminal.name = transform.name + "_Terminal_" + i;
            terminal.transform.SetParent(_TerminalsParent);
            Destroy(_Terminals[i].gameObject);
        }

        for (int i = 0; i < _InteractiveObjects.Length; ++i)
        {
            string prefabName = _InteractiveObjects[i].Prefab().name;
            GameObject interactiveObject = PhotonNetwork.Instantiate(prefabName, _InteractiveObjects[i].SpawnPosition().position, _InteractiveObjects[i].SpawnPosition().rotation, 0);
            interactiveObject.name = transform.name + "_" + prefabName + "_" + i;
            interactiveObject.transform.SetParent(_InteractiveObjects[i].SpawnPosition());
            interactiveObject.GetComponent<MovableObject>().SetPathToFollow(_InteractiveObjects[i].Path());
        }
    }
}
