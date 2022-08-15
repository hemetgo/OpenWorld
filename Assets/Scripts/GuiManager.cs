using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GuiManager : MonoBehaviour
{
    public TextMeshProUGUI interactionText;
    public GameObject aimDot;

	public void ActiveAimDot(bool active)
	{
		aimDot.SetActive(active);
	}

    public void SetInteractionText(bool active, string text = "")
	{
		interactionText.text = text;
		interactionText.gameObject.SetActive(active);
	}
}
