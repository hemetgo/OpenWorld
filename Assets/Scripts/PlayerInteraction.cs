using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
	public float range;
	public PlayerController playerController;

	private GuiManager guiManager;

	private void Start()
	{
		guiManager = FindObjectOfType<GuiManager>();
	}

	private void Update()
	{
		DriveControl();
	}

	private void DriveControl()
	{
		if (!playerController.gameObject.activeSelf)
		{
			guiManager.SetInteractionText(false);
			return;
		}

		if (Physics.Raycast(transform.position + Vector3.up, transform.forward, out RaycastHit hit, range, LayerMask.GetMask("Car")))
		{
			if (hit.collider)
			{
				guiManager.SetInteractionText(true, "Press F to drive");
				if (Input.GetKeyDown(KeyCode.F))
				{
					if (hit.collider.gameObject.layer == 7)
					{
						CarController car = hit.collider.GetComponent<CarController>();
						car.StartDrive(playerController);
						guiManager.SetInteractionText(false);
					}
				}
			}
		}
		else
		{
			guiManager.SetInteractionText(false);
		}
	}
}
