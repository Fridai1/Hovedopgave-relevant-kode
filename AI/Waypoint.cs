using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Waypoint
{
    // Start is called before the first frame update
    public float waittime;
    public GameObject waypoint;
    [NonSerialized]
    public Vector3 location;

    void Start()
    {
        location = waypoint.transform.position;
    }
    
}
