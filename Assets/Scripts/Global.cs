using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global : MonoBehaviour
{
    public static Global instance;

    private static List<Tether> tethers;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this);
        } else
        {
            instance = this;
            tethers = new List<Tether>();
        }
    }

    public void RegisterTether(Tether tether)
    {
        tethers.Add(tether);
    }

    public List<Tether> GetTethers()
    {
        return tethers;
    }
}
