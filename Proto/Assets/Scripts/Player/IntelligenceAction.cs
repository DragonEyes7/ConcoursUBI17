using UnityEngine;
using System.Collections.Generic;

public class IntelligenceAction : Action
{
    [SerializeField]Transform m_CenterCam;

    void Update()
    {
        if (m_Interact && Input.GetButtonDown("Action"))
        {
            m_Interactive.Interact();
            m_Interact = false;
        }

        if(Input.GetButtonDown("Uplink"))
        {
            //Look at clues
            LookAtClues();
        }
    }

    void LookAtClues()
    {
        Dictionary<string, int> clues = FindObjectOfType<GameManager>().GetIntelligenceClues();
        Debug.Log("Looking at clues...");

        if(clues.ContainsKey("Hair"))
        {
            Debug.Log("The target has " + GetColorName(clues["Hair"]) + " hairs.");
        }

        if (clues.ContainsKey("Nose"))
        {
            Debug.Log("The target has " + GetColorName(clues["Nose"]) + " Nose.");
        }

        if (clues.ContainsKey("Backpack"))
        {
            Debug.Log("The target has " + GetColorName(clues["Backpack"]) + " backpack.");
        }

        Debug.Log("Looking at clues done.");
    }

    string GetColorName(int colorID)
    {
        switch(colorID)
        {
            case 0:
                return "Blue";
            case 1:
                return "Green";
            case 2:
                return "Pink";
            case 3:
                return "Red";
            case 4:
                return "Yellow";
        }

        return null;
    }

    public Transform GetCenterCam()
    {
        return m_CenterCam;
    }
}