using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("References:")]
    public GameManager gm;
    public Attractor attractor;

    public Logger projectileLogger;
    public bool spawnForQuestionnaire = false;

    //private SphereCollider groundCollider;

    public InteractionMethod interactionMethod;

    [HideInInspector]
    public OVRGrabbable grabbable;

    //after Projectile is grabbed it gets armed to watch for collisions
    public bool isArmed = false;
    public bool isBeforeImpact = true;

    [HideInInspector]
    public float maxSpeed;

    public Rigidbody rb;

    private void Enable()
    {
        //groundCollider = gameObject.AddComponent<SphereCollider>();
        //groundCollider.radius = 0.062f;
        //groundCollider.isTrigger = false;
    }

    ////detects if Projectile hits anything, creates impact and destroys Projectile gameobject
    //private void OnTriggerEnter(Collider col)
    //{
    //    Debug.Log("I'm a Projectile and colliding with : " + col.gameObject.name);
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

    //        //Projectile destroy sequence
    //        gm.scatter.Explode(transform.position);
    //        Destroy(gameObject);
    //    }
    //}

    private void FixedUpdate()
    {
        //this condition only occurs when a Projectile is thrown
        if (isArmed && !grabbable.isGrabbed)
        {
            if (isBeforeImpact)
            {
                rb.rotation = Quaternion.LookRotation(rb.velocity);
            }
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

    public void HideProjectile()
    {
        gameObject.SetActive(false);
    }

    public void DestroyProjectile()
    {
        Destroy(gameObject);
    }

    //detects if Projectile hits anything, creates impact and destroys Projectile gameobject
    private void OnCollisionEnter(Collision col)
    {
        if (grabbable != null)
        {
            if (!grabbable.isGrabbed && isArmed && col.collider.gameObject.layer == 10)
            {
                if(!spawnForQuestionnaire)
                {

                    //if the hit object is target, register the hit
                    if (col.collider.transform.gameObject.name
                        == "Target")
                    {
                        //gm.target.RegisterHit(col.GetContact(0).point);

                        float deviation = (gm.target.center.transform.position - col.GetContact(0).point).magnitude;
                        gm.accuracy += deviation;
                        gm.ProjectilesOnTarget++;

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
                            gm.blackHole.GetComponent<Attractor>().doAttract = false;
                            gm.blackHole.GetComponent<Attractor>().StopVisuals();
                            AdJustParticleSystem.Collapse();
                        }
                    }
                    gm.thrownProjectiles++;
                    //ProjectileLogger.Log(gm.GetTimeStamp() + ";" + col.collider.gameObject.name + ";" + Mathf.Sqrt(maxSpeed));
                }

                isBeforeImpact = false;
                rb.useGravity = false;
                rb.isKinematic = true;
                StartCoroutine(JanDelay(1f));
            }
        }
    }

    IEnumerator JanDelay(float time)
    {
        yield return new WaitForSeconds(time);
        DestroyProjectile();
    }
}
