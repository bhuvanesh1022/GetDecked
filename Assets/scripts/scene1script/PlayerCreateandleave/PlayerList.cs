using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Sprites;
using Photon.Pun;

public class PlayerList : MonoBehaviour
{
    [SerializeField]
    private Text _text;
    [SerializeField]
    private Image imge;
   

    [SerializeField]
    private List<Image> avatar = new List<Image>();
    public Player playersobj
    {get;
        private set;
    }

  
    public float f;

    public bool Ready = false;

    private void Start()
    {
        imge=GetComponent<Image>();
       
      
    }

    public void SetPlayerinfo(Player player)
{
         playersobj = player;
        _text.text = player.NickName;
        Debug.Log(playersobj+ "nick");

      
     //   Mastermanager._gamesettings.Charselect = index;
      
      


       

        



    }



}
