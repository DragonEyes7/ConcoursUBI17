using Assets;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour {

    public List<GameObject> NPCs, InterestPoints;
    public GameObject Target;
    public int NPCCount;
    public float MapLength;
    public GameObject NPC_Prefab;
    public GameObject Target_Prefab;

    public int timeSpent;
    public int schedulerTickRate;
    public float scheduleTimer;
    public int ScheduleGap;

    private TimeController _timeController;
    
	public void Setup()
	{
	    _timeController = FindObjectOfType<TimeController>();
	    _timeController.Tick.Suscribe(DoOnTick);

        //Iniate the variables and lists
        NPCs = new List<GameObject>();
        InterestPoints = new List<GameObject>();
        scheduleTimer = schedulerTickRate;

        //Get the list of all Interest Points in the scene
        InterestPoints.AddRange(GameObject.FindGameObjectsWithTag("InterestPoint"));

        //Get the Materials list
        GameObject[] HatList = Resources.LoadAll<GameObject>("Hats");
        GameObject[] AccessoryList = Resources.LoadAll<GameObject>("Accessories");
        GameObject[] FacialList = Resources.LoadAll<GameObject>("Facials");

        Material[] HatMatList = Resources.LoadAll<Material>("Materials/Hat");
        Material[] AccessoryMatList = Resources.LoadAll<Material>("Materials/Accessory");
        Material[] FacialMatList = Resources.LoadAll<Material>("Materials/Facial");

        List<List<GameObject>> Possibilities = new List<List<GameObject>>();

        //Get all possible combinations
        foreach (GameObject hat in HatList)
        {
            foreach (GameObject acc in AccessoryList)
            {
                foreach (GameObject fac in FacialList)
                {
                    List<GameObject> possibility = new List<GameObject>();
                    possibility.Add(hat);
                    possibility.Add(acc);
                    possibility.Add(fac);
                    Possibilities.Add(possibility);
                }
            }
        }

        for (int i = Possibilities.Count - 1; i > 0; --i)
        {
            int r = Random.Range(0, i);
            List<GameObject> tmp = Possibilities[i];
            Possibilities[i] = Possibilities[r];
            Possibilities[r] = tmp;
        }

        List<List<GameObject>> NPCObjects = Possibilities.GetRange(0, NPCCount);

        //Generate NPCs
        for (int i = 0; i < NPCCount; i++)
        {
            int StartPosIndex = Random.Range(0, InterestPoints.Count);
            Vector3 pos = (i < InterestPoints[StartPosIndex].transform.childCount) 
                ? InterestPoints[StartPosIndex].transform.GetChild(i).transform.position 
                : InterestPoints[StartPosIndex].transform.GetChild(
                  Random.Range(0, InterestPoints[StartPosIndex].transform.childCount)).transform.position;

            GameObject npc = new GameObject();
            if (i == 0)
            {
                npc = PhotonNetwork.Instantiate(Target_Prefab.name, pos, new Quaternion(), 0);
            }
            else
            {
                npc = PhotonNetwork.Instantiate(NPC_Prefab.name, pos, new Quaternion(), 0);             
            }

            npc.GetComponent<NPCWalkScript>().NPCID = i;

            NPCCharacteristics NPCC = npc.GetComponent<NPCCharacteristics>();

            NPCC.Hat = NPCObjects[i][0]; ;
            NPCC.Accessory = NPCObjects[i][1];
            NPCC.Facial = NPCObjects[i][2];
            NPCC.HatMaterial = HatMatList[Random.Range(0, HatMatList.Length)];
            NPCC.AccessoryMaterial = AccessoryMatList[Random.Range(0, AccessoryMatList.Length)];
            NPCC.FacialMaterial = HatMatList[Random.Range(0, FacialMatList.Length)];
            NPCs.Add(npc);
        }
        
        //Set the schedule for each NPC
        for (int i = 1; i < NPCs.Count; i++)
        {
            int StartPosIndex = Random.Range(0, InterestPoints.Count);
            Transform Pos = InterestPoints[StartPosIndex].transform;
            NPCWalkScript NPCWS = NPCs[i].GetComponent<NPCWalkScript>();
            NPCWS.Destination = Pos;
            NPCWS.Location = Pos;
            NPCWS.OnDestinationChange();

            ScheduleNPC NPCSchedule = new ScheduleNPC();
            //NPCSchedule.AddItem(0, NPCs[i].transform);
            for (int j = ScheduleGap; j < MapLength; j += schedulerTickRate)
            {
                if (Random.Range(0, 100) >= 90)
                {
                    int PosIndex = Random.Range(0, InterestPoints.Count);
                    Transform SchedulePos = InterestPoints[PosIndex].transform;
                    NPCSchedule.AddItem(j, SchedulePos);

                    //Lock the NPC in that location for the next 60 seconds
                    j += ScheduleGap;
                }
            }
            NPCSchedule.OrderSchedule();
            NPCWS.setSchedule(NPCSchedule);
        }

        //Set the Target Schedule
        NPCs[0].GetComponent<TargetWalkScript>().setSchedule(CreateTargetSchedule(NPCs[0].transform));
    }

    private ScheduleTarget CreateTargetSchedule(Transform StartingLocation)
    {
        ScheduleTarget Target = new ScheduleTarget();
        //Target.AddItem(0, StartingLocation);
        for (int j = 1; j < MapLength; j += schedulerTickRate)
        {
            if (Random.Range(0, 100) >= 90)
            {
                int PosIndex = Random.Range(0, InterestPoints.Count);
                Transform SchedulePos = InterestPoints[PosIndex].transform;
                Target.AddItem(j, SchedulePos);

                //Lock the NPC in that location for the next 60 seconds
                j += ScheduleGap;
            }
        }
        Target.OrderSchedule();
        return Target;
    }

    public Dictionary<string, string> GetTargetCharacteristics()
    {
        Dictionary<string, string> characteristics = new Dictionary<string, string>();

        NPCCharacteristics NPCC = NPCs[0].GetComponent<NPCCharacteristics>();
        characteristics.Add("Hat", NPCC.Hat.name);
        characteristics.Add("Accessory", NPCC.Accessory.name);
        characteristics.Add("Facial", NPCC.Facial.name);
        characteristics.Add("HatMat", NPCC.HatMaterial.name);
        characteristics.Add("AccessoryMat", NPCC.AccessoryMaterial.name);
        characteristics.Add("FacialMat", NPCC.FacialMaterial.name);

        return characteristics;
    }

    private int DoOnTick(int time)
    {
        foreach (var npc in NPCs)
        {
            npc.GetComponent<NPCWalkScript>().OnTimeChange(time);
        }
        return 0;
    }
}
