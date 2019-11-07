using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="Mastermanager")]
public class Mastermanager : SingrltonScriptableObject<Mastermanager>
{
    [SerializeField]
    private Gamesettings gamesettings;

    public static Gamesettings _gamesettings
    {

        get
        {
            return _instance.gamesettings;
        }
    }
   
  
    }
