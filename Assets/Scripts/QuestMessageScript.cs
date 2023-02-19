using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndQuestMessageScript : MonoBehaviour
{
    [SerializeField] GameObject display;
    [SerializeField] TextMeshProUGUI textField;
    [SerializeField] int secondsToShowMessage = 5;

    string defaultText = "Super! Du hast die Quest erfolgreich gemeistert. Kehre jetzt zu deinem Versteck zurueck, um in die naechste Quest zu gelangen.";


    // Start is called before the first frame update
    void Start()
    {
        display.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowMessage()
    {
        ShowMessage(defaultText);
    }

    public void ShowMessage(string text)
    {
        textField.text = text;
        StartCoroutine(ShowMessageForSeconds(secondsToShowMessage));
    }

    IEnumerator ShowMessageForSeconds(int seconds)
    {
        display.SetActive(true);

        yield return new WaitForSeconds(seconds);

        display.SetActive(false);
    }
}
