using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    //un peu batard mais on a pas vraiment besoin de se casser la tête
    public Transform[] small_spawnLocations;
    public Transform[] medium_spawnLocations;
    public Transform[] large_spawnLocations;

    //un peu batard aussi
    public List<GameObject> small_shops;
    public List<GameObject> medium_shops;
    public List<GameObject> large_shops;

    public int nbr_of_small_shops;
    public int nbr_of_med_shops;
    public int nbr_of_large_shops;

    // Use this for initialization
    void Start () {
       /* instanciateShops(small_spawnLocations, small_shops);
        instanciateShops(medium_spawnLocations, medium_shops);
        instanciateShops(large_spawnLocations, large_shops);*/

        instanciateShops(small_spawnLocations, small_shops,nbr_of_small_shops);
        instanciateShops(medium_spawnLocations, medium_shops,nbr_of_med_shops);
        instanciateShops(large_spawnLocations, large_shops, nbr_of_large_shops);
    }

    void instanciateShops( Transform[] ts, List<GameObject> s)
    {
        int random;

        foreach (Transform t in ts)
        {
            random = Random.Range(0, s.Count);
            GameObject shop = Instantiate(s[random], t.transform, false);
            s.RemoveAt(random);
        }
    }
// Could help with balance 
    void instanciateShops(Transform[] ts, List<GameObject> s, int NbrOfShop)
    {
        int random;

        for(int i =0; i<NbrOfShop;i++)
        {
            random = Random.Range(0, s.Count);
            GameObject shop = Instantiate(s[random], ts[i].transform, false);
            s.RemoveAt(random);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
