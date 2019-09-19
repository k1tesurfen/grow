using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attractor : MonoBehaviour
{
    const float G = 66.74f;

    public int mass;
    public static List<Attractor> Attractors;
    public bool isBlackHole = false;
    public bool attractable = false;
    private float lifeSpan = 300f;

    void FixedUpdate(){
        foreach(Attractor at in Attractors){
            if(at != this && this.isBlackHole && at.attractable) 
                Attract(at);
        }
    }

    void Update(){
        if(isBlackHole){
            if(lifeSpan < 0f)
                Destroy(gameObject);

            lifeSpan -= Time.deltaTime;
        }
    }

    void OnEnable(){
        if(Attractors == null)
            Attractors = new List<Attractor>();

        Attractors.Add(this);
    }

    void OnDisable(){
        Attractors.Remove(this);
    }

    void Attract(Attractor objToAttract){

        Vector3 direction = transform.position - objToAttract.transform.position;
        float distance = direction.magnitude;

        if(distance < 1f){
            return;
        }

            float forceMagnitude = G * (this.mass * objToAttract.mass) / Mathf.Pow(distance, 2);
            Vector3 force = direction.normalized * forceMagnitude;

            objToAttract.GetComponent<Rigidbody>().AddForce(force);
        
    }
}
