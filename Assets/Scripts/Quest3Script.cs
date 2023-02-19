using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Playables;

public class Quest3Script : MonoBehaviour
{
    [SerializeField] GameObject questPanel;
    private bool isActive;
    [SerializeField] GameObject playerVisual;

    private Dictionary<string, GameObject> rooms;
    private int currentLevel;
    [SerializeField] int totalElements;
    [SerializeField] GameObject planArea;

    [SerializeField] GameObject pauseMenu;

    [SerializeField] GameObject roomButton;

    private Rooms currentRoom;
    private bool inRoom;
    [SerializeField] GameObject QuestChoosingMenu;
    [SerializeField] TextMeshProUGUI roomDescription;
    [SerializeField] TextMeshProUGUI roomName;
    [SerializeField] GameObject roomChoiceButtons;
    [SerializeField] PlayableDirector mouseWriteCutscene;
    private Dictionary<Rooms,Dictionary<string, string>> quest3Dictionary;
    private Button[] buttons;
    private Dictionary<Rooms,RoomButtonClicked> roomChoice;
    private GameObject[] cubes;
    [SerializeField] TextMeshProUGUI textField1;
    // Start is called before the first frame update
    void Start()
    {   
        isActive=false;
        currentLevel=3;
        inRoom=false;

        textField1.text = "0 / " + "4";

        rooms=new Dictionary<string,GameObject>();
        GameObject roomsFolder= GameObject.Find("Rooms");
        for(int i=0;i<roomsFolder.transform.childCount;i++){
            GameObject roomObject=roomsFolder.transform.GetChild(i).gameObject;
            rooms.Add(roomObject.name,roomObject);
        }
        roomChoice=new Dictionary<Rooms,RoomButtonClicked>();
        quest3Dictionary=new Dictionary<Rooms,Dictionary<string,string>>();
        foreach(Rooms room in Enum.GetValues(typeof(Rooms))){
            Dictionary<string,string> textFieldsDictionary=getTextFieldDictionary(room);
            quest3Dictionary.Add(room, textFieldsDictionary);
            roomChoice.Add(room,RoomButtonClicked.None);
        }
        buttons=new Button[3];
        for(int i=0;i<roomChoiceButtons.transform.childCount;i++){
            Button button=roomChoiceButtons.transform.GetChild(i).gameObject.GetComponent<Button>();
            buttons[i]=button;
        }
        buttons[0].onClick.AddListener(Button1Clicked);
        buttons[1].onClick.AddListener(Button2Clicked);
        buttons[2].onClick.AddListener(Button3Clicked);

        cubes=new GameObject[4];
        cubes[0] = GameObject.Find("ButtonEntrance");
        cubes[1] = GameObject.Find("ButtonOrgelempore");
        cubes[2] = GameObject.Find("ButtonRuheraum");
        cubes[3] = GameObject.Find("ButtonAltarraum");

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
    void Button1Clicked(){
        if(inRoom==true){
            roomChoice[currentRoom] = RoomButtonClicked.First;
            colorizeButtons(0);
        }
    }
    void Button2Clicked(){
        if(inRoom==true){
            roomChoice[currentRoom] = RoomButtonClicked.Second;
            colorizeButtons(1);
        }
    }
    void Button3Clicked(){
        if(inRoom==true){
            roomChoice[currentRoom] = RoomButtonClicked.Third;
            colorizeButtons(2);
        }
    }
    void colorizeButtons(int whichOne){
        for(int i=0;i<roomChoiceButtons.transform.childCount;i++){
            TextMeshProUGUI textField=roomChoiceButtons.transform.GetChild(i).gameObject.GetComponentInChildren<TextMeshProUGUI>();
            if(whichOne==i){
                Color color = new Color32(159, 124, 33, 255);
                textField.color = color;
            }else{
                Color color = new Color32(255, 255, 255, 255);
                textField.color = color;
            }
        }
    }
    void colorizeButtons(){
        for(int i=0;i<roomChoiceButtons.transform.childCount;i++){
            TextMeshProUGUI textField=roomChoiceButtons.transform.GetChild(i).gameObject.GetComponentInChildren<TextMeshProUGUI>();
            Color color = new Color32(255, 255, 255, 255);
            textField.color = color;
        }
    }
    // Update is called once per frame
    void Update()
    {
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
        if(Input.GetButtonDown("Interact")){
            checkForInteraction();
        }
    }
    private void checkForInteraction(){
        if(inRoom==true){
            QuestChoosingMenu.SetActive(!QuestChoosingMenu.activeSelf);
        }
    }
    private void OnTriggerEnter(Collider other)
    {   
        if (other.gameObject.CompareTag("FinishArea"))
        {
            FinishFunction();
        }else{
            inRoom=true;
            roomButton.SetActive(true);
            if(other.gameObject.name=="AltarRoom"){
                currentRoom = Rooms.AltarRoom;
            }else if(other.gameObject.name=="Orgelempore"){
                currentRoom = Rooms.Orgelempore;
            }else if(other.gameObject.name=="MainEntranceRoom"){
                currentRoom = Rooms.Entrance;
            }else if(other.gameObject.name=="SecondFloorRoom"){
                currentRoom = Rooms.QuietRoom;
            }
            Dictionary<string,string>textDictionary=quest3Dictionary[currentRoom];
            roomDescription.text=textDictionary["roomDescription"];
            roomName.text=textDictionary["roomName"];
            for(int i=0;i<roomChoiceButtons.transform.childCount;i++){
                TextMeshProUGUI textField=roomChoiceButtons.transform.GetChild(i).gameObject.GetComponentInChildren<TextMeshProUGUI>();
                textField.text=textDictionary["button"+i];
                switch(roomChoice[currentRoom]){
                    case RoomButtonClicked.First:
                        colorizeButtons(0);
                        break;
                    case RoomButtonClicked.Second:
                        colorizeButtons(1);
                        break;
                    case RoomButtonClicked.Third:
                        colorizeButtons(2);
                        break;
                    default:
                        colorizeButtons();
                        break;
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("FinishArea"))
        {
            FinishFunction();
        }else{
            roomButton.SetActive(false);
            inRoom=false;
        }
    }
    private void FinishFunction()
    {
        List<string> wronglyAssignedRooms = new List<string>();
        if(roomChoice[Rooms.AltarRoom]!=RoomButtonClicked.Third){
            wronglyAssignedRooms.Add("Altarraum");
        }
        if(roomChoice[Rooms.Entrance]!=RoomButtonClicked.First){
            wronglyAssignedRooms.Add("Eingangsbereich");
        }
        if(roomChoice[Rooms.Orgelempore]!=RoomButtonClicked.Second){
            wronglyAssignedRooms.Add("Orgelempore");
        }
        if(roomChoice[Rooms.QuietRoom]!=RoomButtonClicked.Third){
            wronglyAssignedRooms.Add("Nebenraum Orgelempore");
        }
        if(wronglyAssignedRooms.Count==0){
            //SceneManager.LoadScene("Quest4");
            mouseWriteCutscene.Play();
        }else{
            string output;
            if(wronglyAssignedRooms.Count==1){
                //output="Du hast leider nicht alle Räume korrekt zugewiesen. Folgender Raum konnte nicht korrekt zugewiesen werden: "+wronglyAssignedRooms[0];
                output="Folgendem Raum wurde leider die falsche Rolle zugewiesen: "+wronglyAssignedRooms[0];
                textField1.text = "3 / " + "4";
            }else{
                output="Folgenden Raeumen wurden leider die falsche Rolle zugewiesen: ";
                for(int i=0; i<wronglyAssignedRooms.Count-1; i++){
                    output+=wronglyAssignedRooms[i]+", "; 
                }
                textField1.text = (4-wronglyAssignedRooms.Count)+" / " + "4";
                output+=wronglyAssignedRooms[wronglyAssignedRooms.Count-1];
            }
            GameObject g = GameObject.Find("QuestMessagePrefab");
            QuestMessageScript endQuestMessageScript = g.GetComponent<QuestMessageScript>();
            endQuestMessageScript.ShowMessage(output);
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
    private Dictionary<string,string> getTextFieldDictionary(Rooms room){
        Dictionary<string,string> textFieldsDictionary=new Dictionary<string,string>();
        string roomDescription="";
        switch (room)
        {
            case Rooms.AltarRoom:
                textFieldsDictionary.Add("roomName","Altarraum");
                textFieldsDictionary.Add("button0","Buero");
                textFieldsDictionary.Add("button1","Garderobe");
                textFieldsDictionary.Add("button2","Spielbereich");
                roomDescription = "\nDu befindest dich gerade im alten Altarraum.\n\nDer Altarraum wird in der Planung des Architekten von dem restlichen Kirchenraum getrennt werden.\n\nSo entsteht ein grosser abgeschlossener Raum, der fuer vielerlei Sachen genutzt werden kann.\n\nWas soll in den frueheren Altarraum gebaut werden?";     
                textFieldsDictionary.Add("roomDescription",roomDescription);
                break;
            case Rooms.Entrance:
                textFieldsDictionary.Add("roomName","Eingang");
                textFieldsDictionary.Add("button0","Garderobe");
                textFieldsDictionary.Add("button1","Ruheraum");
                textFieldsDictionary.Add("button2","Buero");
                roomDescription = "\nDu befindest dich gerade im Haupteingangsbereich der Kirche.\n\nUeber diesen Eingang werden in Zukunft alle Kinder in den Kindergarten reinkommen. Es wird somit ein Durchgangsraum sein, wo haeufig viel los sein wird.\n\nWas soll deiner Meinung nach in diesen Raum reinkommen?";
                textFieldsDictionary.Add("roomDescription",roomDescription);
                break;
            case Rooms.Orgelempore:
                textFieldsDictionary.Add("roomName","Orgelempore");
                textFieldsDictionary.Add("button0","Ruheraum");
                textFieldsDictionary.Add("button1","Buero");
                textFieldsDictionary.Add("button2","Spielbereich");
                roomDescription = "\nDu befindest dich gerade auf der Orgelempore.\n\nVon hier oben kann man den gesamten Kirchenraum ueberblicken. Dennoch ist der Raum, durch seine Erhoehung raeumlich von den anderen Raeumlichkeiten getrennt.\n\nIm Plan des Architekten trennt zudem eine neue Wand den Raum von der Treppe.\n\nWas sollte am besten in den hier entstehenden Raum?";
                textFieldsDictionary.Add("roomDescription",roomDescription);
                break;
            case Rooms.QuietRoom:
                textFieldsDictionary.Add("roomName","Emporenraum");
                textFieldsDictionary.Add("button0","Spielbereich");
                textFieldsDictionary.Add("button1","Garderobe");
                textFieldsDictionary.Add("button2","Ruheraum");
                roomDescription = "\nDu befindest dich gerade im kleinen Raum neben der Orgelempore.\n\nEr ist raeumlich von den anderen Zimmern getrennt. Dadurch ist es hier verhaeltnissmaessig leise. Hier wuerde man wenig von der alltaeglichen Lautstaerke im Kindergarten mitbekommen.\n\nWas sollte in diesen Raum deiner Meinung nach kommen?";
                textFieldsDictionary.Add("roomDescription",roomDescription);
                break;
            default:
                break;
        }
        return textFieldsDictionary;
    }
    private enum Rooms {
        AltarRoom,
        Entrance,
        Orgelempore,
        QuietRoom
    }
    private enum RoomButtonClicked {
        None,
        First,
        Second,
        Third
    }
}
