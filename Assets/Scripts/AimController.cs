using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{

    public bool isAiming;
    public Transform weapon;
    public GameObject shootParticlePrefab;
    public GameObject bulletImpactPrefab;

    public PlayerController playerController;
    private CameraManager cameraManager;
    private GuiManager guiManager;

    // Start is called before the first frame update
    void Start()
    {
        cameraManager = FindObjectOfType<CameraManager>();
        guiManager = FindObjectOfType<GuiManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire2") && playerController.isControlling)
        {
            isAiming = true;
            cameraManager.SetCamera(CameraManager.CameraType.Aim);
        }
		else
		{
            isAiming = false;
            cameraManager.SetCamera(CameraManager.CameraType.Player);
        }

        Shoot();

        guiManager.ActiveAimDot(isAiming);
    }

    private void Shoot()
    {
        if (Input.GetButtonDown("Fire1") && isAiming)
        {
            Physics.Raycast(Camera.main.ScreenToWorldPoint(guiManager.aimDot.transform.position), transform.forward, out RaycastHit hit);
            if (hit.collider)
			{
                GameObject shootParticle = Instantiate(shootParticlePrefab, weapon.transform.position, Quaternion.identity);

                GameObject bulletImpact = Instantiate(bulletImpactPrefab, hit.point, Quaternion.identity);
                bulletImpact.transform.LookAt(bulletImpact.transform.position + hit.normal);

                Destroy(shootParticle, 1);
                Destroy(bulletImpact, 1);
			}
        }
    }
}
