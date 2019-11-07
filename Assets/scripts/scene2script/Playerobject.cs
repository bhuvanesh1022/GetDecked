using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class Playerobject : MonoBehaviourPunCallbacks,IPunObservable
{
    public int playerindex;
    public string username;
    public bool Gamestarted;
    public bool isvisualenabled;
    public Text usernametext;
    public float currenttime;
    public float maxtime;
    public float calculatedtime;
    public Image timerimage;
    public bool cardisplacedbyplayer;
    private void Start()
    {
        Startbuttonactive();
        Playerposition();
        
        Playerdetails();
        photonView.RPC("Addplayers", RpcTarget.AllBuffered, null);
        Manager.manager.startbutton.GetComponent<Button>().onClick.AddListener(Onclickstartbutton);
    }


    private void Update()
    {
        Visualinformation();
        Visualtext();
        StartCoroutine("Wait");
        Timer();
        Iscardplacedbyplayer();
    }


    //positioning player
    void Playerposition()
    {
        if (Manager.manager == null)
        {
            Manager.manager = GameObject.Find("Manager").GetComponent<Manager>();
        }
        if (photonView.IsMine)
        {
            transform.position = Manager.manager.playerposition[0].transform.position;
        }
        else
        {
            transform.position = Manager.manager.playerposition[1].transform.position;
        }
    }




    //player details
    void Playerdetails()
    {
        if (photonView.IsMine)
        {
            username = Mastermanager._gamesettings.Nickname;
            this.gameObject.name = Mastermanager._gamesettings.Nickname;
            playerindex = Mastermanager._gamesettings.playerenteredindex;
            usernametext.text = "YOU";
            currenttime = maxtime;
        }
        else
        {
            username = photonView.Owner.NickName;
            this.gameObject.name = photonView.Owner.NickName;
            usernametext.text = this.gameObject.name;
        }
    }


    //adding the player to the list
    [PunRPC]
    public void Addplayers()
    {
        if (Manager.manager == null)
        {
            Manager.manager = GameObject.Find("Manager").GetComponent<Manager>();
        }
        if (Manager.manager.playerlist.Contains(this.gameObject))
            return;
        Manager.manager.playerlist.Add(this.gameObject);
      


    }





    //start button active for masterclient
    public void Startbuttonactive()
    {
        if (Manager.manager == null)
        {
            Manager.manager = GameObject.Find("Manager").GetComponent<Manager>();
        }
        if (PhotonNetwork.IsMasterClient)
        {
            Manager.manager.startbutton.SetActive(true);
        }
    }


    //on click start button
    public void Onclickstartbutton()
    {

        photonView.RPC("StartBool", RpcTarget.AllBuffered, null);
        Manager.manager.startbutton.SetActive(false);
    }

    //passsing bool if true game starts or wait for masterclient to start
    [PunRPC]
    public void StartBool()
    {
        Gamestarted = true;
    }




   void Visualinformation()
    {
     
    if(isvisualenabled)
        {
            Manager.manager.visualtext.SetActive(true);
        }
        else
        {
            Manager.manager.visualtext.SetActive(false);
        }
       
    }

    void Visualtext()
    {
        if(Gamestarted && isvisualenabled)
        {
            Manager.manager.visualtext.GetComponent<Text>().text = "LeTZzzzzz PlAy";
          
        }
        if(Manager.manager.placedcardlist.Count==2)
        {
            Manager.manager.visualtext.GetComponent<Text>().text = "Turn Ends";
        }
    }
 
    IEnumerator Wait()
    {
        if(Gamestarted)
        {
            isvisualenabled = true;
            yield return new WaitForSeconds(1.5f);
            Gamestarted = false;
            isvisualenabled = false;
           Manager.manager. canplay = true;
        }
        if(Manager.manager.placedcardlist.Count==2)
        {
            isvisualenabled = true;
            yield return new WaitForSeconds(2f);
            Manager.manager.placedcardlist = new List<GameObject>();
            isvisualenabled = false;
        }
    }

    void Timer()
    {
        if (photonView.IsMine)
        {
            if (Manager.manager.canplay &&!cardisplacedbyplayer)
            {
                currenttime -= Time.deltaTime;
                calculatedtime = currenttime / maxtime;
                timerimage.fillAmount = calculatedtime;
            }
            if(currenttime<0 ||cardisplacedbyplayer)
            {
                Manager.manager.canplay = false;
                cardisplacedbyplayer = true;
                currenttime = maxtime;
                calculatedtime = 1;
                timerimage.fillAmount = calculatedtime;
            }
        }
    }




    void Iscardplacedbyplayer()
    {
        if (Manager.manager == null)
        {
            Manager.manager = GameObject.Find("Manager").GetComponent<Manager>();
        }
        if (photonView.IsMine)
        {
            for (int i = 0; i < Manager.manager.cardlist.Count; i++)
            {
                if (Manager.manager.cardlist[i].GetComponent<Card>().isplaced && Manager.manager.cardlist[i].GetComponent<Card>().photonView.IsMine)
                {
                    cardisplacedbyplayer = true;
                }
               
            }

        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
       if(stream.IsWriting)
        {
            stream.SendNext(playerindex);
        }else if(stream.IsReading)
        {
            playerindex = (int)stream.ReceiveNext();
        }
    }
}
