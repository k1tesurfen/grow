using UnityEngine;

public class Snowball : MonoBehaviour
{
    [Header("References:")]
    public GameManager gm;

    public Logger snowballLogger;
    public bool spawnForQuestionnaire;

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

            if (GetComponent<Rigidbody>().velocity.sqrMagnitude > maxSpeed)
            {
                maxSpeed = GetComponent<Rigidbody>().velocity.sqrMagnitude;
            }
        }
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
                //if the hit object is target, register the hit
                if (col.collider.transform.gameObject.name
                    == "Target")
                {
                    gm.target.RegisterHit(col.GetContact(0).point);
                    float deviation = (gm.target.center.transform.position - col.GetContact(0).point).magnitude;
                    gm.accuracy += deviation;
                    if (deviation < 0.675f)
                    {
                        gm.points += 100;
                    }
                    else if (deviation < 1.867f)
                    {
                        gm.points += 75;
                    }
                    else if(deviation < 3.029f)
                    {
                        gm.points += 50;
                    }
                    else if(deviation < 4f)
                    {
                        gm.points += 25;
                    }
                    else
                    {
                        gm.points += 0;
                    }
                }

                //log throw properties
                //gm.logger.Log(col.collider.transform.gameObject.name);
                snowballLogger.Log(gm.GetTimeStamp() + ";" + col.collider.gameObject.name + ";" + Mathf.Sqrt(maxSpeed));

                //snowball destroy sequence
                gm.scatter.Explode(col.GetContact(0).point);
                DestroySnowball();
            }
        }
    }
}
