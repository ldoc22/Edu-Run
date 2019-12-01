using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateQuestion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space Pressed");
            Question q = ScriptableObject.CreateInstance(typeof(Question)) as Question;
            string []  answers = {"answer1", "answer2", "answer3" };
            q.Create("test", 10f, answers, 1);
        }
    }
}
