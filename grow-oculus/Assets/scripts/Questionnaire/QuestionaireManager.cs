using System.Collections;
using UnityEngine;

public class QuestionaireManager : MonoBehaviour
{
    public GameManager gm;

    public Questionaire[] questionnaires;
    public int questionnaireToDo = 0;

    public GameObject spawnPlatform;
    public GameObject undoButton;

    public bool questionnaireMode = false;

    private bool isFirstTimeQuestionnaire = true;

    //prepare everything for the first questionnaire and start it.
    public void StartQuestionnaireMode()
    {
        gm.fm.HideFortress();
        spawnPlatform.GetComponent<Spawner>().ClearProjectiles();
        questionnaireToDo = 0;
        questionnaireMode = true;
        StartCoroutine(JanDelay(1f, false));
    }


    //abort questionaire mode and continue with conditions and stuff
    public void EndQuestionnaireMode()
    {
        gm.pm.ClearProjectiles();
        gm.fm.ShowFortress();
        spawnPlatform.SetActive(false);
        spawnPlatform.GetComponent<Spawner>().HideProjectiles();
        undoButton.transform.parent.gameObject.SetActive(false);
        StartCoroutine(JanDelay(1f, true));
    }

    public void AbortQuestionnaireMode()
    {
        questionnaires[questionnaireToDo - 1].ClearQuestion();
        questionnaireToDo = 0;
        StartNextQuestionaire();
    }

    public void RedoRecentQuestion()
    {
        questionnaires[questionnaireToDo - 1].RedoLastQuestion();
    }

    public void StartNextQuestionaire()
    {
        if (questionnaireToDo == questionnaires.Length)
        {
            EndQuestionnaireMode();
            return;
        }

        questionnaires[questionnaireToDo].InitQuestionaire();
        questionnaireToDo++;
    }

    IEnumerator JanDelay(float time, bool isEnd)
    {
        yield return new WaitForSeconds(time);
        if (!isEnd)
        {
            if (isFirstTimeQuestionnaire)
            {
                gm.audioManager.AddToQueue("questionnaire");
                isFirstTimeQuestionnaire = false;
            }
            spawnPlatform.SetActive(true);
            spawnPlatform.GetComponent<Spawner>().SpawnProjectile();
            undoButton.transform.parent.gameObject.SetActive(true);
            StartCoroutine(SamyDeluxe(gm.audioManager.waitingTime));
        }
        else
        {
            questionnaireMode = false;
            gm.NextCondition();
        }
    }

    public void MakeUndubuttonSwitchable()
    {
        undoButton.GetComponent<UndoButton>().isSwitchable = true;
    }

    IEnumerator SamyDeluxe(float time)
    {
        yield return new WaitForSeconds(time);
        MakeUndubuttonSwitchable();
        StartNextQuestionaire();
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, new Vector3(0.1f, 0.1f, 0.1f));
    }
}
