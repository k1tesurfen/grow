using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowball : MonoBehaviour
{
    [Header("References:")]
    public GameManager gm;

    private SphereCollider groundCollider;

    void OnEnable()
    {
        //groundCollider = gameObject.AddComponent<SphereCollider>();
        //groundCollider.radius = 0.062f;
        //groundCollider.isTrigger = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        groundCollider = gameObject.AddComponent<SphereCollider>();
        groundCollider.radius = 0.07f;
        groundCollider.isTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<OVRGrabbable>().isGrabbed)
        {
            Destroy(groundCollider);
        }
    }

    //detects if snowball hits anything, creates impact and destroys snowball gameobject
    private void OnCollisionEnter(Collision col)
    {
        if (false)
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
            Destroy(gameObject);
        }
    }
}
