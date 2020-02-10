using System;
using System.Collections.Generic;
using System.Linq;
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
    public GameObject endScreen;
    public OVRGrabber leftHand;
    public OVRGrabber rightHand;
    public Pointer mainHand;
    public Pointer offHand;

    private static float scale;
    public static float Scale { get { return scale; } }


    public Vector3 lastPos;
    public List<float> lastVelocities = new List<float>();

    public float defaultTwister = 1f;
    public float twisterDeviation = 0.3f;

    //participantID 
    [Space(20)]
    public string playerName;

    //Count up for each participant.
    public int playerNumber;
    public bool rightHanded;

    public float enhancedThrowMultiplyer = 5f;
    public float defaultThrowMultiplyer = 2f;

    public bool storyTime;

    public int interactionLS;
    public int scenarioLS;

    private bool firstCondition = true;

    [HideInInspector]
    public Scenario currentScenario;
    [HideInInspector]
    public InteractionMethod currentInteractionMethod;

    [HideInInspector]
    public BlackHoleStatus currentBlackHoleStatus;

    public int timeInScenario = 120;

    //all information gathered for logging
    [HideInInspector]
    public int points = 0;
    [HideInInspector]
    public int thrownProjectiles = 0;
    [HideInInspector]
    public int ProjectilesOnTarget = 0;
    [HideInInspector]
    public float accuracy = 0f;

    [HideInInspector]
    public List<int> answers = new List<int>();


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

    public void FixedUpdate()
    {
        //
        // Twistermovement of the hand
        //
        if (currentInteractionMethod == InteractionMethod.magical)
        {
            if (lastPos != null)
            {
                lastVelocities.Add(Vector3.Distance(mainHand.transform.position, lastPos) / Time.fixedDeltaTime);
                while (lastVelocities.Count >= 15)
                {
                    lastVelocities.RemoveAt(0);
                }
            }
            float velocity = lastVelocities.Sum();

            velocity /= lastVelocities.Count;
            scale = velocity - defaultTwister;
            if(scale < 0)
            {
                scale = 0;
            }
            if (scale > twisterDeviation)
            {
                scale = twisterDeviation;
            }
            scale /= twisterDeviation;
            //Debug.Log("Scale: " + scale + " Velocity: " + velocity);
            lastPos = mainHand.transform.position;

            //if (lastPos != null)
            //{
            //    lastVelocities.Add(Vector3.Distance(mainHand.transform.position, lastPos) / Time.fixedDeltaTime);
            //    while (lastVelocities.Count >= 15)
            //    {
            //        lastVelocities.RemoveAt(0);
            //    }
            //}
            //float velocity = lastVelocities.Sum();

            //velocity /= lastVelocities.Count;
            //scale = Mathf.Abs(velocity - defaultTwister);
            //scale = velocity - defaultTwister;
            ////scale has to be between defaulttwister and max(default+deviation)
            //if (scale < defaultTwister)
            //{
            //    scale = 0;
            //}
            //if(scale > defaultTwister + twisterDeviation)
            //{
            //    scale = twisterDeviation;
            //}
            //scale /= twisterDeviation;
            //scale = 1 - scale;
            ////Debug.Log("Scale: " + scale + " Velocity: " + velocity);
            //lastPos = mainHand.transform.position;
        }
        //
        // END Twistermovement of the hand
        //
    }

    public void InitiateExperiment()
    {
        //set handedness, 
        SetHandedness();
        NextCondition();
    }

    public void NextCondition()
    {
        if (!firstCondition)
        {
            //Log the previous conditions data.
            LogCondition();
        }
        else
        {
            firstCondition = false;
        }

        //check if the experiment has already finished
        if (latinSquare.finished)
        {
            EndExperiment();
            return;
        }

        currentScenario = latinSquare.GetScenario();
        currentInteractionMethod = latinSquare.GetInteractionMethod();

        Debug.Log("current scenario is: " + currentScenario + ". with the interaction method: " + currentInteractionMethod);

        if (currentScenario == Scenario.competitive)
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
    //timestamp - playerID - scenario - interactionMethod - Projectilesthrown - ProjectilesOnTarget - points - accuracyAVG - questionanswers
    public void LogCondition()
    {
        string questionnaireAnswers = string.Join(";", answers);
        logger.Log(GetTimeStamp() + ";" + playerName + ";" + currentScenario + ";" + currentInteractionMethod + ";"
                         + thrownProjectiles + ";" + ProjectilesOnTarget + ";" + points + ";" + (accuracy / ProjectilesOnTarget) + ";" + questionnaireAnswers);
        answers.Clear();
        points = 0;
        accuracy = 0;
        ProjectilesOnTarget = 0;

    }

    //sets the playarea to fit the handedness of a player
    public void SetHandedness()
    {
        //set the position of the undo button. 
        //for a righthanded player the button has to be on the left side to avoid accidental activation.
        undoButton.SetPosition(rightHanded);
        undoButton.SetRotation(rightHanded);

        if (rightHanded)
        {
            rightHand.enhancedMultiplyer = defaultThrowMultiplyer;
            leftHand.GetComponent<Pointer>().isLaserHand = true;
            mainHand = rightHand.gameObject.GetComponent<Pointer>();
            offHand = leftHand.gameObject.GetComponent<Pointer>();
        }
        else
        {
            leftHand.enhancedMultiplyer = defaultThrowMultiplyer;
            rightHand.GetComponent<Pointer>().isLaserHand = true;
            mainHand = leftHand.gameObject.GetComponent<Pointer>();
            offHand = rightHand.gameObject.GetComponent<Pointer>();
        }

    }

    public void EndExperiment()
    {
        qm.spawnPlatform.GetComponent<Spawner>().HideProjectiles();
        endScreen.SetActive(true);
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

public enum BlackHoleStatus
{
    selection = 0,
    force = 1,
    wurf = 2
};

