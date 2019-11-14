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
    [SerializeField] private Button createRoomBtn;
    public Connectiontest connectiontest;
    public Canvas datacanvas;
    [SerializeField] private GameObject RoomListingPanel;
    [SerializeField] private GameObject CurrentRoomPanel;

    [SerializeField]
    private Text valuetex;
   
    private void Start()
    {
        Mastermanager._gamesettings.playerenteredindex = -1;
        //  roomcanvases.LevelSelect.levelselected = false;
    }

    public void Update()
    {
        if (Roomname.text.Length < 2)
        {
            createRoomBtn.interactable = false;
            createRoomBtn.GetComponentInChildren<Text>().enabled = false;
        }
        else
        {
            createRoomBtn.interactable = true;
            createRoomBtn.GetComponentInChildren<Text>().enabled = true;
        }
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
        option.MaxPlayers = 2;

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

        RoomListingPanel.transform.localScale=new Vector3(0f,0f,0f);
        CurrentRoomPanel.SetActive(true);

        //if (PhotonNetwork.IsMasterClient)
        //{
        //    roomcanvases.currentroomcanvas.Show();
        //}
        //else
        //{
        //    roomcanvases.currentroomcanvas.Show();
        //}
    }

    /*  
    public void Onclickgotogame()
    {
        roomcanvases.currentroomcanvas.Show();
      
       // roomcanvases.LevelSelect.LevelCanvas.sortingOrder = 3;
        roomcanvases.currentroomcanvas.canvases.currentroomcanvases.sortingOrder = 3;
        //  roomcanvases.currentroomcanvas.canvases.Hide();
        roomcanvases.currentroomcanvas.canvases.Datacanvas.sortingOrder = 0;
    }
    */

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        valuetex.text = "Create room";

        Debug.Log("Joined failed " + returnCode + "" + message);
    }
}
