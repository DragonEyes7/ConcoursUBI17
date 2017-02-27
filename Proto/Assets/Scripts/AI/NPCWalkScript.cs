using Assets;
using UnityEngine;
using UnityEngine.AI;

public class NPCWalkScript : MonoBehaviour {

    public Transform Destination;
    public Transform Location;
    private ScheduleNPC NPCSchedule;
    public int NPCID = 0;

    protected NavMeshAgent agent;
    
    public virtual void setSchedule(ScheduleNPC Schedule)
    {
        NPCSchedule = Schedule;
    }

    protected void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = true;
    }
    
    public void OnDestinationChange()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(Destination.position);
    }

    public virtual void OnTimeChange(int Time)
    {
        Transform NextPosition = NPCSchedule.NextDestination(Time, transform);
        if (NextPosition != Location)
        {
            Destination = NextPosition;
            Location = Destination;
            //NPCSchedule.RemoveLocation(NextPosition);
            OnDestinationChange();
        }
    }

    void OnTriggerEnter(Collider location)
    {
        //Verify the NPC as reached the correct location
        if (location.transform == Destination)
        {
            Destination = location.GetComponentInParent<LocationInteraction>().GetLocation(NPCID);
            location.GetComponentInParent<LocationInteraction>().Occupy(NPCID, Destination);
            OnDestinationChange();
        }
    }

    void OnTriggerExit(Collider location)
    {
        LocationInteraction LI = location.GetComponentInParent<LocationInteraction>();

        if (LI)
        {
            LI.Free(NPCID);
        }
    }
}

