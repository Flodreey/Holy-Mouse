using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class GameState
{
    private int level;
    private float mouseSensitivity = 0.5f;
    static float globalMouseSensitivity = 0.5f;
    static GameState instance;

    public GameState(int levelID)
    {
        level = levelID;
        mouseSensitivity = globalMouseSensitivity;
        instance = this;
    }
    public int getLevel(){
        return this.level;
    }
    public void setLevel(int level){
        this.level=level;
    }
    public static void setMouseSensitivity(float value)
    {
        globalMouseSensitivity = value;
        if (instance == null) return;
        instance.mouseSensitivity = value;
    }
    public float getMouseSensitivity()
    {
        return mouseSensitivity;
    }
    public static float getGlobalMouseSensitivity()
    {
        return globalMouseSensitivity;
    }
}