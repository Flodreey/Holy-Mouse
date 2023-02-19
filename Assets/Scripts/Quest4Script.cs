using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Quest4Script : MonoBehaviour
{
    [SerializeField] GameObject questPanel;
    private bool isActive;
    [SerializeField] GameObject playerVisual;

    private GameObject ladderEntranceMaterial;
    private GameObject altarEntranceMaterial;
    [SerializeField] GameObject ladderEntrance;
    [SerializeField] GameObject altarEntrance;
    [SerializeField] GameObject ladderEntranceObject;
    [SerializeField] GameObject altarEntranceObject;
    private int placed;
    [SerializeField] TextMeshProUGUI textField1;
    [SerializeField] TextMeshProUGUI textField2;
    private int currentLevel;
    [SerializeField] GameObject planArea;

    [SerializeField] GameObject pauseMenu;
    
    // Start is called before the first frame update
    void Start()
    {
        textField1.text = "0 / " + "2";
        textField2.text = textField1.text;

        isActive=false;
        placed=0;
        currentLevel=4;

        ladderEntranceMaterial = GameObject.Find("ladderEntranceMaterial");
        altarEntranceMaterial = GameObject.Find("altarEntranceMaterial");

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
            if(ladderEntranceMaterial.activeSelf){
                float distance = Vector3.Distance(playerVisual.transform.position, ladderEntranceMaterial.transform.position);
                if(distance<0.5){
                    ladderEntranceMaterial.SetActive(false);
                    ladderEntranceMaterial.transform.parent.gameObject.SetActive(false);
                    ladderEntrance.SetActive(true);
                    if (!altarEntranceMaterial.activeSelf) {
                        GameObject g = GameObject.Find("QuestMessagePrefab");
                        QuestMessageScript endQuestMessageScript = g.GetComponent<QuestMessageScript>();
                        endQuestMessageScript.ShowMessage();
                    }
                }
            }else{
                if(ladderEntrance.activeSelf && ladderEntrance.GetComponent<BoxCollider>().bounds.Contains(playerVisual.transform.position)){
                    ladderEntranceObject.SetActive(true);
                    ladderEntrance.SetActive(false);
                    placed++;
                    textField1.text = placed+" / " + "2";
                    textField2.text = textField1.text;
                }
            }
            if(altarEntranceMaterial.activeSelf){
                float distance = Vector3.Distance(playerVisual.transform.position, altarEntranceMaterial.transform.position);
                if(distance<0.5){
                    altarEntranceMaterial.SetActive(false);
                    altarEntranceMaterial.transform.parent.gameObject.SetActive(false);
                    altarEntrance.SetActive(true);
                    if (!ladderEntranceMaterial.activeSelf) {
                        GameObject g = GameObject.Find("QuestMessagePrefab");
                        QuestMessageScript endQuestMessageScript = g.GetComponent<QuestMessageScript>();
                        endQuestMessageScript.ShowMessage();
                    }
                }
            }else{
                if(altarEntrance.activeSelf && altarEntrance.GetComponent<BoxCollider>().bounds.Contains(playerVisual.transform.position)){
                    altarEntranceObject.SetActive(true);
                    altarEntrance.SetActive(false);
                    placed++;
                    textField1.text = placed+" / " + "2";
                    textField2.text = textField1.text;
                }
            }
        }
        if(Input.GetButtonDown("Cancel")){
            openPauseMenu();
        }
        if (Input.GetButtonDown("Tab"))
        {
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
        if(placed==2){
            //SceneManager.LoadScene("MainMenu");
            CutsceneManager.StartCutscene();
        }
    }
    void openPauseMenu(){
        pauseMenu.SetActive(!pauseMenu.activeSelf);

        if (pauseMenu.activeSelf)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}