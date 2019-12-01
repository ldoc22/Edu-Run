using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Difficulty", menuName = "ScriptableObjects/Difficulty")]
public class Difficulty : ScriptableObject
{

    public enum DifficultLevel
    {
    Easy,
    medium,
    hard};
    public DifficultLevel ThisDifficultyLevel;
    public float BaseSpeed;


    public float MediumIncrease;
    public float HardIncrease;


    public float GetDifficulty(int currentSpeed, int questionNumber)
    {
        switch (ThisDifficultyLevel)
        {
            case DifficultLevel.Easy:
                return currentSpeed;
                break;
            case DifficultLevel.medium:

                return questionNumber * MediumIncrease + BaseSpeed;
                break;
            case DifficultLevel.hard:


                return Mathf.Pow(BaseSpeed, currentSpeed + HardIncrease);
                break;
        }
        return 0;

    }
}
