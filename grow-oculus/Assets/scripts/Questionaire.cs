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

    public float gapAngle = 20f;

    public int currentQuestion;
    public QOption[] currentQuestionOptions;
    
    public int selection;
    private GameObject activeBucket;
    private GameObject activeSnowball;

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

        //answer buckets get distributed on the hemisphere in front of the player
        float startAngle = ((q.answerOptionsLabels.Length - 1f) / 2f) * -gapAngle;

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
            Quaternion rotation = Quaternion.Euler(new Vector3(0f, startAngle + i * gapAngle, 0f));
            option.transform.rotation = rotation;
            option.transform.position = (rotation * (0.6f * Vector3.forward)) + 1.1f * Vector3.up;
        }
    }

    //if a selection has been made this selection will update the value
    public void SetSelection(int value, GameObject activeBucket, GameObject activeSnowball)
    {
        selection = value;

        //remove all current selectionindicators
        if(this.activeBucket != null)
        {
            this.activeBucket.GetComponent<QOption>().SetDefaultMaterial();
        }
        if (this.activeSnowball)
        {
            Destroy(this.activeSnowball);
        }

        //set selectionindicator
        activeBucket.GetComponent<QOption>().SetActiveMaterial();

        Destroy(activeSnowball.GetComponent<OVRGrabbable>());

        //save current selectionindicators
        this.activeSnowball = activeSnowball;
        this.activeBucket = activeBucket;
    }

    //the current selection is saved as answer of the question
    //the currentQuestion is raised.
    public void SetAnswer()
    {
        answers[currentQuestion] = selection;

        currentQuestion++;

        if(transform.Find("options") != null)
        {
            Destroy(transform.Find("options").gameObject);
        }
        if(currentQuestion >= questions.Length)
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

    public void SaveQuestionaire(int[] answers)
    {
        Debug.Log("saving the questionaire");
    }
}
