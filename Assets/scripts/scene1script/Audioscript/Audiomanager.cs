using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audiomanager : MonoBehaviour
{
    public Sound[] sounds;
    public static Audiomanager instance;

    private void Awake()
    {
      
        if(instance==null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
        foreach (Sound s in sounds)
        {
                s.source = gameObject.AddComponent<AudioSource>();
           
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
           
        }
    }

 public   void Play(int audionum)
    {
        Sound s = Array.Find(sounds, sound => sound.audionumber == audionum);
        Debug.Log(s + "s");
        if (s==null)
        {
           
            Debug.Log("no audio to play");
            return;
           
        }
        s.source.Play();
        Debug.Log(s + "s");
    }
    public void Stop(int audionum)
    {
        Sound s = Array.Find(sounds, sound => sound.audionumber == audionum);
        Debug.Log(s + "s");
        if (s == null)
        {

            Debug.Log("no audio to play");
            return;

        }
        s.source.Stop();
    }
}
