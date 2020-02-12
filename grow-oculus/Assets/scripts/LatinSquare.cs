using UnityEngine;

public class LatinSquare : MonoBehaviour
{
    public GameManager gm;

    public int[,] latinSquareInteraction = new int[,] { { 0, 1, 2 }, { 1, 2, 0 }, { 2, 0, 1 } };

    public int[,] latinSquareScenario = new int[,] { { 0, 1 }, { 1, 0 } };

    public int interactionIndex = -1;
    public int scenarioIndex = 2;

    public bool finished = false;

    //returns the interaction method the player has to play.
    public InteractionMethod GetInteractionMethod()
    {
        if (scenarioIndex > 1)
        {
            interactionIndex++;
            scenarioIndex = 0;
            //play next interaction audio

            //if we have a legit interactionmethod at hand
            if (interactionIndex < 3)
            {
                InteractionMethod im = (InteractionMethod)latinSquareInteraction[gm.interactionLS, interactionIndex];
                if (gm.storyTime)
                {
                    switch (im)
                    {
                        case InteractionMethod.normal:
                            Debug.Log("story time + interaction method normal");
                            gm.audioManager.AddToQueue("storynormal");
                            break;
                        case InteractionMethod.enhanced:
                            Debug.Log("story time + interaction method enhanced");
                            gm.audioManager.AddToQueue("storyenhanced");
                            break;
                        case InteractionMethod.magical:
                            Debug.Log("story time + interaction method magical");
                            gm.audioManager.AddToQueue("storymagical");
                            break;
                    }
                    
                }
                else
                {
                    switch (im)
                    {
                        case InteractionMethod.normal:
                            Debug.Log("interaction method normal");
                            gm.audioManager.AddToQueue("normal");
                            break;
                        case InteractionMethod.enhanced:
                            Debug.Log("interaction method enhanced");
                            gm.audioManager.AddToQueue("enhanced");
                            break;
                        case InteractionMethod.magical:
                            Debug.Log("interaction method magical");
                            gm.audioManager.AddToQueue("magical");
                            break;
                    }
                }
            }
        }
        InteractionMethod ret = (InteractionMethod)latinSquareInteraction[gm.interactionLS, interactionIndex];
        return ret;
    }

    //returns the scenario the player must play(competetive/explorative).
    public Scenario GetScenario()
    {
        Scenario ret = (Scenario)latinSquareScenario[gm.scenarioLS, scenarioIndex];
        scenarioIndex++;
        if (interactionIndex == 2 && scenarioIndex > 1)
        {
            finished = true;
        }
        return ret;
    }


    ////returns the scenario the player must play(competetive/explorative).
    //public Scenario GetScenario()
    //{
    //    if (interactionIndex > 2)
    //    {
    //        scenarioIndex++;
    //        interactionIndex = 0;
    //    }
    //    Scenario ret = (Scenario)latinSquareScenario[gm.scenarioLS, scenarioIndex];
    //    return ret;
    //}

    ////returns the interaction method the player has to play.
    //public InteractionMethod GetInteractionMethod()
    //{
    //    InteractionMethod ret = (InteractionMethod)latinSquareInteraction[gm.interactionLS, interactionIndex];
    //    interactionIndex++;
    //    if(scenarioIndex == 1 && interactionIndex > 2)
    //    {
    //        finished = true; 
    //    }
    //    return ret;
    //}
}
