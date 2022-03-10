using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public static Transform[] Positions;
    void Awake()
    {
        Positions = new Transform[transform.childCount];
        for (int i = 0; i < Positions.Length; i++)
        {
            Positions[i] = transform.GetChild(i);
        }
    }
}
