using UnityEngine;

public class NPCCharacteristics : MonoBehaviour
{
    public GameObject Hat, Accessory, Facial;
    public Material HatMaterial, AccessoryMaterial, FacialMaterial;

    void Start ()
    {
        if(PhotonNetwork.isMasterClient)
        {
            GameObject hat = Instantiate(Hat, transform.position, transform.rotation, transform);
            GameObject accessory = Instantiate(Accessory, transform.position, transform.rotation, transform);
            GameObject facial = Instantiate(Facial, transform.position, transform.rotation, transform);

            hat.GetComponentInChildren<Renderer>().material = HatMaterial;
            accessory.GetComponentInChildren<Renderer>().material = AccessoryMaterial;
            facial.GetComponentInChildren<Renderer>().material = FacialMaterial;
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
        GetComponent<PhotonView>().RPC("RPCClothReceive", PhotonTargets.Others
            , Hat.name
            , Accessory.name
            , Facial.name
            , HatMaterial.name
            , AccessoryMaterial.name
            , FacialMaterial.name);
    }

    [PunRPC]
    void RPCClothReceive(string hatName, string accessoryName, string facialName, string hatMatName, string accMatName, string facMatName)
    {
        GameObject[] HatList = Resources.LoadAll<GameObject>("Hats");
        GameObject[] AccessoryList = Resources.LoadAll<GameObject>("Accessories");
        GameObject[] FacialList = Resources.LoadAll<GameObject>("Facials");

        Material[] HatMatList = Resources.LoadAll<Material>("Materials/Hat");
        Material[] AccessoryMatList = Resources.LoadAll<Material>("Materials/Accessory");
        Material[] FacialMatList = Resources.LoadAll<Material>("Materials/Facial");

        foreach(Material hMat in HatMatList)
        {
            if(hMat.name == hatMatName)
            {
                HatMaterial = hMat;
            }
        }

        foreach (Material aMat in AccessoryMatList)
        {
            if (aMat.name == accMatName)
            {
                AccessoryMaterial = aMat;
            }
        }

        foreach (Material fMat in FacialMatList)
        {
            if (fMat.name == facMatName)
            {
                FacialMaterial = fMat;
            }
        }

        foreach (GameObject h in HatList)
        {
            if (h.name == hatName)
            {
                GameObject hat = Instantiate(h, transform.position, transform.rotation, transform);

                hat.GetComponentInChildren<Renderer>().material = HatMaterial;
                break;
            }
        }

        foreach (GameObject acc in AccessoryList)
        {
            if (acc.name == accessoryName)
            {
                GameObject accessory = Instantiate(acc, transform.position, transform.rotation, transform);
                accessory.GetComponentInChildren<Renderer>().material = AccessoryMaterial;
                break;
            }
        }

        foreach (GameObject fac in FacialList)
        {
            if (fac.name == facialName)
            {
                GameObject facial = Instantiate(fac, transform.position, transform.rotation, transform);
                facial.GetComponentInChildren<Renderer>().material = FacialMaterial;
                break;
            }
        }
    }
}
