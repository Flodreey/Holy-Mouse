using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugFeatures : MonoBehaviour
{
    [SerializeField] string[] scenes = {"Quest1", "Quest2", "Quest3", "Quest4" };
    static int currentScene = 0;

    void Start()
    {
        OnSceneChanged(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        SceneManager.sceneLoaded += OnSceneChanged;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            LoadScene(-1);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            LoadScene(0);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            LoadScene(1);
            return;
        }
    }

    void LoadScene(int offset)
    {
        currentScene += offset;
        currentScene = Mathf.Clamp(currentScene, 0, scenes.Length-1);
        SceneManager.LoadScene(scenes[currentScene]);
    }

    void OnSceneChanged(Scene scene, LoadSceneMode mode)
    {
        for (int i=0; i<scenes.Length; i++)
        {
            if (scenes[i].Equals(scene.name))
            {
                currentScene = i;
                return;
            }
        }
    }
}
