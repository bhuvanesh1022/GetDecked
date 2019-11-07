using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class Userscript : MonoBehaviour,IPunObservable
{
    public static Userscript userscript;

    public InputField userinput;
    public string user;
    public Canvas usercanvas;
    void Start()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            PhotonNetwork.LeaveRoom();
        }
            usercanvas.sortingOrder = 3;
            FindObjectOfType<Audiomanager>().Play(0);

        


    }
   public void Onclickuserbutton()
    {
       user = userinput.text;
        Mastermanager._gamesettings.Nickname = user;
        PhotonNetwork.LocalPlayer.NickName = Mastermanager._gamesettings.Nickname;
        usercanvas.sortingOrder = 1;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
     if(stream.IsWriting)
        {
         stream.SendNext(user);
        }if(stream.IsReading)
        {
            user= (string)stream.ReceiveNext();
        }
    }
    void Update()
    {
     
            
    }
}
