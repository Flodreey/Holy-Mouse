using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class MainMenu : MonoBehaviour
{
    public void PlayGame(){
        if (File.Exists("savedata.dat"))
        {
            // Load the saved data from the file
            byte[] data = File.ReadAllBytes("savedata.dat");

            // Deserialize the GameState object from the byte array
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream(data);
            GameState gameState = (GameState)formatter.Deserialize(stream);

            // Load the saved game state
            if(gameState.getLevel()<=3&&gameState.getLevel()>=1){
                SceneManager.LoadScene("Quest"+gameState.getLevel());
            }else{
                File.Delete("savedata.dat");
                SceneManager.LoadScene("Quest1");
            }
        }
        else
        {
            // Start a new game
            SceneManager.LoadScene("Quest1");
        }
    }
    public void QuitGame(){
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
