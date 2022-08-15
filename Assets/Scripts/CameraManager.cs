using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
	public List<CameraStruct> cameras = new List<CameraStruct>();

    public void SetCamera(CameraType cam)
	{
        foreach(CameraStruct camStruct in cameras)
		{
			if (camStruct.type == cam)
			{
				camStruct.freeLookCamera.gameObject.SetActive(true);
			}
			else
			{
				camStruct.freeLookCamera.gameObject.SetActive(false);
			}
		}
	}

	[System.Serializable]
	public struct CameraStruct
	{
		public CinemachineFreeLook freeLookCamera;
		public CameraType type;
	}

	public enum CameraType
	{
        Player, Car, Aim
	}
}
