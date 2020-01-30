﻿using System.Collections;
using UnityEngine;

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

    public float gapWidth;

    public int currentQuestion;
    public QOption[] currentQuestionOptions;

    public int selection;

    public bool answerLock;

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

        qm.questionLabel.text = q.questionText;
        currentQuestionOptions = new QOption[q.answerOptionsLabels.Length];


        float startPos = ((q.answerOptionsLabels.Length - 1f) / 2f) * -gapWidth;

        for (int i = 0; i < q.answerOptionsLabels.Length; i++)
        {
            QOption option = Instantiate(qOption, optionHolder.transform);
            option.optionLabel.text = q.answerOptionsLabels[i].Replace(";", "\n");
            option.value = i;
            option.SetQuestionaire(this);
            if (q.answerOptionsImages.Length > 0)
            {
                option.optionImage.sprite = q.answerOptionsImages[i];
            }
            Vector3 position = new Vector3((startPos + (i * gapWidth)), 1.2f, 0.6f);
            option.transform.position = position;
        }
    }

    //if a selection has been made this selection will update the value
    public void SetSelection(int value, GameObject activeBucket, GameObject activeSnowball)
    {
        selection = value;

        //set selectionindicator
        activeBucket.GetComponent<QOption>().SetActiveMaterial();

        StartCoroutine(JanDelay(0.8f, activeBucket, activeSnowball));
    }

    IEnumerator JanDelay(float time, GameObject activeBucket, GameObject activeSnowball)
    {
        yield return new WaitForSeconds(time);
        SetAnswer(selection, activeBucket, activeSnowball);
    }

    //the current selection is saved as answer of the question
    //the currentQuestion is raised.
    public void SetAnswer(int value, GameObject activeBucket, GameObject activeSnowball)
    {
        //remove/destroy indicators
        activeSnowball.GetComponent<Snowball>().DestroySnowball();

        answers[currentQuestion] = value;

        currentQuestion++;
        answerLock = false;

        if (transform.Find("options") != null)
        {
            Destroy(transform.Find("options").gameObject);
        }
        if (currentQuestion >= questions.Length)
        {
            //all questions for this questionaire have been answered.
            //save questionaire to file

            qm.questionLabel.text = "";
            SaveQuestionaire(answers);
            qm.StartNextQuestionaire();
            //gameObject.SetActive(false);
        }
        else
        {
            AskQuestion(currentQuestion);
        }
    }

    public void RedoLastQuestion()
    {
        if (transform.Find("options") != null)
        {
            Destroy(transform.Find("options").gameObject);
        }
        if (currentQuestion > 0)
        {
            currentQuestion--;
            AskQuestion(currentQuestion);
        }
        else if(currentQuestion == 0)
        {
            AskQuestion(currentQuestion);
        }
    }

    public void SaveQuestionaire(int[] answers)
    {
        Debug.Log("saving the questionaire");
    }
}
