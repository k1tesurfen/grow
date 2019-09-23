using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    const float G = 66.74f;

    public int mass;
    public float rotateSpeed = 200f;
    public static List<Attractor> Attractables;
    public bool isAttractor = false;
    public bool doAttract = false;
    private float lifeSpan = 5f;
    //private float attractionRadius = 200f;
    //private float turnSpeed = 20f;
    
    
    void FixedUpdate()
    {
        foreach (Attractor at in Attractables)
        {
            if (at != this && isAttractor && at.doAttract)
            {
                Attract(at);
                //Rigidbody rbAt = at.GetComponent<Rigidbody>();
                //Vector3 direction = transform.position - at.transform.position;
                //if (direction.sqrMagnitude < 100)
                //{
                //    rbAt.useGravity = false;
                //    rbAt.velocity = at.transform.forward * (rbAt.velocity.magnitude + (3f / direction.sqrMagnitude));
                //    Quaternion targetRotation = Quaternion.LookRotation(direction);
                //    rbAt.MoveRotation(Quaternion.RotateTowards(at.transform.rotation, targetRotation, turnSpeed));
                //}
            }
        }
    }

    void Update()
    {
        if (isAttractor)
        {
            if (lifeSpan < 0f)
                Destroy(gameObject);

            lifeSpan -= Time.deltaTime;
        }
    }

    void OnEnable()
    {
        if (Attractables == null)
            Attractables = new List<Attractor>();

        Attractables.Add(this);
    }

    void OnDisable()
    {
        Attractables.Remove(this);
    }

    void Attract(Attractor objToAttract)
    {
        Vector3 direction = transform.position - objToAttract.transform.position;
        float distance = direction.magnitude;

        if (distance == 0f || objToAttract.transform.position.z > transform.position.z)
        {
            return;
            Attractables.Remove(objToAttract);
        }
        else
        {
            float forceMagnitude = G * (this.mass * objToAttract.mass) / Mathf.Pow(distance, 2);
            Vector3 force = direction.normalized * Mathf.Clamp(forceMagnitude, 0.0f, 1000f);

            objToAttract.GetComponent<Rigidbody>().AddForce(force);
        }
    }
}
