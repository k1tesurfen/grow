using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleVisual : MonoBehaviour
{
    public ParticleSystem init;
    public ParticleSystem loop;

    private void OnEnable()
    {
        init.Play();
        StartCoroutine(JanDelay(0.2f));
    }

    IEnumerator JanDelay(float time)
    {
        yield return new WaitForSeconds(time);
        loop.Play();
    }

}
