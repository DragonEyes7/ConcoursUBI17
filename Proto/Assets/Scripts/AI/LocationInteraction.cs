using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationInteraction : MonoBehaviour {

    public string Tag;
    private List<Transform> ChildLocations;
	// Use this for initialization
	void Start () {
        ChildLocations = new List<Transform>();

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

    public Transform GetLocation()
    {
        return ChildLocations[(int)(Random.Range(0, ChildLocations.Count / 100.0f) * 100)];
    }
}
