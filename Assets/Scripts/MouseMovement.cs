using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class MouseMovement : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    [SerializeField] float jumpHeight = 0.5f;
    [SerializeField] float gravity = 2f;
    [SerializeField] float tetherSnapDistance = .3f;

    private Vector3 moveDirection;
    private bool tethered = false;
    private Tether tether;
    private bool justJumped = false;
    private Quaternion visualRotation = Quaternion.Euler(0,0,0);

    // How fast the player turns to face movement direction
    [SerializeField] float turnSmoothTime = 0.1f;
    // not really used, needed for SmoothDampAngle(...)
    float turnSmoothVelocity;

    [SerializeField] GameObject playerVisual;

    CharacterController controller;
    GameObject mainCamera;
    Animator animator;

    public GameObject[] pinkCubes;
    private int count;
    [SerializeField] Text textField;
    [SerializeField] int currentLevel;
    [SerializeField] int totalElements;

    [SerializeField] GameObject pauseMenu;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        animator = GetComponentInChildren<Animator>();

        pinkCubes = new GameObject[totalElements];

        string name="ButtonCube";
        for(int i=1;i<=totalElements;i++){
            pinkCubes[i-1] = GameObject.Find(name+i);
        }

        // Create a new GameState object with the current game data
        GameState gameState = new GameState(currentLevel);

        // Serialize the GameState object to a byte array
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream stream = new MemoryStream();
        formatter.Serialize(stream, gameState);
        byte[] data = stream.ToArray();

        // Save the byte array to a file
        File.WriteAllBytes("savedata.dat", data);
    }

    void Update()
    {
        if (Input.GetButtonDown("Interact")){
            checkForInteraction();
        }
        if(Input.GetButtonDown("Cancel")){
            openPauseMenu();
        }
        if(Input.GetButtonDown("QuestAbgabe")){
            handInQuest();
        }
        if (tethered)
        {
            moveTethered();
        } else
        {
            moveFreely();
        }
    }

    private void LateUpdate()
    {
        handleVisuals();
    }

    void handInQuest(){
        if(count==totalElements){
            SceneManager.LoadScene("Quest"+(currentLevel+1));
        }
    }
    void openPauseMenu(){
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }
    void checkForInteraction(){
        for(int i=0;i<totalElements;i++){
            if(pinkCubes[i].activeSelf){
                float distance = Vector3.Distance(playerVisual.transform.position, pinkCubes[i].transform.position);
                if(distance<0.5){
                    pinkCubes[i].SetActive(false);
                    count++;
                    if(currentLevel==1){
                        Debug.Log("Gegenstand gefunden! Du hast jetzt "+count+" Gegenstände identifiziert");
                        textField.text = count + " von "+totalElements+" Gegenständen gefunden.";
                    }else{
                        Debug.Log("Mangel identifiziert! Du hast jetzt "+count+" Mängel gefunden");
                        textField.text = count + " von "+totalElements+" Mängel identifiziert.";
                    }
                }
            }
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

            visualRotation = Quaternion.Euler(0f, visualAngle, 0f);

            move = transform.forward;
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

                transform.position = tether.GetClosestPoint(transform.position, out dir);

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
            if (!controller.isGrounded)
            {
                moveDirection.y += Mathf.Sqrt(jumpHeight * -2.0f * gravity);
            }
            moveFreely();
            return;
        }
        moveDirection = new Vector3();
        justJumped = false;

        float movementInput = Input.GetAxis("Vertical");
        if (Mathf.Abs(movementInput) > 0.1f)
        {
            Vector3 lineDir;
            Vector3 closestPoint = tether.GetClosestPoint(transform.position, out lineDir);
            //Vector3 cameraDir = mainCamera.transform.forward;
            int dirSwitch = movementInput > 0 ? 1 : -1;
            Vector3 dir = -lineDir;// * (Vector3.Dot(lineDir, cameraDir) > 0 ? 1 : -1);

            transform.position += movementInput * dir * speed * Time.deltaTime;
            transform.position = tether.GetClosestPoint(transform.position, out lineDir);

            if(tether.shouldDrop(transform.position, dir * dirSwitch))
            {
                tether = null;
                tethered = false;
                moveFreely();
                return;
            }

            // Visual stuff
            playerVisual.transform.rotation = new Quaternion();
            playerVisual.transform.LookAt(transform.position + dir * dirSwitch);

            Vector3 mouseDownAdjusted = tether.mouseDown;
            float scalar = Vector3.Dot(playerVisual.transform.forward, mouseDownAdjusted);
            mouseDownAdjusted -= playerVisual.transform.forward * scalar;

            float downDiff = Vector3.Dot(playerVisual.transform.right, mouseDownAdjusted.normalized);
            playerVisual.transform.Rotate(playerVisual.transform.forward, downDiff * 90f, Space.World);

            // Stupid edge case
            float upDiff = Vector3.Dot(playerVisual.transform.up, mouseDownAdjusted);
            if (upDiff > 0)
            {
                playerVisual.transform.Rotate(playerVisual.transform.forward, 180f, Space.World);
            }
            visualRotation = playerVisual.transform.rotation;
        }

        // animation
        animator.SetBool("IsWalking", Mathf.Abs(movementInput) > 0.1f);
    }

    void handleVisuals()
    {
        playerVisual.transform.rotation = visualRotation;
    }
}
