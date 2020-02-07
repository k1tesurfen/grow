using UnityEngine;

public class Snowball : MonoBehaviour
{
    [Header("References:")]
    public GameManager gm;
    public Attractor attractor;

    public Logger snowballLogger;
    public bool spawnForQuestionnaire = false;

    private SphereCollider groundCollider;

    public InteractionMethod interactionMethod;

    public OVRGrabbable grabbable;

    //after snowball is grabbed it gets armed to watch for collisions
    public bool armed;

    public float maxSpeed;

    private void Enable()
    {
        groundCollider = gameObject.AddComponent<SphereCollider>();
        groundCollider.radius = 0.062f;
        groundCollider.isTrigger = false;
    }

    ////detects if snowball hits anything, creates impact and destroys snowball gameobject
    //private void OnTriggerEnter(Collider col)
    //{
    //    Debug.Log("I'm a snowball and colliding with : " + col.gameObject.name);
    //    Debug.Log("Do I have the correct layer? " + col.transform.gameObject.layer);
    //    if (!gameObject.GetComponent<OVRGrabbable>().isGrabbed && armed && col.transform.gameObject.layer == 10)
    //    {
    //        Debug.Log("And I'm even not grabbed.");
    //        //if the hit object is target, register the hit
    //        if (col.gameObject.name
    //            == "Target")
    //        {
    //            gm.target.RegisterHit(transform.position);
    //        }

    //        //log throw properties
    //        //gm.logger.Log(col.collider.transform.gameObject.name);

    //        //snowball destroy sequence
    //        gm.scatter.Explode(transform.position);
    //        Destroy(gameObject);
    //    }
    //}

    private void FixedUpdate()
    {
        //this condition only occurs when a snowball is thrown
        if (armed && !grabbable.isGrabbed)
        {
            //@TODO: if unreal condition, set Attractor.doAttract to true
            //set the blackhole to GO!
            if (gm.currentInteractionMethod == InteractionMethod.magical)
            {
                if (!attractor.doAttract)
                {
                    attractor.doAttract = true;
                }
            }

            if (GetComponent<Rigidbody>().velocity.sqrMagnitude > maxSpeed)
            {
                maxSpeed = GetComponent<Rigidbody>().velocity.sqrMagnitude;
            }
        }
    }

    public void HideSnowball()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    public void DestroySnowball()
    {
        Destroy(gameObject);
    }

    //detects if snowball hits anything, creates impact and destroys snowball gameobject
    private void OnCollisionEnter(Collision col)
    {
        if (grabbable != null)
        {
            if (!grabbable.isGrabbed && armed && col.collider.gameObject.layer == 10)
            {
                if (!spawnForQuestionnaire)
                {
                    //if the hit object is target, register the hit
                    if (col.collider.transform.gameObject.name
                        == "Target")
                    {
                        gm.target.RegisterHit(col.GetContact(0).point);
                        float deviation = (gm.target.center.transform.position - col.GetContact(0).point).magnitude;
                        gm.accuracy += deviation;
                        gm.snowballsOnTarget++;
                        if (deviation < 0.675f)
                        {
                            Debug.Log("=================100p");
                            gm.points += 100;
                        }
                        else if (deviation < 1.867f)
                        {
                            gm.points += 75;
                            Debug.Log("=================75p");
                        }
                        else if (deviation < 3.029f)
                        {
                            gm.points += 50;
                            Debug.Log("=================50p");
                        }
                        else if (deviation < 4f)
                        {
                            Debug.Log("=================25p");
                            gm.points += 25;
                        }
                        else
                        {
                            Debug.Log("=================0p");
                            gm.points += 0;
                        }
                        gm.competitionManager.UpdateLeaderBoard();

                        //kill the blackhole if it is the right circumstance
                        if (gm.currentInteractionMethod == InteractionMethod.magical && gm.blackHole.GetComponent<Attractor>().doAttract)
                        {
                            gm.blackHole.GetComponent<Attractor>().doAttract =  false;
                            gm.blackHole.GetComponent<Attractor>().StopVisuals();
                            gm.blackHole.transform.position = new Vector3(0f, -10f, 0f);
                            gm.leftHand.GetComponent<Pointer>().blackHoleIsSet = false;
                            gm.rightHand.GetComponent<Pointer>().blackHoleIsSet = false;
                        }
                    }
                    gm.thrownSnowballs++;
                    //log throw properties
                    //gm.logger.Log(col.collider.transform.gameObject.name);
                    snowballLogger.Log(gm.GetTimeStamp() + ";" + col.collider.gameObject.name + ";" + Mathf.Sqrt(maxSpeed));
                }

                //snowball destroy sequence
                gm.scatter.Explode(col.GetContact(0).point);
                DestroySnowball();
            }
        }
    }
}
