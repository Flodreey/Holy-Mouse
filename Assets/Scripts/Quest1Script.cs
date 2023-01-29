using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Quest1Script : MonoBehaviour
{
    [SerializeField] GameObject questPanel;
    private bool isActive;
    [SerializeField] GameObject playerVisual;

    private GameObject[] cubeFolders;
    private GameObject[] stickyNodes;
    private int count;
    [SerializeField] TextMeshProUGUI textField1;
    [SerializeField] TextMeshProUGUI textField2;
    [SerializeField] int currentLevel;
    [SerializeField] int totalElements;

    [SerializeField] GameObject pauseMenu;
    
    // Start is called before the first frame update
    void Start()
    {
        textField1.text = "0 / " + totalElements;
        textField2.text = textField1.text;

        isActive =false;
        cubeFolders = new GameObject[totalElements];
        stickyNodes = new GameObject[totalElements];

        string name="Buttons";
        for(int i=1;i<=totalElements;i++){
            cubeFolders[i-1] = GameObject.Find(name+i);
        }
        //Assign StickyNodes GameObjects
        stickyNodes[0] = GameObject.Find("BänkeNordStickyNotes");
        stickyNodes[0].SetActive(false);
        stickyNodes[1] = GameObject.Find("AltarStickyNotes");
        stickyNodes[1].SetActive(false);
        stickyNodes[2] = GameObject.Find("LautsprecherStickyNotes");
        stickyNodes[2].SetActive(false);
        stickyNodes[3] = GameObject.Find("BänkeSüdStickyNotes");
        stickyNodes[3].SetActive(false);
        stickyNodes[4] = GameObject.Find("BänkeNordStickyNotes");
        stickyNodes[4].SetActive(false);

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
            checkForInteraction();
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
            SceneManager.LoadScene("Quest"+(currentLevel+1));
        }
    }
    void openPauseMenu(){
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }
    void checkForInteraction(){
        for(int i=0;i<totalElements;i++){
            if(cubeFolders[i].activeSelf && cubeFolders[i].GetComponent<BoxCollider>().bounds.Contains(playerVisual.transform.position)){
                Debug.Log("Check for Buttons"+i);
                GameObject child;
                for(int j=0;j<cubeFolders[i].transform.childCount;j++){
                    child=cubeFolders[i].transform.GetChild(j).gameObject;
                    float distance = Vector3.Distance(playerVisual.transform.position, child.transform.position);
                    if(distance<0.5){
                        cubeFolders[i].SetActive(false);
                        count++;
                        Debug.Log("Gegenstand gefunden! Du hast jetzt "+count+" Gegenstände identifiziert");
                        textField1.text = count + " / " + totalElements;
                        textField2.text = textField1.text;
                        stickyNodes[i].SetActive(true);
                    }
                }
            }
        }
    }

}
