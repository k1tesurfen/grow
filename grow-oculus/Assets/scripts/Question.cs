using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Question : ScriptableObject
{
    public string questionText;
    public string[] answerOptionsLabels;
    public Sprite[] answerOptionsImages;
}
