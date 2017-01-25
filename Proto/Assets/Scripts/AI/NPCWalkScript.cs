using Assets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCWalkScript : MonoBehaviour {

    public Transform Destination;
    public Transform Location;
    public Schedule NPCSchedule;
    public int NPCID;

    private NavMeshAgent agent;

    void Start()
    {

    }

    public void OnDestinationChange()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();

        agent.SetDestination(Destination.position);
    }

    public void OnTimeChange(int Time)
    {
        Transform NextPosition = NPCSchedule.NextDestination(Time);
        if (NextPosition != Location)
        {
            Destination = NextPosition;
            Location = Destination;
            OnDestinationChange();
        }
    }

    void OnTriggerEnter(Collider location)
    {
        int i = NPCID;
        //Verify the NPC as reqached the correct location
        if (location.transform == Destination)
        {
            Destination = location.GetComponentInParent<LocationInteraction>().GetLocation();
            OnDestinationChange();
        }
    }
}

