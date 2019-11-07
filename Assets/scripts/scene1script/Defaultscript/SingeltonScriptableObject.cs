using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class SingrltonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    private static T instance;

    // Start is called before the first frame update
    public static T _instance {

        get
        {
            if (instance == null)
            {
                T[] results = Resources.FindObjectsOfTypeAll<T>();
               if(results.Length==0)
                {
                    Debug.LogError("Not found");
                    return null;
                }
               if(results.Length>1)
                {
                    Debug.Log("Found");
                    return null;
                }
               
                instance = results[0];
            }
            return instance;
        }
       
    } }
