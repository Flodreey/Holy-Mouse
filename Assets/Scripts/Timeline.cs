using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class Timeline : MonoBehaviour
{
    [Tooltip("After this amount of time in the timeline the scene will switch")]
    [SerializeField] double timeToSwitchScene = 105;
    [SerializeField] GameObject pauseMenu;

    private PlayableDirector playableDirector;
    
    private bool timelinePaused = false;

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
            SceneManager.LoadScene("Quest1");
        }

        if (Input.GetButtonDown("Cancel"))
        {
            timelinePaused = !timelinePaused;

            if (timelinePaused)
            {
                PauseTimeline();
                pauseMenu.SetActive(true);
            }
            else
            {
                ResumeTimeline();
                pauseMenu.SetActive(false);
            }
        }
    }

    public void PauseTimeline()
    {
        timelinePaused = true;
        Time.timeScale = 0;
    }

    public void ResumeTimeline()
    {
        timelinePaused = false;
        Time.timeScale = 1;
    }

    public void RestartTimeline()
    {
        playableDirector.time = 0;
    }

    public void SkipTimeline()
    {
        playableDirector.time = 101;
    }
}
