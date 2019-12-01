using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCheckZone : MonoBehaviour
{
    public void Activate(int index) {
        GameObject[] childs = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            childs[i] = transform.GetChild(i).gameObject;
        }
       
        for (int i = 0; i < childs.Length; i++)
        {
           
            if(i == index)
            {
                childs[i].GetComponent<Renderer>().material.color = Color.green;
            }
            else
            {
                childs[i].GetComponent<Renderer>().material.color = Color.red;
                childs[i].AddComponent<Rigidbody>();

            }
        }
    
    }

    public void Deactivate()
    {
        Renderer[] rend = GetComponentsInChildren<Renderer>();
        foreach(Renderer r in rend)
        {
            r.material.color = Color.gray;
        }
    }

}
