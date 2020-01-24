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

    public string playerName;

    public FortressManager fm;
    //poisson disk spawning
    //public float radius = 1;
    //public Vector2 regionSize = new Vector2(15, 15);
    //List<Vector2> spawnPoints;

    // Start is called before the first frame update
    void Start()
    {
        competition.StartGame();
        competition.UpdateLeaderBoard(25);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            fm.HideFortress();
            qm.StartNextQuestionaire();
        }
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            competition.UpdateLeaderBoard(50);
        }
    }

    public string GetTimeStamp()
    {
        return DateTime.UtcNow.ToString("HH:mm:ss:fff");
    }

}
