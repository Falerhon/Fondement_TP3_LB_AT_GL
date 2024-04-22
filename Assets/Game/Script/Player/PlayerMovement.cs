using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDir;

    Rigidbody rb;

    [Header("Jump")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;

    private bool isJumpReady = true;

    [Header("Fly")]
    private bool isFlying = false;
    [SerializeField] private float raiseSpeed;
    [SerializeField] private float lowerSpeed;
    [SerializeField] private float flyDrag;
    [SerializeField] private float flySpeed;



    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask groundLayer;
    bool isGrounded = true;

    [Header("Keybind")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode flyKey = KeyCode.V;
    public KeyCode lowerKey = KeyCode.C;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }



    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f);
        SpeedControll();

        if (!isFlying)
        {
            moveInput();
        }
        else
        {
            flyInput();
        }

        if (isGrounded && !isFlying)
        {
            rb.drag = groundDrag;
        } else if (isFlying)
        {
            rb.drag = flyDrag;
        }else
        {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {

        movePlayer();
        
    }

    private void moveInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && isJumpReady && isGrounded )
        {
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (Input.GetKeyDown(flyKey))
        {
            EnableFly();
        }
    }

    private void flyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey))
        {
            rb.AddForce(Vector3.up * raiseSpeed * 10f, ForceMode.Force);
        }

        if(Input.GetKey(lowerKey)) 
        {
            rb.AddForce(Vector3.down * lowerSpeed * 10f, ForceMode.Force);
        }

        if (Input.GetKeyDown(flyKey))
        {
            DisableFly();
        }
    }

    private void movePlayer()
    {
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(isGrounded)
            rb.AddForce(moveDir.normalized * moveSpeed * 10f, ForceMode.Force);
        else
            rb.AddForce(moveDir.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void flyPlayer()
    {
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDir.normalized * flySpeed * 10f, ForceMode.Force);
    }

    private void SpeedControll()
    {
        Vector3 velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(velocity.magnitude > moveSpeed)
        {
            Vector3 maxVelocity = velocity.normalized * moveSpeed;
            rb.velocity = new Vector3(maxVelocity.x, rb.velocity.y, maxVelocity.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce * 10f, ForceMode.Force);

        isJumpReady = false;
    }

    private void ResetJump()
    {
        isJumpReady = true;
    }

    private void EnableFly()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        isFlying = true;
        rb.useGravity = false;
    }

    private void DisableFly()
    {
        isFlying = false;
        rb.useGravity = true;
    }
}
