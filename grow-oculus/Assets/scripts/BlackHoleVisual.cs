using System.Collections;
using UnityEngine;

public class BlackHoleVisual : MonoBehaviour
{
    public ParticleSystem init;
    public ParticleSystem loop;

    public void StartVisuals()
    {
        StopVisuals();
        init.Play();
        StartCoroutine(JanDelay(0.2f));
    }

    public void StopVisuals()
    {
        init.Stop();
        loop.Stop();
    }

    IEnumerator JanDelay(float time)
    {
        yield return new WaitForSeconds(time);
        loop.Play();
    }

}
