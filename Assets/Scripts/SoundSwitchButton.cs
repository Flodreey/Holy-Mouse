using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSwitchButton : MonoBehaviour
{
    [SerializeField] Sprite soundActivated;
    [SerializeField] Sprite soundActivatedPressed;
    [SerializeField] Sprite soundDeactivated;
    [SerializeField] Sprite soundDeactivatedPressed;

    bool sound = true;
    Button button;
    Image buttonImage;

    // Start is called before the first frame update
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(TaskOnClick);

        buttonImage = GetComponent<Image>();
    }

    void TaskOnClick()
    {
        sound = !sound;

        if (sound)
        {
            buttonImage.sprite = soundActivated;
            BGMusic.instance.GetComponent<AudioSource>().Play();
        }
        else
        {
            buttonImage.sprite = soundDeactivated;
            BGMusic.instance.GetComponent<AudioSource>().Pause();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (BGMusic.instance.GetComponent<AudioSource>().isPlaying)
        {
            buttonImage.sprite = soundActivated;
            sound = true;
        }
        else
        {
            buttonImage.sprite = soundDeactivated;
            sound = false;
        }
    }
}
