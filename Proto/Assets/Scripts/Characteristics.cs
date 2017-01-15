﻿using UnityEngine;

public class Characteristics : MonoBehaviour
{
    PhotonView m_PhotonView;
    [SerializeField]int[] m_Characteristics = new int[6];

	void Start ()
    {
        m_PhotonView = GetComponent<PhotonView>();
	}

    public int[] GetCharacteristics()
    {
        return m_Characteristics;
    }

    public void TargetIdentified()
    {
        GetComponentInChildren<Renderer>().material.color = Color.red;
    }
}