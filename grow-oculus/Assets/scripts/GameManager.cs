using System;
using UnityEngine;
using Random = UnityEngine.Random;

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

    [Space(20)] // 10 pixels of spacing here.

    //participantID 
    public string playerName;

    //Count up for each participant.
    public int playerNumber;
    public bool rightHanded;

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
         
    }
    
    //sets the playarea to fit the handedness of a player
    public void SetHandedness()
    {
        if (rightHanded)
        {
            

        }
        else
        {

        }
    }


    public string GetTimeStamp()
    {
        return DateTime.UtcNow.ToString("HH:mm:ss:fff");
    }

}
