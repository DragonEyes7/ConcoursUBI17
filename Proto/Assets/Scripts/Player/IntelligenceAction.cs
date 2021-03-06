﻿using UnityEngine;

public class IntelligenceAction : Action
{
    [SerializeField]Transform m_CenterCam;

    HUD m_Hud;

    new void Start()
    {
        base.Start();

        m_Hud = FindObjectOfType<HUD>();
    }

    void Update()
    {
        if (InputMode.isInMenu) return;
        if (m_Interact && Input.GetButtonDown("Action"))
        {
            m_Interactive.Interact();
            m_Interact = false;
        }

        if(Input.GetButtonDown("Uplink"))
        {
            LookAtClues();
        }
    }

    void LookAtClues()
    {
        m_Hud.LookAtClues();
    }

    public Transform GetCenterCam()
    {
        return m_CenterCam;
    }
}