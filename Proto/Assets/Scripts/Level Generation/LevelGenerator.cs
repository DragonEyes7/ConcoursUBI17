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

    List<int> _Shops = new List<int>();

    public void Setup ()
    {
        if (PhotonNetwork.isMasterClient)
        {
            InstantiateShops(small_spawnLocations, small_shops, nbr_of_small_shops);
            InstantiateShops(medium_spawnLocations, medium_shops, nbr_of_med_shops);
            InstantiateShops(large_spawnLocations, large_shops, nbr_of_large_shops);

            string ext = "(";

            foreach (int i in _Shops)
            {
                ext += i + ":";
            }

            ext += ")";
            Debug.Log(ext);

            FindObjectOfType<HUD>().ShowMessages(ext, 10);
        }
        else
        {
            AskForShops();
        }
    }

    void InstantiateShops(Transform[] spawnSpot, List<GameObject> shop, int NbrOfShop)
    {
        int random;

        for(int i =0; i<NbrOfShop;i++)
        {
            random = Random.Range(0, shop.Count);
            Instantiate(shop[random], spawnSpot[i].transform.position, spawnSpot[i].transform.rotation, spawnSpot[i].transform);
            shop.RemoveAt(random);
            _Shops.Add(random);
        }
    }

    void AskForShops()
    {
        GetComponent<PhotonView>().RPC("RPCShopAnswer", PhotonTargets.MasterClient);
    }

    [PunRPC]
    void RPCShopAnswer()
    {
        GetComponent<PhotonView>().RPC("RPCShopReceive", PhotonTargets.Others, _Shops.ToArray());
    }

    [PunRPC]
    void RPCShopReceive(int[] shops)
    {
        string ext = "(";

        foreach (int i in shops)
        {
            ext += i + ":";
        }

        ext += ")";

        FindObjectOfType<HUD>().ShowMessages(ext, 10);

        for (int i = 0; i < nbr_of_small_shops; ++i)
        {
            Instantiate(small_shops[shops[i]], small_spawnLocations[i].transform.position, small_spawnLocations[i].transform.rotation, small_spawnLocations[i].transform);
        }

        for (int i = 0; i < nbr_of_med_shops; ++i)
        {
            Instantiate(medium_shops[shops[i + nbr_of_small_shops]], medium_spawnLocations[i].transform.position, medium_spawnLocations[i].transform.rotation, medium_spawnLocations[i].transform);
        }

        for (int i = 0; i < nbr_of_large_shops; ++i)
        {
            Instantiate(large_shops[shops[i + nbr_of_small_shops + nbr_of_med_shops]], large_spawnLocations[i].transform.position, large_spawnLocations[i].transform.rotation, large_spawnLocations[i].transform);
        }
    }
}
