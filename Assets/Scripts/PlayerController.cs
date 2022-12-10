using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed = 8f;
    public float jumpStrength = 2f;
    public float rotateVelocity = 20f;
    public GameObject playerModel;
    public GameObject cameraController;

    private Rigidbody playerRigidbody;
    private bool onGround = false;
    private int jumpCooldown = 30;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        handleMovement();
        handleRotation();
    }

    void handleMovement()
    {
        jumpCooldown = Mathf.Max(jumpCooldown - 1, 0);
        if(!onGround && jumpCooldown == 0)
        {
            Ray floorCheck = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(floorCheck, .2f))
            {
                onGround = true;
            }
        }

        float inputX = UnityEngine.Input.GetAxis("Horizontal");
        float inputY = UnityEngine.Input.GetAxis("Jump");
        float inputZ = UnityEngine.Input.GetAxis("Vertical");
        Vector3 input = new Vector3(inputX, 0f, inputZ) * 100f * movementSpeed * Time.deltaTime;
        Vector3 jumpInput = new Vector3();
        if(onGround && inputY > 0.1f)
        {
            onGround = false;
            jumpCooldown = 30;
            jumpInput = new Vector3(0f, jumpStrength, 0f);
        }
        Vector3 gravity = Vector3.down * 9.81f * Time.deltaTime;
        playerRigidbody.velocity = new Vector3(transform.TransformDirection(input).x, playerRigidbody.velocity.y, transform.TransformDirection(input).z);
        playerRigidbody.velocity += jumpInput;
        playerRigidbody.velocity += gravity;
        playerRigidbody.angularVelocity = new Vector3();
    }

    void handleRotation()
    {
        Quaternion playerTargetYaw = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
        Quaternion cameraTargetPitch = Quaternion.Euler(cameraController.transform.eulerAngles.x, transform.rotation.eulerAngles.y, 0f);

        float inputAngleX = UnityEngine.Input.GetAxisRaw("Mouse X");
        float inputAngleY = -UnityEngine.Input.GetAxisRaw("Mouse Y");

        if (Mathf.Abs(inputAngleX) > 0.1f)
        {
            playerTargetYaw *= Quaternion.AngleAxis(rotateVelocity * inputAngleX * 100f * Time.deltaTime, Vector3.up);
            cameraTargetPitch *= Quaternion.AngleAxis(rotateVelocity * inputAngleX * 100f * Time.deltaTime, Vector3.up);
        }
        if (Mathf.Abs(inputAngleY) > 0.1f)
        {
            cameraTargetPitch *= Quaternion.AngleAxis(rotateVelocity * inputAngleY * 100f * Time.deltaTime, Vector3.right);
        }

        transform.rotation = playerTargetYaw;
        cameraController.transform.rotation = cameraTargetPitch;
    }
}
