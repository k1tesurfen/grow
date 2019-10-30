using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionaireManager : MonoBehaviour
{
    public GameManager gm;

    public Questionaire[] questionaires;
    public int questionaireToDo = 0;

    public bool questionaireMode = false;

    public void StartNextQuestionaire()
    {
        if(questionaireToDo == questionaires.Length)
        {
            //abort questionaire mode and continue with cool conditions and stuff
            gm.fm.ShowFortress();
            return;
        }

        questionaires[questionaireToDo].InitQuestionaire();
        questionaireToDo++;
    }
}
