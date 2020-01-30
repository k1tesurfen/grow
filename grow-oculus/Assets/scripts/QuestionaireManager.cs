using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestionaireManager : MonoBehaviour
{
    public GameManager gm;

    public Questionaire[] questionaires;
    public int questionaireToDo = 0;

    public TextMeshPro questionLabel;
    public GameObject spawnPlatform;
    public GameObject undoButton;

    public bool questionnaireMode = false;

    public void StartQuestionnaireMode()
    {
        gm.fm.HideFortress();
        gm.pm.ClearProjectiles();
        questionLabel.gameObject.SetActive(true);
        spawnPlatform.SetActive(true);
        undoButton.SetActive(true);
        StartNextQuestionaire();
        questionnaireMode = true;
    }

    public void RedoRecentQuestion()
    {
        questionaires[questionaireToDo-1].RedoLastQuestion();
    }

    public void StartNextQuestionaire()
    {
        if(questionaireToDo == questionaires.Length)
        {
            //abort questionaire mode and continue with conditions and stuff
            gm.fm.ShowFortress();
            questionLabel.gameObject.SetActive(false);
            spawnPlatform.SetActive(false);
            undoButton.SetActive(false);
            questionnaireMode = false;
            return;
        }

        questionaires[questionaireToDo].InitQuestionaire();
        questionaireToDo++;
    }
}
