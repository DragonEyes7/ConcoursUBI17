using UnityEngine;

public class NPCCharacteristics : MonoBehaviour
{

    public Transform HairPart, PantPart, ShirtPart;
    public Material HairMaterial, PantMaterial, ShirtMaterial;

    void Start ()
    {
        if(PhotonNetwork.isMasterClient)
        {
            //Set the Material for various pieces
            HairPart.GetComponent<Renderer>().material = HairMaterial;
            PantPart.GetComponent<Renderer>().material = PantMaterial;
            ShirtPart.GetComponent<Renderer>().material = ShirtMaterial;
        }
        else
        {
            AskForCloth();
        }
    }

    void AskForCloth()
    {
        GetComponent<PhotonView>().RPC("RPCClothAnswer", PhotonTargets.All);
    }

    [PunRPC]
    void RPCClothAnswer()
    {
        if(PhotonNetwork.isMasterClient)
        {
            GetComponent<PhotonView>().RPC("RPCClothReceive", PhotonTargets.All, HairMaterial.name, PantMaterial.name, ShirtMaterial.name);
        }
    }

    [PunRPC]
    void RPCClothReceive(string hairName, string pantName, string shirtName)
    {
        if (PhotonNetwork.isMasterClient) return;
        Material[] HairList = Resources.LoadAll<Material>("Materials/Hair");
        Material[] ClothList = Resources.LoadAll<Material>("Materials/Cloth");
        foreach(Material cloth in ClothList)
        {
            if(cloth.name == pantName)
            {
                PantPart.GetComponent<Renderer>().material = cloth;
            }

            if(cloth.name == shirtName)
            {
                ShirtPart.GetComponent<Renderer>().material = cloth;
            }
        }

        foreach(Material hair in HairList)
        {
            if (hair.name == hairName)
            {
                HairPart.GetComponent<Renderer>().material = hair;
            }
        }
    }
}
