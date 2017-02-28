using UnityEngine;

public class NPCCharacteristics : MonoBehaviour
{

    public Transform HairPart, PantPart, ShirtPart;
    public Material HairMaterial, PantMaterial, ShirtMaterial;
    
    void Start ()
    {
        //Set the Material for various pieces
        HairPart.GetComponent<Renderer>().material = HairMaterial;
        PantPart.GetComponent<Renderer>().material = PantMaterial;
        ShirtPart.GetComponent<Renderer>().material = ShirtMaterial;
    }
}
