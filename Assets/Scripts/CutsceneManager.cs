using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] Sprite[] frames;
    [SerializeField] float timePerFrame = 3f;
    [SerializeField] float fadeSpeed = 4f;
    [SerializeField] string nextScene = "Quest2";
    Image display;
    static CutsceneManager instance;
    
    void Start()
    {
        display = GetComponentInChildren<Image>();
        display.color = new Color(0,0,0,0);
        instance = this;
    }

    public static void StartCutscene(bool fadeout = true)
    {
        instance.Begin(fadeout);
    }

    void Begin(bool fadeout)
    {
        if (frames.Length == 0) return;
        display.gameObject.GetComponentInParent<Canvas>().sortingOrder = 7;
        display.gameObject.SetActive(true);
        if (fadeout)
        {
            StartCoroutine(PrepareCutscene());
        } else
        {
            StartCoroutine(ShowFrame(0));
        }
    }

    IEnumerator PrepareCutscene()
    {
        //Fade out
        while (true)
        {
            display.color = Color.Lerp(display.color, new Color(0, 0, 0, 1), Time.deltaTime * (fadeSpeed*2));
            if (display.color.a >= .9f)
            {
                StartCoroutine(ShowFrame(0));
                break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    IEnumerator ShowFrame(int index)
    {
        display.color = new Color(0,0,0,1);
        display.sprite = frames[index];
        float time = 0f;

        //Fade in
        while(true){
            time += Time.deltaTime;
            display.color = Color.Lerp(display.color, new Color(1, 1, 1, 1), Time.deltaTime*fadeSpeed);
            if(time >= timePerFrame)
            {
                break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }

        //Fade out
        while (true)
        {
            display.color = Color.Lerp(display.color, new Color(0, 0, 0, 1), Time.deltaTime * fadeSpeed);
            if (display.color.r <= .1f)
            {
                break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }

        if(index < frames.Length - 1)
        {
            StartCoroutine(ShowFrame(index + 1));
        } else
        {
            SceneManager.LoadScene(nextScene);
        }
    }
    
    // Debug input
    /*
    void Update()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            StartCutscene();
        }
    }
    */
}
