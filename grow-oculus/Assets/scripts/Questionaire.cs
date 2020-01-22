using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Questionaire : MonoBehaviour
{
    public QuestionaireManager qm;
    public string ID;
    //A questionaire holds many questions. A Question is a scriptable object,
    //that holds the requires question text and the answer options.
    public Question[] questions;
    private int[] answers;

    public GameObject optionHolder;

    public QOption qOption;

    public int currentQuestion;
    public TextMeshPro currentQuestionLabel;

    public void InitQuestionaire()
    {
        //for each question, there has to be one answer
        answers = new int[questions.Length];
        currentQuestion = 0;
        AskQuestion(currentQuestion); 
    }

    public void AskQuestion(int n)
    {
        optionHolder = new GameObject("options");
        optionHolder.transform.parent = transform;

        Question q = questions[n];

        currentQuestionLabel.text = q.questionText;

        //answer buckets get distributed on the hemisphere in front of the player
        float startAngle = ((q.answerOptionsLabels.Length - 1f) / 2f) * -25f;

        for(int i=0; i<q.answerOptionsLabels.Length; i++)
        {
            QOption option = Instantiate(qOption, optionHolder.transform);
            option.optionLabel.text = q.answerOptionsLabels[i];
            option.value = i;
            option.SetQuestionaire(this);
            if(q.answerOptionsImages.Length > 0)
            {
                option.optionImage.sprite = q.answerOptionsImages[i];
            }
            Quaternion rotation = Quaternion.Euler(new Vector3(0f, startAngle + i * 25, 0f));
            option.transform.rotation = rotation;
            option.transform.position = (rotation * (1.5f *Vector3.forward)) + 0.7f * Vector3.up;
        }
    }

    //sets answer for current question.
    public void SetAnswer(int value)
    {
        answers[currentQuestion] = value;
        currentQuestion++;
        if(transform.Find("options") != null)
        {
            Destroy(transform.Find("options").gameObject);
        }
        if(currentQuestion >= questions.Length)
        {
            //all questions for this questionaire have been answered.
            //save questionaire to file

            currentQuestionLabel.text = "";
            SaveQuestionaire(answers);
            qm.StartNextQuestionaire();
            //gameObject.SetActive(false);
        }
        else
        {
            AskQuestion(currentQuestion);
        }
    }

    public void SaveQuestionaire(int[] answers)
    {
        Debug.Log("saving the questionaire");
    }
}
