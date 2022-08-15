using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
	public bool isControlling;
	public PlayerController pilot;

	public float maxAcceleration = 30.0f;
	public float brakeAcceleration = 50.0f;

	public float turnSensitivity = 1.0f;
	public float maxSteerAngle = 30.0f;

	public List<Wheel> wheels;

	float moveInput;
	float steerInput;

	public Vector3 _centerOfMass;

	private Rigidbody rig;
	private float drivingTimer;

	private void Start()
	{
		rig = GetComponent<Rigidbody>();
		rig.centerOfMass = _centerOfMass;
		AnimateWheels();
	}

	private void Update()
	{
		if (isControlling)
		{
			drivingTimer += Time.deltaTime;
			GetInput();
			AnimateWheels();

			if (Input.GetKeyDown(KeyCode.F) && pilot && drivingTimer > .5f)
			{
				StopDrive();
			}
		}
	}

	private void LateUpdate()
	{
		if (isControlling)
		{
			Move();
			Steer();
			Brake(Input.GetKey(KeyCode.Space));
		}
		else
		{
			Brake(true);
		}
	}

	void GetInput()
	{
		moveInput = Input.GetAxis("Vertical");
		steerInput = Input.GetAxis("Horizontal");
	}

	void Move()
	{
		// It makes the reverse slower
		foreach (var wheel in wheels)
		{
			wheel.wheelCollider.motorTorque = moveInput * 600 * maxAcceleration * Time.deltaTime;
		}
	}

	void Steer()
	{
		foreach (var wheel in wheels)
		{
			if (wheel.axel == Axel.Front)
			{
				var _steerAngle = steerInput * turnSensitivity * maxSteerAngle;
				wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, _steerAngle, .6f);
			}
		}
	}

	void Brake(bool brake)
	{
		if (brake)
		{
			foreach (var wheel in wheels)
			{
				wheel.wheelCollider.brakeTorque = 300 * brakeAcceleration * Time.deltaTime;
			}
		}
		else
		{
			foreach (var wheel in wheels)
			{
				wheel.wheelCollider.brakeTorque = 0;
			}
		}

	}

	void AnimateWheels()
	{
		foreach (var wheel in wheels)
		{
			Quaternion rot;
			Vector3 pos;
			wheel.wheelCollider.GetWorldPose(out pos, out rot);
			wheel.wheelModel.transform.position = pos;
			wheel.wheelModel.transform.rotation = rot;
		}
	}

	public void StartDrive(PlayerController player)
	{
		pilot = player;
		isControlling = true;
		pilot.isControlling = false;
		pilot.gameObject.SetActive(false);
		pilot.transform.position = transform.position;
		pilot.transform.parent = transform;
		FindObjectOfType<CameraManager>().SetCamera(CameraManager.CameraType.Car);
	}

	public void StopDrive()
	{
		drivingTimer = 0;
		isControlling = false;
		pilot.isControlling = true;
		pilot.gameObject.SetActive(true);
		pilot.transform.parent = null;
		pilot.transform.position = transform.position - transform.right;
		pilot = null;
		FindObjectOfType<CameraManager>().SetCamera(CameraManager.CameraType.Player);
	}

	public enum Axel
	{
		Front,
		Rear
	}

	public enum Side
	{
		Right,
		Left
	}

	[System.Serializable]
	public struct Wheel
	{
		public GameObject wheelModel;
		public WheelCollider wheelCollider;
		public Axel axel;
		public Side side;
	}

}