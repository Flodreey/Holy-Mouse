using System;

[Serializable]
public class GameState
{
    private int level;

    public GameState(int level)
    {
        this.level = level;
    }
    public int getLevel(){
        return level;
    }
    public void setLevel(int level){
        this.level=level;
    }
}