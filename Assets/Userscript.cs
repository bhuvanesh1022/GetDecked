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
    public Button userNameInput;
    public GameObject userNamePanel;
    public GameObject RoomListingPanel;
    
    void Start()
    {
        RoomListingPanel.transform.localScale = new Vector3(0f, 0f, 0f);
        if (PhotonNetwork.CurrentRoom != null)
        {
            PhotonNetwork.LeaveRoom();
        }
        FindObjectOfType<Audiomanager>().Play(0);
    }

    public void Update()
    {
        if (userinput.text.Length < 2)
        {
            userNameInput.interactable = false;
            userNameInput.GetComponentInChildren<Text>().enabled = false;
        }
        else
        {
            userNameInput.interactable = true;
            userNameInput.GetComponentInChildren<Text>().enabled = true;
        }
    }

    public void Onclickuserbutton()
    {
        user = userinput.text;
        Mastermanager._gamesettings.Nickname = user;
        PhotonNetwork.LocalPlayer.NickName = Mastermanager._gamesettings.Nickname;
        userNamePanel.SetActive(false);
        RoomListingPanel.transform.localScale=new Vector3(1,1,1);
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
}
