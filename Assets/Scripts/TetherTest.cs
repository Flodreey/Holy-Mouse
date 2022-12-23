using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherTest : MonoBehaviour
{
    public Tether tether;
    public GameObject projectedPoint;

    void Update()
    {
        Vector3 dir;
        projectedPoint.transform.position = tether.GetClosestPoint(transform.position, out dir);
    }
}
