using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scatter : MonoBehaviour
{
    public AudioClip impactSound;

    public void Explode(Vector3 pos)
    {
        transform.position = pos;
        GetComponent<AudioSource>().PlayOneShot(impactSound);
        GetComponent<ParticleSystem>().Play();
    }
}

