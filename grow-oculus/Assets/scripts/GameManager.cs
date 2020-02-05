using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References:")]
    public Logger logger;
    public Scatter scatter;
    public Target target;
    public CompetitionManager competition;
    public GameObject hand;
    public QuestionaireManager qm;
    public FortressManager fm;
    public ProjectileManager pm;
    public UndoButton undoButton;


    [Space(20)] // 10 pixels of spacing here.

    //participantID 
    public string playerName;

    //Count up for each participant.
    public int playerNumber;
    public bool rightHanded;

    [HideInInspector]
    public Scenario currentScenario = Scenario.competitive;
    [HideInInspector]
    public InteractionMethod currentInteractionMethod = InteractionMethod.normal;

    //all information gathered for logging
    [HideInInspector]
    public int points;
    public float accuracy;

    public List<int> answers;

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
        if (Input.GetKeyDown(KeyCode.Return))
        {
            competition.EndCompetition();
            qm.StartQuestionnaireMode();
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            competition.UpdateLeaderBoard(50);
        }
        if (OVRInput.GetDown(OVRInput.Button.Two))
        {
            qm.StartQuestionnaireMode();
        }
    }

    public void InitiateExperiment()
    {
        //set handedness, 
        SetHandedness();
        competition.StartCompetition();
    }

    public void NextCondition()
    {
        LogCondition();
        
        //@TODO: prepare next condition and start it
    }

    //log data from current condition
    public void LogCondition()
    {
        string questionnaireAnswers = string.Join(";", answers);
        logger.Log(GetTimeStamp() + ";" + playerName + ";" + currentScenario + ";" + currentInteractionMethod + ";"
                         + points + ";" + questionnaireAnswers);
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
    competitive = 0,
    exploratory = 1
};

