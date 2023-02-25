using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tether : MonoBehaviour
{
    // Whether the player should drop off the tether automatically once they reach the start (point 0) or end (last point)
    [SerializeField] bool dropAtStart = false;
    [SerializeField] bool dropAtEnd = false;
    public Vector3 mouseDown = new Vector3( 0, 0, 1 );
    public int id;

    private LineRenderer line;
    private Vector3[] points;
    private Vector3 start, end;
    private float dropDistance = .05f;
    
    void Start()
    {
        line = GetComponent<LineRenderer>();
        points = new Vector3[line.positionCount];
        line.GetPositions(points);
        start = transform.position + points[0];
        end = transform.position + points[points.Length - 1];
        
        id = Global.instance.RegisterTether(this);
    }

    /*
     * ASSUMPTION: line is mostly straight, does not curl up to the point where the closest two line segments are disjoint
     */
    public Vector3 GetClosestPoint(Vector3 pos, out Vector3 segmentDirection)
    {
        pos -= transform.position;
        Vector3 output = pos;
        segmentDirection = new Vector3();
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
                segmentDirection = AB.normalized;
            }
        }
        
        return output+transform.position;
    }

    public bool shouldDrop(Vector3 pos, Vector3 dir)
    {
        bool reachedStart = Vector3.Distance(pos, start) < dropDistance;
        bool movingTowardsStart = Vector3.Dot(start - pos, dir) > 0;
        bool reachedEnd = Vector3.Distance(pos, end) < dropDistance;
        bool movingTowardsEnd = Vector3.Dot(end - pos, dir) > 0;

        return (dropAtStart && reachedStart && movingTowardsStart) || (dropAtEnd && reachedEnd && movingTowardsEnd);
    }

}
