using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private float currentTime;
    [SerializeField]
    private bool started;
    [SerializeField]
    private float targetTime;

    private GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0;
        gm = GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (started)
        {
            currentTime += Time.deltaTime;
            if(currentTime >= targetTime)
            {
                started = false;
                gm.TimesUp();
                
                
            }
        }
        
    }
    

    public float getTime()
    {
        return currentTime;
    }
    public void setTargetTime(float tt)
    {
        targetTime = tt;
    }
    public void StartTime()
    {
        currentTime = 0;
        started = true;
        
    }

}
