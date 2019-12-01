using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Answer
{
    private  string context;
    private  bool iscorrect;

    public  Answer(string c, bool correct)
    {
        context = c;
        iscorrect = correct;
    }

    public  string getContext()
    {
        return context;
    }

    public  bool isCorrect()
    {
        return iscorrect;
    }


}
