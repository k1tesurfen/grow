using System.Collections;
using UnityEngine;
using TMPro;

public class Questionaire : MonoBehaviour
{
    public QuestionaireManager qm;
    public TextMeshPro questionLabel;
    public string ID;
    //A questionaire holds many questions. A Question is a scriptable object,
    //that holds the requires question text and the answer options.
    public Question[] questions;
    private int[] answers;

    [HideInInspector]
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
        answers = new int[questions.Length-1];
        currentQuestion = 0;
        questionLabel.gameObject.SetActive(true);
        AskQuestion(currentQuestion);
    }

    public void AskQuestion(int n)
    {
        optionHolder = new GameObject("options");
        optionHolder.transform.parent = transform;
        optionHolder.transform.localPosition = Vector3.zero;

        Question q = questions[n];

        questionLabel.text = q.questionText;
        currentQuestionOptions = new QOption[q.answerOptionsLabels.Length];


        float startPos = ((q.answerOptionsLabels.Length - 1f) / 2f) * -gapWidth;

        for (int i = 0; i < q.answerOptionsLabels.Length; i++)
        {
            QOption option = Instantiate(qOption, optionHolder.transform);
            option.optionLabel.text = q.answerOptionsLabels[i].Replace(";", "\n");
            option.value = i+1;
            option.SetQuestionaire(this);
            if (q.answerOptionsImages.Length > 0)
            {
                option.optionImage.sprite = q.answerOptionsImages[i];
            }
            Vector3 position = new Vector3((startPos + (i * gapWidth)), 0f, 0f);
            option.transform.localPosition = position;
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

    //adds a delay for the answer to get finally accepted.
    IEnumerator JanDelay(float time, GameObject activeBucket, GameObject activeSnowball)
    {
        yield return new WaitForSeconds(time);
        SetAnswer(selection, activeBucket, activeSnowball);
    }

    //the current selection is saved as answer of the question
    //the currentQuestion is raised.
    public void SetAnswer(int value, GameObject activeBucket, GameObject activeSnowball)
    {
        try
        {
            //remove/destroy indicators
            activeSnowball.gameObject.SetActive(false);
        }
        catch
        {

        }
        if(currentQuestion < (questions.Length-1))
        {
            answers[currentQuestion] = value;
        }

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

            questionLabel.text = "";
            questionLabel.gameObject.SetActive(false);
            SaveQuestionaire(answers);
            qm.StartNextQuestionaire();
        }
        else
        {
            AskQuestion(currentQuestion);
        }
    }

    public void ClearQuestion()
    {
        if (transform.Find("options") != null)
        {
            Destroy(transform.Find("options").gameObject);
        }
        questionLabel.text = "";
    }

    //removes options of current question and asks the previous question, if there is one.
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
        else if (currentQuestion == 0)
        {
            AskQuestion(currentQuestion);
        }
    }

    //save all answers to this questtionnaire
    public void SaveQuestionaire(int[] answers)
    {
        qm.gm.answers.AddRange(answers);
    }

}
