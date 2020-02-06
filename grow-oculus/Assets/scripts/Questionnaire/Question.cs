using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Question : ScriptableObject
{
    [TextArea(4, 10)]
    public string questionText;
    public string[] answerOptionsLabels;
    public Sprite[] answerOptionsImages;
}
