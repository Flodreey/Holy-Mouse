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

    public int RegisterTether(Tether tether)
    {
        tethers.Add(tether);
        return tethers.Count - 1;
    }

    public List<Tether> GetTethers()
    {
        return tethers;
    }
}
