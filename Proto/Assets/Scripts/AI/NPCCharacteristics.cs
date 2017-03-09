using UnityEngine;

public class NPCCharacteristics : MonoBehaviour
{
    public Transform PantPart, ShirtPart;
    public Material PantMaterial, ShirtMaterial;
    public GameObject Head;

    void Start ()
    {
        if(PhotonNetwork.isMasterClient)
        {
            //Set the Material for various pieces
            Instantiate(Head, transform.position, transform.rotation, transform);
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
        GetComponent<PhotonView>().RPC("RPCClothReceive", PhotonTargets.Others, Head.name, PantMaterial.name, ShirtMaterial.name);
    }

    [PunRPC]
    void RPCClothReceive(string headName, string pantName, string shirtName)
    {
        GameObject[] HeadList = Resources.LoadAll<GameObject>("Head");
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

        foreach(GameObject head in HeadList)
        {
            if (head.name == headName)
            {
                Instantiate(head, transform.position, transform.rotation, transform);
            }
        }
    }
}
