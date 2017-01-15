using UnityEngine;

public class EliminateTarget : Interactive
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
        if(act)
        {
            Debug.Log("EliminateTarget Possible");
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
        PhotonNetwork.Destroy(gameObject);
    }
}