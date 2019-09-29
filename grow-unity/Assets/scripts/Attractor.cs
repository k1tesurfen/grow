using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    const float G = 66.74f;

    public int mass;
    public static List<Attractor> Attractables;
    public bool isAttractor = false;

    //should the obToAttract be attracted or not
    public bool doAttract = false;
    private float lifeSpan = 500f;
    public float attractionRadius = 10f;

    void FixedUpdate()
    {
        foreach (Attractor at in Attractables)
        {
            if (at != this && isAttractor && at.doAttract)
            {
                Attract(at);
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

    private void OnDestroy()
    {
        Attractables.Remove(this);
    }

    void Attract(Attractor objToAttract)
    {
        Vector3 direction = transform.position - objToAttract.transform.position;
        float distance = direction.magnitude;

        if (distance < 0.3f)
        {
            return;
        }
        if (distance < attractionRadius)
        {
            //use homing as attraction mode. 
            Vector3 velocity = objToAttract.GetComponent<Rigidbody>().velocity;
            Vector3 targetVelocity = Vector3.Slerp(velocity.normalized, (direction).normalized, 1 - (distance / attractionRadius));
            objToAttract.GetComponent<Rigidbody>().velocity = targetVelocity * velocity.magnitude;
            return;
        }
        else
        {
            //use mass and gravity as attraction mode
            //force magnitude calculated by (m1 * m2) / dist^2
            float forceMagnitude = G * (mass * objToAttract.mass) / Mathf.Pow(distance, 2);
            Vector3 force = direction.normalized * Mathf.Clamp(forceMagnitude, 0f, 1000f);

            objToAttract.GetComponent<Rigidbody>().AddForce(force);
        }

    }
}