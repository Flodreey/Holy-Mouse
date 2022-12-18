using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseMovement : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float jumpHeight = 0.5f;
    [SerializeField] float gravity = 2f;

    private Vector3 moveDirection;

    // How fast the player turns to face movement direction
    [SerializeField] float turnSmoothTime = 0.1f;
    // not really used, needed for SmoothDampAngle(...)
    float turnSmoothVelocity;

    CharacterController controller;
    GameObject mainCamera;
    Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        bool groundedPlayer = controller.isGrounded;
        if (groundedPlayer && moveDirection.y < 0)
        {
            moveDirection.y = 0f;
        }

        // user input
        Vector2 inputDirection = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        Vector3 move = new Vector3();
        if (inputDirection.magnitude > 0.1)
        {
            // calculating player angle around y, which depends on inputs and on the rotation of the camera
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;

            // smoothes the rotation when player turns
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            // calculating direction in which player looks depending on the current rotation angle
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            move = Quaternion.Euler(0.0f, angle, 0.0f) * Vector3.forward;
        }

        controller.Move(move * Time.deltaTime * speed);

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            moveDirection.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }

        moveDirection.y += gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

        // animation
        animator.SetBool("IsWalking", inputDirection.x != 0 || inputDirection.y != 0);
    }
}
