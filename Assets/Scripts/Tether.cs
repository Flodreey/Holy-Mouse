using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tether : MonoBehaviour
{
    private LineRenderer line;
    private Vector3[] points;
    
    void Start()
    {
        line = GetComponent<LineRenderer>();
        points = new Vector3[line.positionCount];
        line.GetPositions(points);
        
        Global.instance.RegisterTether(this);
    }

    /*
     * ASSUMPTION: line is mostly straight, does not curl up to the point where the closest two line segments are disjoint
     */
    public Vector3 GetClosestPoint(Vector3 pos)
    {
        pos -= transform.position;
        Vector3 output = pos;
        float closestDist = Mathf.Infinity;
        for(int i=1; i<points.Length; i++)
        {
            Vector3 A = points[i];
            Vector3 B = points[i - 1];

            // project pos onto this line segment
            Vector3 AB = (B - A);
            float scalar = (Vector3.Dot((pos - A), AB) / Vector3.Dot(AB, AB));
            scalar = Mathf.Max(0f, Mathf.Min(1f, scalar));

            Vector3 projectedPoint = A + scalar * AB;

            float dist = Vector3.Distance(pos, projectedPoint);
            if (dist < closestDist)
            {
                output = projectedPoint;
                closestDist = dist;
            }
        }
        
        return output+transform.position;
    }

}
