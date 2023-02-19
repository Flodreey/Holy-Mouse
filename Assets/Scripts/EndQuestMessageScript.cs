using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndQuestMessageScript : MonoBehaviour
{
    [SerializeField] GameObject messageDisplay;
    [SerializeField] int secondsToShowMessage = 5;


    // Start is called before the first frame update
    void Start()
    {
        messageDisplay.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowMessage()
    {
        StartCoroutine(ShowMessageForSeconds(secondsToShowMessage));
    }

    IEnumerator ShowMessageForSeconds(int seconds)
    {
        messageDisplay.SetActive(true);

        yield return new WaitForSeconds(seconds);

        messageDisplay.SetActive(false);
    }
}
