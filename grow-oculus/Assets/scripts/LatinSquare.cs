using UnityEngine;

public class LatinSquare : MonoBehaviour
{
    public GameManager gm;

    public int[,] latinSquareInteraction = new int[,] { { 0, 1, 2 }, { 1, 2, 0 }, { 2, 0, 1 } };

    public int[,] latinSquareScenario = new int[,] { { 0, 1 }, { 1, 0 } };

    private int interactionIndex = 0;
    private int scenarioIndex = 0;

    //returns the scenario the player must play(competetive/explorative).
    public Scenario GetScenario()
    {
        if (interactionIndex > 2)
        {
            scenarioIndex++;
            interactionIndex = 0;
        }
        Scenario ret = (Scenario)latinSquareScenario[gm.scenarioLS, scenarioIndex];
        return ret;
    }
    
    //returns the interaction method the player has to play.
    public InteractionMethod GetInteractionMethod()
    {
        InteractionMethod ret = (InteractionMethod)latinSquareInteraction[gm.interactionLS, interactionIndex];
        interactionIndex++;
        return ret;
    }
}
