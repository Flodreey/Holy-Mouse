using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button resumeButtonActivated;
    [SerializeField] Button resumeButtonDeactivated;

    private void Update()
    {
        
        if (File.Exists("savedata.dat"))
        {
            resumeButtonActivated.gameObject.SetActive(true);
            resumeButtonDeactivated.gameObject.SetActive(false);
        }
        else
        {
            resumeButtonActivated.gameObject.SetActive(false);
            resumeButtonDeactivated.gameObject.SetActive(true);
        }
    }

    public void PlayGame(){
        if (File.Exists("savedata.dat"))
        {
            // Load the saved data from the file
            byte[] data = File.ReadAllBytes("savedata.dat");

            // Deserialize the GameState object from the byte array
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(data);
            GameState gameState = (GameState)formatter.Deserialize(stream);

            Options.loadMouseSensitivity(gameState.getMouseSensitivity());

            // Load the saved game state
            if(gameState.getLevel()<=4 && gameState.getLevel()>=1){
                SceneManager.LoadScene("Quest"+gameState.getLevel());
            }else{
                File.Delete("savedata.dat");
                SceneManager.LoadScene("Quest1");
            }
        }
    }
    public void NewGame(){
        File.Delete("savedata.dat");
        SceneManager.LoadScene("Cutscene");
    }
    public void QuitGame(){
        Debug.Log("QUIT!");
        File.Delete("savedata.dat");
        Application.Quit();
    }
}
