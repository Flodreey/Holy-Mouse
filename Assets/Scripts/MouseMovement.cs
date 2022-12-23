using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseMovement : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float jumpHeight = 0.5f;
    [SerializeField] float gravity = 2f;
    [SerializeField] float tetherSnapDistance = .2f;

    private Vector3 moveDirection;
    private bool tethered = false;
    private Tether tether;
    private bool justJumped = false;

    // How fast the player turns to face movement direction
    [SerializeField] float turnSmoothTime = 0.1f;
    // not really used, needed for SmoothDampAngle(...)
    float turnSmoothVelocity;

    [SerializeField] GameObject playerVisual;

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
        if (tethered)
        {
            moveTethered();
        } else
        {
            moveFreely();
        }
    }

    void moveFreely()
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
            float visualAngle = Mathf.SmoothDampAngle(playerVisual.transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            // calculating direction in which player looks depending on the current rotation angle
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            playerVisual.transform.rotation = Quaternion.Euler(0f, visualAngle, 0f);

            move = Quaternion.Euler(0.0f, targetAngle, 0.0f) * Vector3.forward;
        }

        controller.Move(move * Time.deltaTime * speed);

        // Changes the height position of the player..
        if (!justJumped && Input.GetButtonDown("Jump"))
        {
            justJumped = true;
            if (groundedPlayer)
            {
                moveDirection.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
            }
            

            float tetherDist = Mathf.Infinity;
            Vector3 closestPoint = transform.position + new Vector3(0, 1000, 0);
            Vector3 dir;
            Tether closest = null;
            foreach(Tether tether in Global.instance.GetTethers())
            {
                closestPoint = tether.GetClosestPoint(transform.position, out dir);
                float dist = Vector3.Distance(closestPoint, transform.position);
                if (dist < tetherDist)
                {
                    tetherDist = dist;
                    closest = tether;
                }
            }
            
            if(tetherDist < tetherSnapDistance)
            {
                tether = closest;
                tethered = true;

                transform.position = closestPoint;

                moveTethered();
                return;
            }
        }
        justJumped = false;

        moveDirection.y += gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

        // animation
        animator.SetBool("IsWalking", inputDirection.x != 0 || inputDirection.y != 0);
    }

    void moveTethered()
    {
        if (!justJumped && Input.GetButtonDown("Jump"))
        {
            justJumped = true;
            tether = null;
            tethered = false;
            moveDirection.y += Mathf.Sqrt(jumpHeight * -0.5f * gravity);
            moveFreely();
            return;
        }
        justJumped = false;

        float movementInput = Input.GetAxis("Vertical");
        if (Mathf.Abs(movementInput) > 0.1f)
        {
            Vector3 lineDir;
            Vector3 closestPoint = tether.GetClosestPoint(transform.position, out lineDir);
            Vector3 cameraDir = mainCamera.transform.forward;

            Vector3 dir = lineDir * (Vector3.Dot(lineDir, cameraDir) > 0 ? 1 : -1);

            transform.position += movementInput * dir * speed * Time.deltaTime;
            transform.position = tether.GetClosestPoint(transform.position, out lineDir);

            // Visual stuff
            playerVisual.transform.LookAt(transform.position + dir);

            float downDiff = Vector3.Dot(-playerVisual.transform.up, tether.mouseDown);
            playerVisual.transform.Rotate(playerVisual.transform.forward, -downDiff * Mathf.PI, Space.World);
        }

        // animation
        animator.SetBool("IsWalking", Mathf.Abs(movementInput) > 0.1f);
    }
}
