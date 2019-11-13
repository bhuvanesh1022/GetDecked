using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Roomlist : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Text _text;

    [SerializeField]
    private Roomcanvases canvases;
    public RoomInfo rominfo
    {
        get;

        private set;
        

        }
    void Firstinitialize(Roomcanvases roomcanvases)
    {
        canvases = roomcanvases;
    }

    public void SetRoominfo(RoomInfo roominfo)
{
    rominfo = roominfo;
        _text.text = roominfo.Name;
    }
    public void Onclickutton()
    {
        /*  if (canvases.CreateRoom.number.Charselect == false)
              return;*/
      
        PhotonNetwork.JoinRoom(rominfo.Name);
    }
}
