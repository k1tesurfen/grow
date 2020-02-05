using TMPro;
using UnityEngine;

public class QuestionaireManager : MonoBehaviour
{
    public GameManager gm;

    public Questionaire[] questionaires;
    public int questionaireToDo = 0;

    public TextMeshPro questionLabel;
    public GameObject spawnPlatform;
    public GameObject undoButton;

    public bool questionnaireMode = false;

    //prepare everything for the first questionnaire and start it.
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


    //abort questionaire mode and continue with conditions and stuff
    public void EndQuestionnaireMode()
    {
        gm.fm.ShowFortress();
        questionLabel.gameObject.SetActive(false);
        spawnPlatform.SetActive(false);
        undoButton.SetActive(false);
        questionnaireMode = false;
        gm.NextCondition();
    }

    public void RedoRecentQuestion()
    {
        questionaires[questionaireToDo - 1].RedoLastQuestion();
    }

    public void StartNextQuestionaire()
    {
        if (questionaireToDo == questionaires.Length)
        {
            EndQuestionnaireMode();
            return;
        }

        questionaires[questionaireToDo].InitQuestionaire();
        questionaireToDo++;
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, new Vector3(0.1f, 0.1f, 0.1f)); 
    }
}
