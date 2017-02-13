using Assets;
using System.Collections;
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

	// Use this for initialization
	void Start ()
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
        Material[] HairList = Resources.LoadAll<Material>("Materials/Hair");
        Material[] ClothList = Resources.LoadAll<Material>("Materials/Cloth");

        //Get all possible combinations
        foreach (Material h in HairList)
        {
            foreach (Material p in ClothList)
            {
                foreach (Material s in ClothList)
                {

                }
            }
        }

        //Generate NPCs
        for (int i = 0; i < NPCCount + 1; i++)
        {

            int StartPosIndex = (int)(Random.Range(0, InterestPoints.Count / 100.0f) * 100);
            Vector3 Pos = InterestPoints[StartPosIndex].transform.position;
            GameObject npc = new GameObject();
            if (i == 0)
            {
                npc = Instantiate(Target_Prefab, Pos, new Quaternion());
            }
            else
            {
                npc = Instantiate(NPC_Prefab, Pos, new Quaternion());
            }

            npc.GetComponent<NPCWalkScript>().NPCID = i;
            npc.GetComponent<Recorder>().SetTimeController(_timeController);
            NPCs.Add(npc);
        }
        
        //Set the schedule for each NPC
        for (int i = 1; i < NPCs.Count; i++)
        {

            int StartPosIndex = (int)(Random.Range(0, InterestPoints.Count / 100.0f) * 100);
            Transform Pos = InterestPoints[StartPosIndex].transform;
            NPCs[i].GetComponent<NPCWalkScript>().Destination = Pos;
            NPCs[i].GetComponent<NPCWalkScript>().Location = Pos;
            NPCs[i].GetComponent<NPCWalkScript>().OnDestinationChange();

            ScheduleNPC NPCSchedule = new ScheduleNPC();
            //NPCSchedule.AddItem(0, NPCs[i].transform);
            for (int j = ScheduleGap; j < MapLength; j += schedulerTickRate)
            {
                if ((int)(Random.Range(0.0f, 1.0f) * 100) >= 90)
                {
                    int PosIndex = (int)(Random.Range(0, InterestPoints.Count / 100.0f) * 100);
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
        Target.AddItem(0, StartingLocation);
        for (int j = 1; j < MapLength; j += schedulerTickRate)
        {
            if ((int)(Random.Range(0.0f, 1.0f) * 100) >= 90)
            {
                int PosIndex = (int)(Random.Range(0, InterestPoints.Count / 100.0f) * 100);
                Transform SchedulePos = InterestPoints[PosIndex].transform;
                Target.AddItem(j, SchedulePos);

                //Lock the NPC in that location for the next 60 seconds
                j += ScheduleGap;
            }
        }
        Target.OrderSchedule();
        return Target;
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
