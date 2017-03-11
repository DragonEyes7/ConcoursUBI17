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

            DoorSetup DS = _Doors[i].GetComponent<DoorSetup>();
            if(DS)
            {
                door.GetComponent<Door>().SetDoor(DS.isOpen(), DS.isLock());
            }
            else
            {
                Debug.LogWarning("The door will be set as default no DoorSetup was found on " + _Doors[i].name + " from " + transform.name);
            }
            
            Destroy(_Doors[i].gameObject);
        }

        for (int i = 0; i < _Cameras.Length; ++i)
        {
            GameObject camera = PhotonNetwork.Instantiate("SecurityCam", _Cameras[i].position, _Cameras[i].rotation, 0);
            camera.name = transform.name + "_SecurityCamera_" + i;
            camera.transform.SetParent(_CamerasParent.GetChild(i).transform);

            CameraSetup CS = _Cameras[i].GetComponent<CameraSetup>();
            if (CS)
            {
                CS.SetupCamera(camera.GetComponent<CameraMovement>());
            }
            else
            {
                Debug.LogWarning("The camera will be set as default no CameraSetup was found on " + _Cameras[i].name + " from " + transform.name);
            }

            Destroy(_Cameras[i].gameObject);
        }

        for (int i = 0; i < _ClueGivers.Length; ++i)
        {
            GameObject clueGiver = PhotonNetwork.Instantiate("ClueGiver", _ClueGivers[i].position, _ClueGivers[i].rotation, 0);
            clueGiver.name = transform.name + "_ClueGiver_" + i;
            clueGiver.transform.SetParent(_ClueGiversParent);

            ClueGiverSetup CGS = _ClueGivers[i].GetComponent<ClueGiverSetup>();
            if (CGS)
            {
                clueGiver.GetComponent<ClueGiver>().SetPartsName(CGS.GetPartsName());
            }
            else
            {
                Debug.LogWarning("The clue giver will be set as default no ClueGiverSetup was found on " + _ClueGivers[i].name + " from " + transform.name);
            }

            Destroy(_ClueGivers[i].gameObject);
        }

        for (int i = 0; i < _Terminals.Length; ++i)
        {
            GameObject terminal = PhotonNetwork.Instantiate("Terminal", _Terminals[i].position, _Terminals[i].rotation, 0);
            terminal.name = transform.name + "_Terminal_" + i;
            terminal.transform.SetParent(_TerminalsParent);

            TerminalSetup TS = _Terminals[i].GetComponent<TerminalSetup>();
            if (TS)
            {
                terminal.GetComponent<Terminal>().SetDoors(TS.GetDoors());
            }
            else
            {
                Debug.LogWarning("The terminal will be set as default no TerminalSetup was found on " + _Terminals[i].name + " from " + transform.name);
            }

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
