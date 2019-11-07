using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
public class Createroom : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text Roomname;
    private Roomcanvases roomcanvases;

    public Connectiontest connectiontest;
    public Canvas datacanvas;

  
    [SerializeField]
    private Text valuetex;
   
    private void Start()
    {
        Mastermanager._gamesettings.playerenteredindex = -1;
        //  roomcanvases.LevelSelect.levelselected = false;

        
        
        
    }
    public void Firstinitialize(Roomcanvases canvases)
    {
        roomcanvases = canvases;
    }
    public void OnclickCreatebutton()
    {
        if (!PhotonNetwork.IsConnected)
            return;

     

        RoomOptions option = new RoomOptions();
        option.MaxPlayers = 4;
        if (string.IsNullOrEmpty(Roomname.text))
        {
            print("name required");
            valuetex.color = Color.red;
            valuetex.text = "NAME REQUIRED";
            return;
        }
      



        else
        {
            valuetex.text = "";




        }
     
        PhotonNetwork.JoinOrCreateRoom(Roomname.text, option, TypedLobby.Default);
      

        Debug.Log(Roomname + "roomname");




    }


    public override void OnJoinedRoom()
    {
        Debug.Log("Joined " + Roomname.text);
        if (PhotonNetwork.IsMasterClient)
        {
            roomcanvases.currentroomcanvas.Show();
      
        }
        else
        {
            roomcanvases.currentroomcanvas.Show();
         
        }








    }
  /*  public void Onclickgotogame()
    {
        roomcanvases.currentroomcanvas.Show();
      
       // roomcanvases.LevelSelect.LevelCanvas.sortingOrder = 3;
        roomcanvases.currentroomcanvas.canvases.currentroomcanvases.sortingOrder = 3;
        //  roomcanvases.currentroomcanvas.canvases.Hide();
        roomcanvases.currentroomcanvas.canvases.Datacanvas.sortingOrder = 0;



    }*/

    public override void OnCreateRoomFailed(short returnCode, string message)
    {

        valuetex.text = "Create room";

        Debug.Log("Joined failed " + returnCode + "" + message);

    }





}
