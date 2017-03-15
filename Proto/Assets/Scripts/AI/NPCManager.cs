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
        GameObject[] HatList = Resources.LoadAll<GameObject>("Hat");
        Material[] ClothList = Resources.LoadAll<Material>("Materials/Cloth");

        List<List<Material>> Possibilities = new List<List<Material>>();

        //Get all possible combinations
        foreach (Material p in ClothList)
        {
            foreach (Material s in ClothList)
            {
                List<Material> possibility = new List<Material>();
                possibility.Add(p);
                possibility.Add(s);
                Possibilities.Add(possibility);
            }
        }

        for (int i = Possibilities.Count - 1; i > 0; --i)
        {
            int r = Random.Range(0, i);
            List<Material> tmp = Possibilities[i];
            Possibilities[i] = Possibilities[r];
            Possibilities[r] = tmp;
        }

        List<List<Material>> NPCMats = Possibilities.GetRange(0, NPCCount);

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
            npc.GetComponent<NPCCharacteristics>().Hat = HatList[Random.Range(0, HatList.Length)];
            npc.GetComponent<NPCCharacteristics>().PantMaterial = NPCMats[i][0];
            npc.GetComponent<NPCCharacteristics>().ShirtMaterial = NPCMats[i][1];
            NPCs.Add(npc);
        }
        
        //Set the schedule for each NPC
        for (int i = 1; i < NPCs.Count; i++)
        {
            int StartPosIndex = Random.Range(0, InterestPoints.Count);
            Transform Pos = InterestPoints[StartPosIndex].transform;
            NPCs[i].GetComponent<NPCWalkScript>().Destination = Pos;
            NPCs[i].GetComponent<NPCWalkScript>().Location = Pos;
            NPCs[i].GetComponent<NPCWalkScript>().OnDestinationChange();

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
            NPCs[i].GetComponent<NPCWalkScript>().setSchedule(NPCSchedule);
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
        //TODO: Modify this section if the NPCs characteristics change
        GameObject target = NPCs[0];
        characteristics.Add("Hat", target.GetComponent<NPCCharacteristics>().Hat.name);
        characteristics.Add("Shirt", target.GetComponent<NPCCharacteristics>().ShirtMaterial.name);
        characteristics.Add("Pants", target.GetComponent<NPCCharacteristics>().PantMaterial.name);

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
