using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class CompetitionManager : MonoBehaviour
{
    public GameManager gm;

    public Color warningColor;

    public List<Competitor> competitors = new List<Competitor>();
    public TextMeshPro leaderBoardLabel;
    public TextMeshPro timerLabel;
    public string[] competitorNames;


    public void StartCompetition(InteractionMethod im)
    {
        competitors.Clear();
        leaderBoardLabel.gameObject.SetActive(true);
        timerLabel.gameObject.SetActive(true);
        foreach (string compName in competitorNames)
        {
            competitors.Add(new Competitor(compName));
        }
        if (im == InteractionMethod.enhanced)
        {
            //set handmodel to exoskeleton
            gm.leftHand.enhancedMultiplyer = gm.enhancedThrowMultiplyer;
            gm.rightHand.enhancedMultiplyer = gm.enhancedThrowMultiplyer;
        }
        else if(im == InteractionMethod.normal)
        {
            //set handmodel to normal glove
            gm.leftHand.enhancedMultiplyer = gm.defaultThrowMultiplyer;
            gm.rightHand.enhancedMultiplyer = gm.defaultThrowMultiplyer;
        }
        else
        {
            //set handmodel to magical glove
            gm.leftHand.enhancedMultiplyer = gm.defaultThrowMultiplyer;
            gm.rightHand.enhancedMultiplyer = gm.defaultThrowMultiplyer;
            Debug.Log("activating laser because we are doing magic========");
            Pointer.activateLaser = true;
        }
        competitors.Add(new Competitor(gm.playerName));
        gm.pm.Repopulate();
        gm.target.gameObject.SetActive(true);
        UpdateLeaderBoard();
        StartCoroutine("Countdown", gm.timeInScenario);
    }

    public void EndCompetition()
    {
        leaderBoardLabel.gameObject.SetActive(false);
        timerLabel.gameObject.SetActive(false);

        //@TODO: set handmodel to default again

        if (gm.currentInteractionMethod == InteractionMethod.magical && gm.blackHole.GetComponent<Attractor>().doAttract)
        {
            gm.blackHole.GetComponent<Attractor>().doAttract = false;
            gm.blackHole.GetComponent<Attractor>().StopVisuals();
            gm.blackHole.transform.position = new Vector3(0f, -10f, 0f);
            AdJustParticleSystem.Collapse();
        }

        //destroy hidden snowballs from questionnairemode
        gm.qm.spawnPlatform.GetComponent<Spawner>().ClearProjectiles();

        //hide projectiles - they will get destroyed at the end of questionnairemode
        Debug.Log("Hiding Projectiles from pm");
        gm.pm.HideProjectiles();

        gm.target.gameObject.SetActive(false);

        gm.leftHand.enhancedMultiplyer = gm.defaultThrowMultiplyer;
        gm.rightHand.enhancedMultiplyer = gm.defaultThrowMultiplyer;
        Pointer.activateLaser = false;

        gm.qm.StartQuestionnaireMode();
    }

    public void UpdateLeaderBoard()
    {
        foreach (Competitor competitor in competitors)
        {
            if (competitor.name == gm.playerName)
            {
                competitor.score = gm.points;
            }

            if (competitor.name != gm.playerName)
            {
                StartCoroutine(JanDelayScoreUpdate(competitor.name));
            }
        }

        //sorting the competitors for the leaderboard
        competitors.Sort();
        competitors.Reverse();

        UpdateLeaderBoardLabel();
    }

    private IEnumerator JanDelayScoreUpdate(string i)
    {
        yield return new WaitForSeconds(Random.Range(2f, 5f));
        var competitor = competitors.Find(n => n.name == i);

        if (competitor != null)
        {
            competitor.score += Random.Range(0, 2) * 25;
        }

        competitors.Sort();
        competitors.Reverse();

        UpdateLeaderBoardLabel();
    }

    //just update the label itself. 
    public void UpdateLeaderBoardLabel()
    {
        leaderBoardLabel.text = "Leaderboard:";
        foreach (Competitor comp in competitors)
        {
            leaderBoardLabel.text += "\n" + comp.name + "\t" + comp.score;
        }
    }

    //coroutine to countdown the remaining seconds
    private IEnumerator Countdown(int time)
    {
        while (time > 0)
        {
            if (time < 11 && timerLabel.color == Color.white)
            {
                timerLabel.color = warningColor;
            }
            UpdateTimerLabel(time--);
            yield return new WaitForSeconds(1);
        }
        EndCompetition();
    }

    //takes the remaining time in seconds and converts it to minutes 
    //and seconds to display it on the timerLabel.
    private void UpdateTimerLabel(int time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = time % 60;

        if (seconds < 10)
        {
            timerLabel.text = "" + minutes + ":0" + seconds;
        }
        else
        {
            timerLabel.text = "" + minutes + ":" + seconds;
        }
    }

}
