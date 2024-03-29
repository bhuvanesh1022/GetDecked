﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound 
{
    public string soundname;
    public int audionumber;
    public AudioClip clip;
    [Range(0,1f)]
    public float volume;

    [Range(0.1f,3f)]
    public float pitch;
    [HideInInspector]
    public AudioSource source;
    public bool loop;
   
}
