using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class CompetitionManager : MonoBehaviour
{
    public GameManager gm;

    //collected points of the player.
    public int points;

    public Color warningColor;

    public List<Competitor> competitors = new List<Competitor>();
    public TextMeshPro leaderBoardLabel;
    public TextMeshPro timerLabel;
    public string[] competitorNames;

    //Adds points the player made
    private void AddPoints(int points)
    {
        this.points += points;
    }

    public void StartCompetition(InteractionMethod im)
    {
        points = 0;
        leaderBoardLabel.gameObject.SetActive(true);
        timerLabel.gameObject.SetActive(true);
        foreach (string compName in competitorNames)
        {
            competitors.Add(new Competitor(compName));
        }
        competitors.Add(new Competitor(gm.playerName));
        gm.pm.Repopulate();
        gm.target.gameObject.SetActive(true);
        StartCoroutine("Countdown", gm.timeInScenario);
    }

    public void EndCompetition()
    {
        gm.points = points; 
        
        points = 0;

        leaderBoardLabel.gameObject.SetActive(false);
        timerLabel.gameObject.SetActive(false);

        gm.pm.ClearProjectiles();
        gm.target.gameObject.SetActive(false);
        
        gm.qm.StartQuestionnaireMode();
    }

    public void UpdateLeaderBoard(int points)
    {
        foreach (Competitor competitor in competitors)
        {
            if (competitor.name == gm.playerName)
            {
                competitor.score += points;
                AddPoints(points);
            }
        }

        //sorting the competitors for the leaderboard
        competitors.Sort();
        competitors.Reverse();

        UpdateLeaderBoardLabel();

        foreach (Competitor c in competitors)
        {
            if (c.name != gm.playerName)
            {
                StartCoroutine(JanDelayScoreUpdate(c.name));
            }
        }
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
        foreach(Competitor comp in competitors)
        {
            leaderBoardLabel.text += "\n" + comp.name + "\t" + comp.score;
        }
    }
    
    //coroutine to countdown the remaining seconds
    private IEnumerator Countdown(int time)
    {
        while (time > 0)
        {
            if(time < 11 && timerLabel.color == Color.white)
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

        if(seconds < 10)
        {
            timerLabel.text = "" + minutes + ":0" + seconds;
        }
        else
        {
            timerLabel.text = "" + minutes + ":" + seconds;
        }
    }

}
