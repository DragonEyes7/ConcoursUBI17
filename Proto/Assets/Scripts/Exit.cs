using UnityEngine;

public class Exit : Interactive
{

	void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}

    void OnTriggerEnter(Collider other)
    {
        Action act = other.GetComponent<Action>();
        if (act)
        {
            Debug.Log("Player is leaving...");
            act.SetInteract(true);
            act.SetInteractionObject(this);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Action act = other.GetComponent<Action>();
        if (act)
        {
            Debug.Log("EliminateTarget not Possible");
            act.SetInteract(false);            
        }
    }

    public override void Interact()
    {
        Debug.Log("Level left");
        /*if(GameManager.ObjectifCompleted())
        {
            //Mission Completed
        }
        else
        {
            //Mission failed
        }*/
    }
}