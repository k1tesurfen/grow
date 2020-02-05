using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplorationManager : MonoBehaviour
{
    public GameManager gm;

    //Points the player made druing condition
    public int points;

    public void BeginExploration()
    {
        points = 0;
    }

    public void EndExploration()
    {
        gm.qm.StartQuestionnaireMode(); 
    }

    //Adds points the player made
    private void AddPoints(int points)
    {
        this.points += points;
    }
}
