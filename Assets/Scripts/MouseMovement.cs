using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class MouseMovement : MonoBehaviour
{
    // move speed of the player
    [SerializeField] float speed = 1f;
    [SerializeField] float maxSpeed = 5f;

    // the height the player can jump
    [SerializeField] float jumpHeight = 5f;

    // How fast the player turns to face movement direction
    [SerializeField] float turnSmoothTime = 0.1f;
    // not really used, needed for SmoothDampAngle(...)
    float turnSmoothVelocity;

    Rigidbody rigidbody;
    GameObject mainCamera;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector2 inputDirection = new Vector3(h, v).normalized;

        if (inputDirection.magnitude >= 0.1f)
        {
            // calculating player angle around y, which depends on inputs and on the rotation of the camera
            float targetAngle = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg + mainCamera.transform.eulerAngles.y;
            // smoothes the rotation when player turns
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            // calculating direction in which player looks
            Vector3 direction = Quaternion.Euler(0.0f, angle, 0.0f) * Vector3.forward;

            rigidbody.AddForce(direction.normalized * speed);
        }

        // player can only jump if he is on the ground
        if (Input.GetButton("Jump") && isOnGround())
        {
            rigidbody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
        }

        // limiting the player velocity to maxSpeed
        if (rigidbody.velocity.magnitude > maxSpeed)
        {
            rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;
        }
    }

    bool isOnGround()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.05f);
    }
}
