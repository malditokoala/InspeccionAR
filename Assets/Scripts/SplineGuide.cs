using UnityEngine;
using System.Collections.Generic;


[RequireComponent(typeof(LineRenderer))]
public class SplineGuide : MonoBehaviour
{
    public Transform waypointsRoot;        // padre con los W0..Wn
    [Range(2, 256)] public int samples = 64;
    public bool closed = false;
    public float yOffset = 0.0f;

    LineRenderer lr;
    List<Transform> wp = new List<Transform>();

    public void Build()
    {
        // Leer waypoints
        wp.Clear();
        if (!waypointsRoot) { lr.positionCount = 0; return; }
        foreach (Transform t in waypointsRoot) wp.Add(t);
        if (wp.Count < 2) { lr.positionCount = 0; return; }

        // Catmull-Rom simple
        lr.positionCount = samples + 1;
        for (int i = 0; i <= samples; i++)
        {
            float t = (float)i / samples;
            Vector3 p = GetPoint(t) + new Vector3(0, yOffset, 0);
            lr.SetPosition(i, p);
        }
    }

    Vector3 GetPoint(float t)
    {
        int n = wp.Count;
        float ft = t * (closed ? n : n - 1);
        int i0 = Mathf.FloorToInt(ft);
        float lt = ft - i0;

        int i1 = (i0 + 1) % n;
        int im1 = (i0 - 1 + n) % n;
        int i2 = (i0 + 2) % n;

        if (!closed)
        {
            im1 = Mathf.Clamp(i0 - 1, 0, n - 1);
            i1 = Mathf.Clamp(i0 + 1, 0, n - 1);
            i2 = Mathf.Clamp(i0 + 2, 0, n - 1);
        }

        Vector3 P0 = wp[im1].position;
        Vector3 P1 = wp[i0].position;
        Vector3 P2 = wp[i1].position;
        Vector3 P3 = wp[i2].position;

        // Catmull-Rom
        return 0.5f * ((2f * P1) +
            (-P0 + P2) * lt +
            (2f * P0 - 5f * P1 + 4f * P2 - P3) * lt * lt +
            (-P0 + 3f * P1 - 3f * P2 + P3) * lt * lt * lt);
    }

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.useWorldSpace = true;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
