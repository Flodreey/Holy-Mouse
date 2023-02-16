using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    [SerializeField] Slider mouseSensitivity;
    static Options instance;

    private void Start()
    {
        instance = this;
        mouseSensitivity.value = GameState.getGlobalMouseSensitivity();
    }

    public void setMouseSensitivity(Slider slider)
    {
        GameState.setMouseSensitivity(slider.value);
        CameraController.SetMouseSensitivity(slider.value);
    }

    public static void loadMouseSensitivity(float value)
    {
        CameraController.SetMouseSensitivity(value);
        instance.mouseSensitivity.value = value;
    }

    public static float getMouseSensitivity()
    {
        if (instance == null) return 0.5f;
        return instance.mouseSensitivity.value;
    }
}
