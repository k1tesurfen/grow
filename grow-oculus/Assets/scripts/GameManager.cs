using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References:")]
    public Logger logger;
    public Scatter scatter;
    public Target target;
    public LatinSquare latinSquare;
    public CompetitionManager competitionManager;
    public ExplorationManager explorationManager;
    public GameObject hand;
    public QuestionaireManager qm;
    public FortressManager fm;
    public ProjectileManager pm;
    public UndoButton undoButton;
    public GameObject blackHole;



    //participantID 
    [Space(20)]
    public string playerName;

    //Count up for each participant.
    public int playerNumber;
    public bool rightHanded;

    public bool storyTime;

    public int interactionLS;
    public int scenarioLS;

    private bool firstCondition = true;

    [HideInInspector]
    public Scenario currentScenario;
    [HideInInspector]
    public InteractionMethod currentInteractionMethod;

    public int timeInScenario = 120;

    //all information gathered for logging
    [HideInInspector]
    public int points;
    [HideInInspector]
    public float accuracy;

    [HideInInspector]
    public List<int> answers;


    [Space(30)]
    public string currentGroup;

    [Space(10)]
    public string currentCondition;

    //poisson disk spawning
    //public float radius = 1;
    //public Vector2 regionSize = new Vector2(15, 15);
    //List<Vector2> spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        //competition.StartGame();
        //competition.UpdateLeaderBoard(25);
        InitiateExperiment();
    }

    private void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One))
        {
            blackHole.SetActive(true);
        }
    }

    public void InitiateExperiment()
    {
        //set handedness, 
        SetHandedness();
        NextCondition();
    }

    public void NextCondition()
    {
        Debug.Log("starting next scenario");

        if (!firstCondition)
        {
            //Log the previous conditions data.
            LogCondition();
        }
        else
        {
            firstCondition = false;
        }

        currentScenario = latinSquare.GetScenario();
        currentInteractionMethod = latinSquare.GetInteractionMethod();

        Debug.Log("current scenario is: " + currentScenario + ". with the interaction method: " + currentInteractionMethod);

        if(currentScenario == Scenario.competitive)
        {
            //start competetive scenario
            competitionManager.StartCompetition(currentInteractionMethod);
        }
        else
        {
            //start exploratory scenario
            explorationManager.StartExploration(currentInteractionMethod);
        }
    }

    //log data from current condition
    public void LogCondition()
    {
        string questionnaireAnswers = string.Join(";", answers);
        logger.Log(GetTimeStamp() + ";" + playerName + ";" + currentScenario + ";" + currentInteractionMethod + ";"
                         + points + ";" + questionnaireAnswers);
        answers.Clear();
    }

    //sets the playarea to fit the handedness of a player
    public void SetHandedness()
    {
        //set the position of the undo button. 
        //for a righthanded player the button has to be on the left side to avoid accidental activation.
        undoButton.SetPosition(rightHanded);
        undoButton.SetRotation(rightHanded);
    }


    public string GetTimeStamp()
    {
        return DateTime.UtcNow.ToString("HH:mm:ss:fff");
    }

}

public enum InteractionMethod
{
    normal = 0,
    enhanced = 1,
    magical = 2
};

public enum Scenario
{
    exploratory = 0,
    competitive = 1
};

