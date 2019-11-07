using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
[CreateAssetMenu(menuName = "Leveldata")]

public class Gamesettings : ScriptableObject
{
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
       
    }

    [SerializeField]
    private string gameversion = "0.0.1";
    public string Gameversion
    {
        get
        {
            return gameversion;
        }
    }
    [SerializeField]
    private string nickname;
    public string Nickname
    {
        get
        {
           
            return nickname;
           
        }
        set
        {
            nickname = value;
        }
       
      
       
    }


   
    [SerializeField]
    private bool loaded = false;
    public bool Loaded
    {
        get
        {
            return loaded;
        }
        set
        {
            loaded = value;
        }
    }

    public int playerenteredindex;

    public int attaclvalue;
    public int maxvalue;
    public int minvalue;
    public int defensevalue;

   public void Value()
    {
        attaclvalue = Random.Range(minvalue, maxvalue);
        defensevalue = Random.Range(minvalue, maxvalue);
    }
}
