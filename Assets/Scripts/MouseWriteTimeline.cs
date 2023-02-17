using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MouseWriteTimeline : MonoBehaviour
{
    PlayableDirector director;

    void Start()
    {
        director = GetComponent<PlayableDirector>();
        director.stopped += onEndReached;
    }

    void onEndReached(PlayableDirector d)
    {
        CutsceneManager.StartCutscene();
    }
}
