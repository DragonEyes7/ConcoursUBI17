using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

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

    public void Setup ()
    {
        InstantiateShops(small_spawnLocations, small_shops, nbr_of_small_shops);
        InstantiateShops(medium_spawnLocations, medium_shops, nbr_of_med_shops);
        InstantiateShops(large_spawnLocations, large_shops, nbr_of_large_shops);
    }

    void InstantiateShops(Transform[] spawnSpot, List<GameObject> shop, int NbrOfShop)
    {
        int random;

        for(int i =0; i<NbrOfShop;i++)
        {
            random = Random.Range(0, shop.Count);
            var currentShop = PhotonNetwork.Instantiate(shop[random].name, spawnSpot[i].transform.position, spawnSpot[i].transform.rotation, 0);
            currentShop.transform.SetParent(spawnSpot[i].transform);
            shop.RemoveAt(random);
        }
    }
}
