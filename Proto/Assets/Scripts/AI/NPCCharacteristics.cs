using UnityEngine;

public class NPCCharacteristics : MonoBehaviour
{
    public Transform PantPart, ShirtPart;
    public Material PantMaterial, ShirtMaterial;
    public GameObject Hat;

    void Start ()
    {
        if(PhotonNetwork.isMasterClient)
        {
            //Set the Material for various pieces
            Instantiate(Hat, transform.position, transform.rotation, transform);
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
        GetComponent<PhotonView>().RPC("RPCClothAnswer", PhotonTargets.MasterClient);
    }

    [PunRPC]
    void RPCClothAnswer()
    {
        GetComponent<PhotonView>().RPC("RPCClothReceive", PhotonTargets.Others, Hat.name, PantMaterial.name, ShirtMaterial.name);
    }

    [PunRPC]
    void RPCClothReceive(string hatName, string pantName, string shirtName)
    {
        GameObject[] HatList = Resources.LoadAll<GameObject>("Hat");
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

        foreach(GameObject hat in HatList)
        {
            if (hat.name == hatName)
            {
                Instantiate(hat, transform.position, transform.rotation, transform);
            }
        }
    }
}
