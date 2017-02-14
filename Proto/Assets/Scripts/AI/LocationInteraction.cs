using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationInteraction : MonoBehaviour {

    public string Tag;
    private List<Transform> ChildLocations;
    private Dictionary<int, Transform> OccupiedLocations;
	// Use this for initialization
	void Start () {
        ChildLocations = new List<Transform>();
        OccupiedLocations = new Dictionary<int, Transform>();

        foreach (Transform child in gameObject.transform)
        {
            if (child.CompareTag(Tag))
            {
                ChildLocations.Add(child);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public Transform GetLocation(int NPCID)
    {

        Transform Location;
        if (ChildLocations.Count > 0)
        {
            Location = ChildLocations[(int)(Random.Range(0, ChildLocations.Count / 100.0f) * 100)];
        } 
        else
        {
            Location = transform;
        }
        return Location;
    }

    public void Occupy(int NPCID, Transform Location)
    {
        if (Location != transform)
        {
            OccupiedLocations.Add(NPCID, Location);
            ChildLocations.Remove(Location);
        }
    }

    public void Free(int NPCID)
    {
        if (OccupiedLocations.ContainsKey(NPCID))
        {
            ChildLocations.Add(OccupiedLocations[NPCID]);
            OccupiedLocations.Remove(NPCID);
        }
    }
}
