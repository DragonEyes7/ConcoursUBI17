using UnityEngine;

public class NPCCharacteristics : MonoBehaviour
{
    [SerializeField]Transform _Parent;
    public GameObject Hat, Accessory, Facial;
    public Material HatMaterial, AccessoryMaterial, FacialMaterial;

    void Start ()
    {
        if(PhotonNetwork.isMasterClient)
        {
            GameObject hat = Instantiate(Hat);
            hat.transform.SetParent(_Parent, false);
            GameObject accessory = Instantiate(Accessory);
            accessory.transform.SetParent(_Parent, false);
            GameObject facial = Instantiate(Facial);
            facial.transform.SetParent(_Parent, false);

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
                break;
            }
        }

        foreach (Material aMat in AccessoryMatList)
        {
            if (aMat.name == accMatName)
            {
                AccessoryMaterial = aMat;
                break;
            }
        }

        foreach (Material fMat in FacialMatList)
        {
            if (fMat.name == facMatName)
            {
                FacialMaterial = fMat;
                break;
            }
        }

        foreach (GameObject h in HatList)
        {
            if (h.name == hatName)
            {
                GameObject hat = Instantiate(h);
                hat.transform.SetParent(_Parent, false);
                hat.GetComponentInChildren<Renderer>().material = HatMaterial;
                break;
            }
        }

        foreach (GameObject acc in AccessoryList)
        {
            if (acc.name == accessoryName)
            {
                GameObject accessory = Instantiate(acc);
                accessory.transform.SetParent(_Parent, false);
                accessory.GetComponentInChildren<Renderer>().material = AccessoryMaterial;
                break;
            }
        }

        foreach (GameObject fac in FacialList)
        {
            if (fac.name == facialName)
            {
                GameObject facial = Instantiate(fac);
                facial.transform.SetParent(_Parent, false);
                facial.GetComponentInChildren<Renderer>().material = FacialMaterial;
                break;
            }
        }
    }
}
