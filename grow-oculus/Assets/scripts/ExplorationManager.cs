using System.Collections;
using UnityEngine;

public class ExplorationManager : MonoBehaviour
{
    public GameManager gm;
    public GameObject target;

    //Points the player made druing condition
    public int points;

    public void StartExploration(InteractionMethod im)
    {
        points = 0;
        gm.pm.Repopulate();
        gm.target.gameObject.SetActive(true);

        if (im == InteractionMethod.enhanced)
        {
            //set handmodel to exoskeleton
            gm.leftHand.enhancedMultiplyer = gm.enhancedThrowMultiplyer;
            gm.rightHand.enhancedMultiplyer = gm.enhancedThrowMultiplyer;
        }
        else if (im == InteractionMethod.normal)
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
            Pointer.activateLaser = true;
        }

        StartCoroutine(Countdown(gm.timeInScenario));
    }

    public void EndExploration()
    {
        gm.target.gameObject.SetActive(false);

        //@TODO: set handmodel to default
        gm.leftHand.enhancedMultiplyer = gm.defaultThrowMultiplyer;
        gm.rightHand.enhancedMultiplyer = gm.defaultThrowMultiplyer;

        //hide darts
        gm.pm.HideProjectiles();

        //destroy snowballs from previous questionnaire mode.
        gm.qm.spawnPlatform.GetComponent<Spawner>().ClearProjectiles();

        if (gm.currentInteractionMethod == InteractionMethod.magical && gm.blackHole.GetComponent<Attractor>().doAttract)
        {
            gm.blackHole.GetComponent<Attractor>().doAttract = false;
            gm.blackHole.GetComponent<Attractor>().StopVisuals();
            AdJustParticleSystem.Collapse();
        }

        gm.qm.StartQuestionnaireMode();
    }

    //Adds points the player made
    private void AddPoints(int points)
    {
        this.points += points;
    }

    //coroutine to countdown the remaining seconds
    private IEnumerator Countdown(int time)
    {
        while (time > 0)
        {
            time--;
            yield return new WaitForSeconds(1);
        }
        EndExploration();
    }
}
