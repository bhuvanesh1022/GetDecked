using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Connectiontest : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text infotext;
    public Text InfoText
    {
        get
        {
            return infotext;
        }
    }
    public Button createbutton;
  
    private void Start()
    {
        // Mastermanager._gamesettings.Nickname = "USER";

     
        infotext.color = new Color(255, 255, 255);
     //   PhotonNetwork.LeaveRoom();
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = Mastermanager._gamesettings.Gameversion;
        PhotonNetwork.LocalPlayer.NickName = Mastermanager._gamesettings.Nickname;
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + "inlobbylocalname");
        infotext.text = "Connecting.......";
        print("Connecting...........");
        createbutton.interactable = false;









        if (PhotonNetwork.IsConnected)
        {
            OnConnectedToMaster();
        }
        else
        {

            PhotonNetwork.ConnectUsingSettings();
        }


    }


    public override void OnConnectedToMaster()
    {
        
        print("connected");
        infotext.color = new Color(0, 255, 0);
        infotext.text = "connected.......";

        if (!PhotonNetwork.InLobby)
        {

            PhotonNetwork.JoinLobby();
            createbutton.interactable = true;
        }
      
      

    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        if(Userscript.userscript==null)
        {
            Userscript.userscript = FindObjectOfType<Userscript>();
        }
        infotext.text = "Disconnected retry";
        Userscript.userscript.usercanvas.sortingOrder = 3;
        print("Disconnected due to " + cause.ToString());
       
    }
}
