using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed;
    public float jumpForce;
    private bool isJumping;

    [Header("States")]
    public bool isControlling;

    [Header("Components")]
    public AimController aimController;
    public Animator animator;
    private Rigidbody rig;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isControlling)
        {
            Jump();

            isGrounded = IsGrounded();
        }
    }

    private void FixedUpdate()
    {
        if (isControlling)
        {
            Movement();
        }
    }

	private void Movement()
    {
        // Get input
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        if (vertical < 0) horizontal = horizontal * -1;

        // Rotate
        if (!aimController.isAiming)
        {
            if (vertical != 0)
            {
                Vector3 camAngles = Camera.main.transform.eulerAngles;
                transform.eulerAngles = new Vector3(0, camAngles.y, 0);

                if (horizontal != 0)
                {
                    Vector3 lookDirection = transform.forward + transform.right * horizontal;
                    transform.LookAt(transform.position + lookDirection);
                }
            }
            else
            {
                if (horizontal != 0)
                {
                    Vector3 lookDirection = Camera.main.transform.right * horizontal;
                    transform.LookAt(transform.position + lookDirection);
                }
            }
        }
		else
		{
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, transform.eulerAngles.z);
		}

        // Speed modifier
        float speedModifier = 1;
        if (vertical < 0 || Input.GetButton("Walk")) speedModifier = .2f;
        if (aimController.isAiming) speedModifier = 0;

        // Move
        Vector3 moveDirection = transform.forward * vertical;
        if (vertical == 0 && horizontal != 0) moveDirection = Camera.main.transform.right * horizontal;
        moveDirection = moveDirection.normalized * moveSpeed * speedModifier;
        rig.velocity = new Vector3(moveDirection.x, rig.velocity.y, moveDirection.z);

        // Animator
        animator.SetFloat("CurrentSpeed", Mathf.Abs(moveDirection.normalized.magnitude * speedModifier));
        animator.SetBool("Aiming", aimController.isAiming);
    }

    private void Jump()
	{
        if (isGrounded && Input.GetButtonDown("Jump"))
		{
            isJumping = true;
            rig.velocity = new Vector3(rig.velocity.x, jumpForce, rig.velocity.z);
        }

        if (isGrounded && rig.velocity.y < 0)
		{
            isJumping = false;
		}

        animator.SetBool("Jumping", isJumping);
    }

    private bool IsGrounded()
    {
        bool result;
        Vector3 origin = new Vector3(transform.position.x, transform.position.y + 0.3f, transform.position.z);

        result = Physics.SphereCast(origin, 0.25f, Vector3.down, out RaycastHit hit, 0.25f);

        return result;
    }

    // Hidden variables
    [HideInInspector] public bool isGrounded;
}
