using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    public QuestionaireManager qm;
    public FortressManager fm;
    public ProjectileManager pm;
    public UndoButton undoButton;
    public GameObject blackHole;
    public GameObject endScreen;
    public OVRGrabber leftHand;
    public OVRGrabber rightHand;
    public AudioManager audioManager;
    public TextMeshPro testThrowsLabel;

    private static float scale;
    public static float Scale { get { return scale; } }

    public Pointer mainHand;
    public Pointer offHand;

    OVRInput.Controller h1;
    OVRInput.Controller h2;

    public Vector3 lastPos;
    public List<float> lastVelocities = new List<float>();

    public float defaultTwister = 1f;
    public float twisterDeviation = 0.3f;

    //participantID 
    [Space(20)]
    [HideInInspector]
    public string playerName;

    public string visiblePlayerName;

    //Count up for each participant.
    public int playerNumber;
    [HideInInspector]
    public bool rightHanded;
    public bool visibleRightHanded;

    public float enhancedThrowMultiplyer = 5f;
    public float defaultThrowMultiplyer = 1.8f;

    //do we need a story for this participant
    public bool storyTime;

    public int interactionLS;
    public int scenarioLS;

    public bool customStart = false;
    public int customInteraction;
    public int customScenario;

    private bool firstCondition = true;
    public bool isConditionStarted = false;
    public bool isWaitingForCondition = false;
    public int numTestThrows = 3;

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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            InitiateExperiment();
        }
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
            if (scale < 0)
            {
                scale = 0;
            }
            if (scale > twisterDeviation)
            {
                scale = twisterDeviation;
            }
            scale /= twisterDeviation;
            lastPos = mainHand.transform.position;
        }
        //
        // END Twistermovement of the hand
        //
        if (isWaitingForCondition)
        {

            if (numTestThrows > 0)
            {
                testThrowsLabel.text = numTestThrows + " test-throws left.";
            }

            if (numTestThrows <= 0)
            {
                Pointer.activateLaser = false;
                offHand.GetComponent<Pointer>().HideVisuals();
                testThrowsLabel.text = "After the audio stops, press the right index trigger to start. Good luck and have fun!";
                pm.ClearProjectiles();
                blackHole.GetComponent<Attractor>().lifeSpan = 7f;
                //wenn ein trigger getriggert wird und kein audio spielt brechen wir die while ab
                if (((OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, h1) > 0.55f && !audioManager.isAudioPlaying) ||
                        (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, h2) > 0.55f && !audioManager.isAudioPlaying)))
                {
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
                    testThrowsLabel.gameObject.SetActive(false);
                    numTestThrows = 3;
                    isWaitingForCondition = false;
                }
            }
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
        if (!firstCondition)
        {
            //Log the previous conditions data.
            LogCondition();
        }
        else
        {
            firstCondition = false;
        }

        target.gameObject.SetActive(true);

        //check if the experiment has already finished
        if (latinSquare.finished)
        {
            EndExperiment();
            return;
        }

        if (customStart)
        {
            latinSquare.interactionIndex = customInteraction;
            latinSquare.scenarioIndex = customScenario;
            customStart = false;
        }
        currentInteractionMethod = latinSquare.GetInteractionMethod();
        currentScenario = latinSquare.GetScenario();

        if (currentScenario == Scenario.competitive)
        {
            audioManager.AddToQueue("competition");
        }
        else
        {
            audioManager.AddToQueue("exploration");
        }

        if (currentInteractionMethod == InteractionMethod.enhanced)
        {
            //set handmodel to exoskeleton
            leftHand.enhancedMultiplyer = enhancedThrowMultiplyer;
            rightHand.enhancedMultiplyer = enhancedThrowMultiplyer;
        }
        else if (currentInteractionMethod == InteractionMethod.normal)
        {
            //set handmodel to normal glove
            leftHand.enhancedMultiplyer = defaultThrowMultiplyer;
            rightHand.enhancedMultiplyer = defaultThrowMultiplyer;
        }
        else
        {
            //set handmodel to magical glove
            leftHand.enhancedMultiplyer = defaultThrowMultiplyer;
            rightHand.enhancedMultiplyer = defaultThrowMultiplyer;
            mainHand.SetLaserStage(LaserStages.setBlackHole);
            offHand.SetLaserStage(LaserStages.setBlackHole);
            Debug.Log("activating laser for testthrows");
            Pointer.activateLaser = true;
        }

        pm.Repopulate();
        testThrowsLabel.gameObject.SetActive(true);
        blackHole.GetComponent<Attractor>().lifeSpan = 20f;

        //Debug.Log("current scenario is: " + currentScenario + ". with the interaction method: " + currentInteractionMethod);
        isWaitingForCondition = true;
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

        h1 = mainHand.GetComponent<OVRGrabber>().m_controller;
        h2 = offHand.GetComponent<OVRGrabber>().m_controller;
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
    magical = 2,
};

public enum Scenario
{
    exploratory = 0,
    competitive = 1,
    end = 2
};

public enum BlackHoleStatus
{
    selection = 0,
    force = 1,
    wurf = 2
};

