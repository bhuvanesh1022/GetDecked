using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class Playerlisting : MonoBehaviourPunCallbacks
{   
    [SerializeField]
    private Transform content;
    [SerializeField]
    private PlayerList player;
    public List<PlayerList> playerlist = new List<PlayerList>();
    public float f;
    
    private Roomcanvases roomcanvases;
    [SerializeField]
    private Text readytext;
    public bool ready;
    public Button readycolor;
    public PhotonView pv;
    public GameObject startbutton;
    public GameObject leaveroombutton;
    public Text readybuttoninfotext;
    public GameObject readybutton;
    public void Firstinitialize(Roomcanvases canvases)
    {
        roomcanvases = canvases;
    }
    private void Awake()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            startbutton.SetActive(false);
            leaveroombutton.GetComponent<Button>().interactable = false;
            // readybuttoninfotext.text = "Click to Confirm";
         
        }
        else
        {
            //  readybuttoninfotext.text ="Waiting for opponent to confirm";
            readybutton.SetActive(false);
        }
       
        GetCurrentroomplayers();
       
    }

    void SetReadytext(bool state)
    {
        ready = state;
        if(ready)
        { 
            readytext.text = "READY";
            readycolor.GetComponent<RawImage>().color = Color.green;
        }
        else
        {
            readycolor.GetComponent<RawImage>().color = Color.red;
            readytext.text = "Click to confirm";
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SetReadytext(false);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        for (int i = 0; i < playerlist.Count; i++)
        {
            Destroy(playerlist[i].gameObject);
            playerlist.Clear();
        }
    }
    public override void OnLeftRoom()
    {
        content.Destroychildren();
    }

    void GetCurrentroomplayers()
    {
        if (!PhotonNetwork.IsConnected)
            return;
        if (PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.Players == null)
            return;
        foreach (KeyValuePair<int,Player> playerinfo in PhotonNetwork.CurrentRoom.Players )
        {
            AddPlayerList(  playerinfo.Value);
         
        }
        Enteredplayernumber();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        roomcanvases.currentroomcanvas.LeaveroomMenu.Onclickleaveroom();
    }
    void AddPlayerList(Player playersname)
    {
       
        int index = playerlist.FindIndex(x => x.playersobj == playersname);
        if (index != -1)
        {  
            playerlist[index].SetPlayerinfo(playersname);
            Debug.Log(index + "ind1");
        }
        else
        {  
            PlayerList listing = Instantiate(player, content);

            if (listing != null)
            {  
                playerlist.Add(listing);
              
                listing.SetPlayerinfo(playersname);
                Debug.Log(index + "ind2");
            }
        }
    }
   
    public void Enteredplayernumber()
    {
        Mastermanager._gamesettings.playerenteredindex = playerlist.Count-1 ;
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {  
        AddPlayerList(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = playerlist.FindIndex(x => x.playersobj == otherPlayer);
        if (index != -1)
        {
            Debug.Log(index + "index");
            Destroy(playerlist[index].gameObject);
           playerlist.RemoveAt(index);
            
        }
    }
   
    public void OnclickStartgame()
    {
            for (int i = 0; i < playerlist.Count; i++)
            {
                if (playerlist[i].playersobj != PhotonNetwork.LocalPlayer)
                {
                    if (!playerlist[i].Ready)
                        return;
            }

            Mastermanager._gamesettings.Loaded = false;
        }
        PhotonNetwork.LoadLevel(1);
        PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
        Mastermanager._gamesettings.Loaded = true;
    }

    public void Onclickreadybutton()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            SetReadytext(!ready);
            base.photonView.RPC("Changereadyfunction", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer, ready);
        }
    }

    [PunRPC]
    void Changereadyfunction(Player player,bool Isready)
    {
        int index = playerlist.FindIndex(x => x.playersobj == player);
        if (index != -1)
         
            playerlist[index].Ready = Isready;
        if (Isready)
        {
            readycolor.GetComponent<RawImage>().color = Color.green;
            readytext.text = "READY";
            ready = Isready;
         
        }
        else
        {
            readycolor.GetComponent<RawImage>().color = Color.red;
            readytext.text = "Wait";
            ready = Isready;
        }
    }

 public void Readytextinfo(Player player, bool Isready)
    {
        int index = playerlist.FindIndex(x => x.playersobj == player);
        if (index != -1)

            playerlist[index].Ready = Isready;
        ready = Isready;
        if (PhotonNetwork.IsMasterClient)
        {
            if (Isready)
            {
               
                readybutton.SetActive(false);
                readybuttoninfotext.text = "Click Start to Play";
            }
            else
            {
                readybutton.SetActive(false);
                readybuttoninfotext.text = "Waiting for opponent to confirm";
            }
        }
        else 
        {
            if (Isready)
            {
                readybutton.SetActive(false);
                readybuttoninfotext.text = "Waiting For opponent to start the game";
            }
            
        }
    }

    private void Update()
    {
        Readytextinfo(PhotonNetwork.LocalPlayer, ready);
    }
}

