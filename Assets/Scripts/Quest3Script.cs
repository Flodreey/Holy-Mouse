using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Quest3Script : MonoBehaviour
{
    [SerializeField] GameObject questPanel;
    private bool isActive;
    [SerializeField] GameObject playerVisual;

    public GameObject[] pinkCubes;
    private int count;
    [SerializeField] Text textField;
    private int currentLevel;
    [SerializeField] int totalElements;
    [SerializeField] GameObject planArea;

    [SerializeField] GameObject pauseMenu;
    
    // Start is called before the first frame update
    void Start()
    {
        isActive=false;
        pinkCubes = new GameObject[totalElements];
        count=0;
        currentLevel=3;

        string name="ColliderCube";
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

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Interact")){
            if(count<totalElements){
                checkForInteraction();  
            }
        }
        if(Input.GetButtonDown("Cancel")){
            openPauseMenu();
        }
        if (Input.GetButtonDown("Tab"))
        {
            Debug.Log("Tab button got pressed");
            if(isActive){
                questPanel.SetActive(false);
                isActive=false;
            }else{
                questPanel.SetActive(true);
                isActive=true;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FinishArea"))
        {
            FinishFunction();
        }
    }
    private void FinishFunction()
    {
        if(count==totalElements){
            SceneManager.LoadScene("MainMenu");
        }
    }
    void openPauseMenu(){
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }
    void checkForInteraction(){
        if(pinkCubes[count].activeSelf){
            BoxCollider areaCollider=pinkCubes[count].GetComponent<BoxCollider>();
            if(areaCollider.bounds.Contains(playerVisual.transform.position)){
                pinkCubes[count].SetActive(false);
                count++;
                Debug.Log("Raum erfolgreich identifiziert! Du hast jetzt "+count+" Räume identifiziert");
                switch (count){
                    case 1:
                        textField.text = "Ein geeigneter Raum für ein Büro wird gesucht:\n- der Raum darf kein Sichtkontakt zum Kirchenraum haben\n- zudem sollte der Raum nicht besonders groß sein";
                        break;
                    case 2:
                        textField.text = "Ein geeigneter Raum für die Turnhalle wird gesucht:\n- der Raum sollte nicht in dem großen Kirchenraum sein\n- zudem sollte er genug Platz bieten, damit die Kinder herumtoben können\n- Zielgröße ist eine kleiner Turnhalle";
                        break;
                    default:
                        textField.text = "Du hast alle Räume erfolgreich zugewiesen!\n\nGehe nun zu dem im Kirchenraum ausliegenden Plan und bestätige deine Auswahl mit P.";
                        break;
                }
            }else{
                Debug.Log("Du befindest dich nicht im richtigen Raum.");
            }
        }else{
            Debug.Log("Weird! Der gesuchte Raum ist nicht active. Count Variable wird um 1 erhöht");
            count++;
        }
    }
}
