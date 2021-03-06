﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingAnimation : MonoBehaviour
{
	Text m_Text;
	[SerializeField]string m_LoadingText;
	[SerializeField]float m_AnimationDelay = 0.5f;
	int m_DotStatus = 0;

	void Start ()
	{
        m_Text = GetComponent<Text>();
		StartCoroutine(Animation(m_AnimationDelay));
	}

	IEnumerator Animation(float delay)
	{
		switch (m_DotStatus)
		{
			case 0:
				m_Text.text = m_LoadingText + "   ";
				++m_DotStatus;
				break;
			case 1:
				m_Text.text = m_LoadingText + ".  ";
				++m_DotStatus;
				break;
			case 2:
				m_Text.text = m_LoadingText + ".. ";
				++m_DotStatus;
				break;
			case 3:
				m_Text.text = m_LoadingText + "...";
				m_DotStatus = 0;
				break;
			default:
				m_Text.text = "You broke the Matrix";
				break;
		}
		yield return new WaitForSecondsRealtime(delay);
		StartCoroutine(Animation(m_AnimationDelay));
	}
}
