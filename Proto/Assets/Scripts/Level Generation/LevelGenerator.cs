using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    //un peu batard mais on a pas vraiment besoin de se casser la tête
    public Transform[] small_spawnLocations;
    public Transform[] medium_spawnLocations;
    public Transform[] large_spawnLocations;

    //un peu batard aussi
    public GameObject[] small_shops;
    public GameObject[] medium_shops;
    public GameObject[] large_shops;

    // Use this for initialization
    void Start () {
        instanciateShops(small_spawnLocations, small_shops);
        instanciateShops(medium_spawnLocations, medium_shops);
        instanciateShops(large_spawnLocations, large_shops);
    }

    void instanciateShops( Transform[] ts, GameObject[] s)
    {
        foreach (Transform t in ts)
        {
            //todo : adder un truc pour empecher de reprendre les memes shops
            GameObject shop = Instantiate(s[Random.Range(0, s.Length)], t.transform, false);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
