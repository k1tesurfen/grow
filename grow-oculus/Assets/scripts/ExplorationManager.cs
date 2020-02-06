using System.Collections;
using UnityEngine;

public class ExplorationManager : MonoBehaviour
{
    public GameManager gm;
    public GameObject target;

    //Points the player made druing condition
    public int points;

    public void StartExploration(InteractionMethod im)
    {
        points = 0;
        gm.pm.Repopulate();
        gm.target.gameObject.SetActive(true);
        StartCoroutine(Countdown(gm.timeInScenario));
    }

    public void EndExploration()
    {
        gm.pm.ClearProjectiles();
        gm.target.gameObject.SetActive(false);
        gm.qm.StartQuestionnaireMode(); 
    }

    //Adds points the player made
    private void AddPoints(int points)
    {
        this.points += points;
    }

    //coroutine to countdown the remaining seconds
    private IEnumerator Countdown(int time)
    {
        while (time > 0)
        {
            time--;
            yield return new WaitForSeconds(1);
        }
        EndExploration();
    }
}
