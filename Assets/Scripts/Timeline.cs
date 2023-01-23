using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class Timeline : MonoBehaviour
{
    [Tooltip("After this amount of time in the timeline the scene will switch")]
    [SerializeField]
    double timeToSwitchScene = 105;

    private PlayableDirector playableDirector;
    // Start is called before the first frame update
    void Start()
    {
        playableDirector = GetComponent<PlayableDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playableDirector.time >= timeToSwitchScene)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
