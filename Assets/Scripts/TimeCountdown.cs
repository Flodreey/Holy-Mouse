using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TimeCountdown : MonoBehaviour
{
    private TextMeshProUGUI textField;
    [SerializeField] private float initialTime = 300;
    private float time;

    // Start is called before the first frame update
    void Start()
    {
        textField = GetComponent<TextMeshProUGUI>();
        time = initialTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
        }
        else
        {
            time = 0;
            OnCountdownExpired();
        }

        DisplayTime();
    }

    void DisplayTime()
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        if (seconds < 10)
            textField.text = minutes + ":0" + seconds;
        else
            textField.text = minutes + ":" + seconds;
    }

    // TODO
    void OnCountdownExpired()
    {

    }
}
