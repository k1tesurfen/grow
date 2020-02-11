using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    //TODO: 6 different audioclips to play before each condition.
    //      2 extra audioclips describe the task to do 

    public Sound[] sounds;

    private void Awake()
    {
        //populate audiomanager with audiosources.
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.loop = s.loop;
        }
    }

    //plays sound with name name
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    public void Play(string[] name)
    {
        float length = 0f;
        foreach(string s in name)
        {
            Sound clip = Array.Find(sounds, sound => sound.name == s);
            StartCoroutine(JanDelay(length, clip));
            length += clip.source.clip.length + 2f;
        } 
    }

    IEnumerator JanDelay(float time, Sound s)
    {
        yield return new WaitForSeconds(time);
        s.source.Play();
    }
}
