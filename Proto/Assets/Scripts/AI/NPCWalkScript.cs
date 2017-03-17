using Assets;
using UnityEngine;
using UnityEngine.AI;

public class NPCWalkScript : MonoBehaviour {

    public Transform Destination;
    public Transform Location;
    private ScheduleNPC NPCSchedule;
    public int NPCID = 0;

    Animator _Animator;
    bool _IdleBreak = false;

    protected NavMeshAgent agent;
    
    public virtual void setSchedule(ScheduleNPC Schedule)
    {
        NPCSchedule = Schedule;
    }

    void OnEnable()
    {
        _Animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = true;
    }

    void Update()
    {
        if (!agent) return;
        _Animator.SetFloat("Speed", agent.velocity.magnitude);
        if(agent.velocity.magnitude > 0)
        {
            IdleBreakReset();
        }
        else if(!_IdleBreak)
        {
            _IdleBreak = true;
            Invoke("IdleBreak", Random.Range(2, 5));
        }
    }

    public void OnDestinationChange()
    {
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

    void IdleBreak()
    {
        _Animator.SetTrigger("IdleBreak");
        Invoke("IdleBreakReset", Random.Range(10, 15));
    }

    void IdleBreakReset()
    {
        CancelInvoke("IdleBreak");
        _IdleBreak = false;
    }
}

