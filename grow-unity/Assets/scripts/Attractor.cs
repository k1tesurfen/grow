using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    const float G = 66.74f;

    public int mass;
    public float rotateSpeed = 200f;
    public static List<Attractor> Attractables;
    public bool isAttractor = false;
    public bool isPreAttractor = false;
    public bool doAttract = false;
    private float lifeSpan = 500f;

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
        if (distance < 1f)
        {
            //objToAttract.GetComponent<Rigidbody>().useGravity = false;
            return;
        }
        if (distance < 2f)
        {
            if (isPreAttractor)
            {
                Destroy(gameObject);
            }
        }

        float forceMagnitude = G * (mass * objToAttract.mass) / Mathf.Pow(distance, 2);
        Vector3 force = direction.normalized * Mathf.Clamp(forceMagnitude, 0f, 1000f);

        objToAttract.GetComponent<Rigidbody>().AddForce(force);
    }
}
