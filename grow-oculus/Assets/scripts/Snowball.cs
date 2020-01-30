using UnityEngine;

public class Snowball : MonoBehaviour
{
    [Header("References:")]
    public GameManager gm;

    private SphereCollider groundCollider;

    //after snowball is grabbed it gets armed to watch for collisions
    public bool armed;

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

    public void DestroySnowball()
    {
        Destroy(gameObject);
    }

    //detects if snowball hits anything, creates impact and destroys snowball gameobject
    private void OnCollisionEnter(Collision col)
    {
        if (gameObject.GetComponent<OVRGrabbable>() != null)
        {
            if (!gameObject.GetComponent<OVRGrabbable>().isGrabbed && armed && col.collider.gameObject.layer == 10)
            {
                //if the hit object is target, register the hit
                if (col.collider.transform.gameObject.name
                    == "Target")
                {
                    gm.target.RegisterHit(col.GetContact(0).point);
                }

                //log throw properties
                //gm.logger.Log(col.collider.transform.gameObject.name);

                //snowball destroy sequence
                gm.scatter.Explode(col.GetContact(0).point);
                DestroySnowball();
            }
        }
    }
}
