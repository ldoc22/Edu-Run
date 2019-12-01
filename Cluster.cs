using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cluster 
{

    private GameObject[] rows;
    private int currentIndex;

    public Cluster()
    {
        rows = new GameObject[3];
        currentIndex = 0;
    }

    public void Add(GameObject obj)
    {
        rows[currentIndex] = obj;
        currentIndex++;
    }

    public GameObject [] GetRows()
    {
        return rows;
    }
    public float GetZPos()
    {
       return rows[0].transform.position.z;
    }

}
