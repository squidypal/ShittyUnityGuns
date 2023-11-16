using System.Collections;
using TMPro;
using UnityEngine;

public class squidypalPlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float sprintSpeed = 8f;
    public float jumpForce = 5f;
    public Camera playerCamera;
    private Vector3 direction;
    private Rigidbody rb;
    private bool isJumping = false;
    private CapsuleCollider capCollider;
    
    public float crouchSpeed = 0.4f;
    public float standHeight = 0f;
    public float crouchHeight = -0.5f;
    private bool isCrouching = false;
    public static float currentSpeed;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        capCollider = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 cameraForward = playerCamera.transform.forward;
        Vector3 cameraRight = playerCamera.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        direction = cameraForward * moveVertical + cameraRight * moveHorizontal;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            direction *= sprintSpeed;
        }
        else
        {
            direction *= speed;
        }

        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
        }
        HandleCrouch();
        currentSpeed = direction.magnitude / sprintSpeed;  // Normalized assuming sprintSpeed is the max

    }
    private void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (isCrouching)
            {
                // Try to stand up.
                // First, check if there's space above the player.
                if (!Physics.Raycast(transform.position, Vector3.up, standHeight - crouchHeight))
                {
                    isCrouching = false;
                    capCollider.height = standHeight;
                    transform.position += new Vector3(0, (standHeight - crouchHeight) / 2, 0);
                }
            }
            else
            {
                isCrouching = true;
                capCollider.height = crouchHeight;
                transform.position -= new Vector3(0, (standHeight - crouchHeight) / 2, 0);
            }
        }

        if (isCrouching)
        {
            direction *= crouchSpeed;
        }
    }


    private void FixedUpdate()
    {
        if (rb.isKinematic)
        {
            // Check if there are any obstacles in the movement direction.
            RaycastHit hitInfo;
            if (!rb.SweepTest(direction.normalized, out hitInfo, direction.magnitude * Time.deltaTime))
            {
                transform.position += direction * Time.deltaTime;
            }
        }
        else
        {
            rb.MovePosition(transform.position + direction * Time.deltaTime);
        }

        if (isJumping)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            isJumping = false;
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, capCollider.bounds.extents.y + 0.1f);
    }
}