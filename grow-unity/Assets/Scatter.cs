using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scatter : MonoBehaviour
{
    public ParticleSystem ps;
    public AudioClip impactSound;

    public void Explode(Vector3 pos)
    {
        ps.transform.position = pos;
        ps.GetComponent<AudioSource>().PlayOneShot(impactSound);
        ps.Play();
    }
}

