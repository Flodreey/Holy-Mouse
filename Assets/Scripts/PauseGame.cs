using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class PauseGame : MonoBehaviour
{
    public void ReturnToMainMenu(){
        SceneManager.LoadScene("MainMenu");
    }

    public void StopTime()
    {
        Time.timeScale = 0;
    }

    public void ContinueTime()
    {
        Time.timeScale = 1;
    }
}
