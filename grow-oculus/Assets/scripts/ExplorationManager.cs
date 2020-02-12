﻿using System.Collections;
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
        //gm.target.gameObject.SetActive(true);

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
            Debug.Log("activating laser because we are doing magic========");
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

        //if (gm.currentInteractionMethod == InteractionMethod.magical && gm.blackHole.GetComponent<Attractor>().doAttract)
        //{
        //    //make linerenderer and particlesystem disappear after condition
        //    gm.mainHand.GetComponent<Pointer>().lineRenderer.enabled = false;
        //    gm.mainHand.GetComponent<Pointer>().ray.transform.position = new Vector3(0, -100, 0);
        //    gm.mainHand.GetComponent<Pointer>().ray.Stop();
        //    gm.offHand.GetComponent<Pointer>().lineRenderer.enabled = false;
        //    gm.offHand.GetComponent<Pointer>().ray.transform.position = new Vector3(0, -100, 0);
        //    gm.offHand.GetComponent<Pointer>().ray.Stop();

        //    AdJustParticleSystem.Collapse();
        //}

        //after the condition no lasers should be visible
        if (gm.currentInteractionMethod == InteractionMethod.magical)
        {
            gm.mainHand.GetComponent<Pointer>().HideVisuals();
            gm.offHand.GetComponent<Pointer>().HideVisuals();
            AdJustParticleSystem.Collapse();
        }
        Pointer.activateLaser = false;

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
