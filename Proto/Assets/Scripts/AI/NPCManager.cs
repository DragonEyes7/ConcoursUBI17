using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour {

    public List<GameObject> NPCs, InterestPoints;
    public int NPCCount;
    public float MapLength;
    public GameObject NPC_Prefab;

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
        
        //Generate NPCs
        for (int i =0; i < NPCCount; i++)
        {
            
            //int posIndex = (int)(Random.Range(0, InterestPoints.Count / 100.0f) * 100);
            GameObject npc = Instantiate(NPC_Prefab);

            npc.GetComponent<NPCWalkScript>().NPCID = i;
            npc.GetComponent<Recorder>().SetTimeController(_timeController);
            NPCs.Add(npc);
        }
        
        //Set the schedule for each NPC
        for (int i = 0; i < NPCs.Count; i++)
        {

            int StartPosIndex = (int)(Random.Range(0, InterestPoints.Count / 100.0f) * 100);
            Transform Pos = InterestPoints[StartPosIndex].transform;
            NPCs[i].GetComponent<NPCWalkScript>().Destination = Pos;
            NPCs[i].GetComponent<NPCWalkScript>().Location = Pos;
            NPCs[i].GetComponent<NPCWalkScript>().OnDestinationChange();

            Schedule NPCSchedule = new Schedule();
            for (int j = 1; j < MapLength; j += schedulerTickRate)
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
            NPCs[i].GetComponent<NPCWalkScript>().NPCSchedule = NPCSchedule;
        }
        
    }
	
	// Update is called once per frame
	/*void Update () {
        scheduleTimer -= Time.deltaTime;
        if (scheduleTimer <= 0 )
        {

            timeSpent += schedulerTickRate;
            scheduleTimer = schedulerTickRate;
            //Send new time to the NPCs

        }
    }*/

    private int DoOnTick(int time)
    {
        foreach (var npc in NPCs)
        {
            npc.GetComponent<NPCWalkScript>().OnTimeChange(time);
        }
        return 0;
    }

    void TimeTravel(float seconds)
    {
        float newTimeValue = scheduleTimer + seconds;
        scheduleTimer = newTimeValue % schedulerTickRate;
        if (scheduleTimer + seconds > schedulerTickRate)
        {
            //Change NPC Schedule - By X
            //Where X = newTimeValue div 60
        }
    }
}
