using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckZoneColor : MonoBehaviour
{
    public void ColorBlocks(int index)
    {
        Renderer[] a = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < a.Length; i++)
        {
            if(i == index)
            {
                a[i].material.color = Color.green;
            }
            else
            {
                a[i].material.color = Color.red;
            }
        }
    }
}
