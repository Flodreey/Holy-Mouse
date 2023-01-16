using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressing : MonoBehaviour
{
    [SerializeField] GameObject questPanel;
    private bool isActive;
    
    // Start is called before the first frame update
    void Start()
    {
        isActive=false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Tab"))
        {
            Debug.Log("Tab button got pressed");
            if(isActive){
                questPanel.SetActive(false);
                isActive=false;
            }else{
                questPanel.SetActive(true);
                isActive=true;
            }
        }
    }
}
