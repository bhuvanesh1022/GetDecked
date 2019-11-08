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
    public bool cancalculate;
    public int health;
    public Text healthtext;
    public bool isvaluechanged;
    public bool isvaluechangedtothisplayer;
    public bool iscalculating;
    public bool nextturn;
    public bool isnothingchanged;
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
        Health();
        Healthvisual();
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
      /*  if (cancalculate)
        {
            Manager.manager.visualtext.GetComponent<Text>().text = "Turn Ends";

        }*/
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
            
            cancalculate = true;
           
            
           
        }
        if(cancalculate)
        {
            isvisualenabled = true;
            yield return new WaitForSeconds(2f);
           
            cancalculate = false;
            iscalculating = true;
          

        }
        if(isvaluechangedtothisplayer)
        {
            isvisualenabled = true;
            yield return new WaitForSeconds(2f);
            isvisualenabled = false;
            isvaluechangedtothisplayer = false;
            isvaluechanged = true;

        }
        if(isvaluechanged)
        {
            isvisualenabled = true;
            yield return new WaitForSeconds(2f);
            isvaluechanged = false;
            isvisualenabled = false;
            for (int j = 0; j < Manager.manager.playerlist.Count; j++)
            {
                Manager.manager.playerlist[j].GetComponent<Playerobject>().nextturn = true;
            }
        }
        
        if (isnothingchanged)
        {
            isvisualenabled = true;
            yield return new WaitForSeconds(2f);
            isnothingchanged = false;
            isvaluechanged = false;
            isvaluechangedtothisplayer = false;
            isvisualenabled = false;
            for (int j = 0; j < Manager.manager.playerlist.Count; j++)
            {
                Manager.manager.playerlist[j].GetComponent<Playerobject>().nextturn = true;
            }
        }
        if(nextturn)
        {
            isvisualenabled = true;
           
            yield return new WaitForSeconds(2f);
            Manager.manager.canplay = true;
            isvisualenabled = false ;
            nextturn = false;
            for (int j = 0; j < Manager.manager.playerlist.Count; j++)
            {
                Manager.manager.playerlist[j].GetComponent<Playerobject>().cardisplacedbyplayer = false;
            }

           

                   


                

            

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






    public void Health()
    {
        if (Manager.manager == null)
        {
            Manager.manager = GameObject.Find("Manager").GetComponent<Manager>();
        }
        if (iscalculating)
        {
            

            for (int i = 0; i < Manager.manager.placedcardlist.Count; i++)
            {
                for (int j = i; j < Manager.manager.placedcardlist.Count; j++)
                {
                    for (int a = 0; a < Manager.manager.playerlist.Count; a++)
                    {
                       

                        if (Manager.manager.placedcardlist[i].GetComponent<Card>().cardvalue == 1 && Manager.manager.placedcardlist[j].GetComponent<Card>().cardvalue == 3)
                        {
                            Debug.Log("Attack beats");
                            if (!Manager.manager.placedcardlist[i].GetComponent<Card>().photonView.IsMine && Manager.manager.playerlist[a].GetComponent<Playerobject>().photonView.IsMine)
                            {
                                Debug.Log(Manager.manager.playerlist[a].GetComponent<Playerobject>().username + "reduces");
                                Manager.manager.playerlist[a].GetComponent<Playerobject>().health -= 1;
                                Manager.manager.playerlist[a].GetComponent<Playerobject>().healthtext.text = Manager.manager.playerlist[a].GetComponent<Playerobject>().health.ToString();
                             
                                Manager.manager.playerlist[a].GetComponent<Playerobject>().isvaluechangedtothisplayer = true;

                            }
                        }
                        else if (Manager.manager.placedcardlist[j].GetComponent<Card>().cardvalue == 1 && Manager.manager.placedcardlist[i].GetComponent<Card>().cardvalue == 3)
                        {
                            Debug.Log("Attack beats");
                            if (!Manager.manager.placedcardlist[j].GetComponent<Card>().photonView.IsMine && Manager.manager.playerlist[a].GetComponent<Playerobject>().photonView.IsMine)
                            {
                                Debug.Log(Manager.manager.playerlist[a].GetComponent<Playerobject>().username + "reduces");
                                Manager.manager.playerlist[a].GetComponent<Playerobject>().health -= 1;
                                Manager.manager.playerlist[a].GetComponent<Playerobject>().healthtext.text = Manager.manager.playerlist[a].GetComponent<Playerobject>().health.ToString();
                                Manager.manager.visualtext.GetComponent<Text>().fontSize = 57;
                               
                                Manager.manager.playerlist[a].GetComponent<Playerobject>().isvaluechangedtothisplayer = true;
                            }
                        }
                        else if (Manager.manager.placedcardlist[i].GetComponent<Card>().cardvalue == 1 && Manager.manager.placedcardlist[j].GetComponent<Card>().cardvalue == 2)
                        {
                            Debug.Log("defend beats");
                            if (!Manager.manager.placedcardlist[j].GetComponent<Card>().photonView.IsMine && Manager.manager.playerlist[a].GetComponent<Playerobject>().photonView.IsMine)
                            {
                                Debug.Log(Manager.manager.playerlist[a].GetComponent<Playerobject>().username + "reduces");
                                Manager.manager.playerlist[a].GetComponent<Playerobject>().health -= 1;
                                Manager.manager.playerlist[a].GetComponent<Playerobject>().healthtext.text = Manager.manager.playerlist[a].GetComponent<Playerobject>().health.ToString();
                                Manager.manager.visualtext.GetComponent<Text>().fontSize = 57;
                              
                                Manager.manager.playerlist[a].GetComponent<Playerobject>().isvaluechangedtothisplayer = true;
                            }
                        }
                        else if (Manager.manager.placedcardlist[j].GetComponent<Card>().cardvalue == 1 && Manager.manager.placedcardlist[i].GetComponent<Card>().cardvalue == 2)
                        {
                            Debug.Log("defend beats");
                            if (!Manager.manager.placedcardlist[i].GetComponent<Card>().photonView.IsMine && Manager.manager.playerlist[a].GetComponent<Playerobject>().photonView.IsMine)
                            {
                                Debug.Log(Manager.manager.playerlist[a].GetComponent<Playerobject>().username + "reduces");
                                Manager.manager.playerlist[a].GetComponent<Playerobject>().health -= 1;
                                Manager.manager.playerlist[a].GetComponent<Playerobject>().healthtext.text = Manager.manager.playerlist[a].GetComponent<Playerobject>().health.ToString();
                                Manager.manager.visualtext.GetComponent<Text>().fontSize = 57;
                               
                                Manager.manager.playerlist[a].GetComponent<Playerobject>().isvaluechangedtothisplayer = true;
                            }
                        }
                        else if (Manager.manager.placedcardlist[i].GetComponent<Card>().cardvalue == 2 && Manager.manager.placedcardlist[j].GetComponent<Card>().cardvalue == 3)
                        {
                            Debug.Log("throw beats");
                            if (!Manager.manager.placedcardlist[j].GetComponent<Card>().photonView.IsMine && Manager.manager.playerlist[a].GetComponent<Playerobject>().photonView.IsMine)
                            {
                                Debug.Log(Manager.manager.playerlist[a].GetComponent<Playerobject>().username + "reduces");
                                Manager.manager.playerlist[a].GetComponent<Playerobject>().health -= 1;
                              
                                Manager.manager.playerlist[a].GetComponent<Playerobject>().healthtext.text = Manager.manager.playerlist[a].GetComponent<Playerobject>().health.ToString();
                                Manager.manager.visualtext.GetComponent<Text>().fontSize = 57;
                               
                                Manager.manager.playerlist[a].GetComponent<Playerobject>().isvaluechangedtothisplayer = true;
                            }
                        }

                        else if (Manager.manager.placedcardlist[j].GetComponent<Card>().cardvalue == 2 && Manager.manager.placedcardlist[i].GetComponent<Card>().cardvalue == 3)
                        {
                            Debug.Log("throw beats");
                            if (!Manager.manager.placedcardlist[i].GetComponent<Card>().photonView.IsMine && Manager.manager.playerlist[a].GetComponent<Playerobject>().photonView.IsMine)
                            {
                                Debug.Log(Manager.manager.playerlist[a].GetComponent<Playerobject>().username + "reduces");
                                Manager.manager.playerlist[a].GetComponent<Playerobject>().health -= 1;

                                Manager.manager.playerlist[a].GetComponent<Playerobject>().healthtext.text = Manager.manager.playerlist[a].GetComponent<Playerobject>().health.ToString();
                                Manager.manager.visualtext.GetComponent<Text>().fontSize = 57;
                                Manager.manager.playerlist[a].GetComponent<Playerobject>().isvaluechangedtothisplayer = true;
                              
                            }
                        }
                       

                    }
                }
            }
            iscalculating = false ;
            Manager.manager.placedcardlist = new List<GameObject>();
        }
    }

    public void Healthvisual()
    {
        if (cancalculate)
        {
            Manager.manager.visualtext.GetComponent<Text>().fontSize = 57;

            Manager.manager.visualtext.GetComponent<Text>().text = "Turn Ends";
        }

        for (int i = 0; i < Manager.manager.playerlist.Count; i++)
        {
            if (Manager.manager.playerlist[i].GetComponent<Playerobject>().isvaluechangedtothisplayer &&photonView.IsMine)
            {
                Manager.manager.visualtext.GetComponent<Text>().text = Manager.manager.playerlist[i].GetComponent<Playerobject>().username + " Loses";
            }
            if (Manager.manager.playerlist[i].GetComponent<Playerobject>().isvaluechanged && photonView.IsMine)
            {
                Manager.manager.visualtext.GetComponent<Text>().text = Manager.manager.playerlist[i].GetComponent<Playerobject>().username + " Health reduces to " + Manager.manager.playerlist[i].GetComponent<Playerobject>().health.ToString();
               
            }
            if(Manager.manager.playerlist[i].GetComponent<Playerobject>().isnothingchanged)
            {
                Manager.manager.visualtext.GetComponent<Text>().text = " Same Cards";
            }
            if(nextturn)
            {
                Manager.manager.visualtext.GetComponent<Text>().text = "Next turn";

                for (int c= 0; c < Manager.manager.cardlist.Count; c++)
                {
                    if (Manager.manager.cardlist[c].GetComponent<Card>().isplaced)
                    {
                       
                        Manager.manager.cardlist[c].transform.position = Manager.manager.cardlist[c].GetComponent<Card>().startpos;
                        Manager.manager.cardlist[c].GetComponent<Card>().isplaced = false;
                    }
                   
                }
            }
           
        }


       
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
       if(stream.IsWriting)
        {
            stream.SendNext(playerindex);
            stream.SendNext(health);
            stream.SendNext(cancalculate);
            stream.SendNext(cardisplacedbyplayer);
            stream.SendNext(healthtext.text);
            stream.SendNext(isvaluechangedtothisplayer);
            stream.SendNext(isvaluechanged);
          
        }else if(stream.IsReading)
        {
            playerindex = (int)stream.ReceiveNext();
            health = (int)stream.ReceiveNext();
           cancalculate = (bool)stream.ReceiveNext();
            cardisplacedbyplayer = (bool)stream.ReceiveNext();
            healthtext.text = (string)stream.ReceiveNext();
            isvaluechangedtothisplayer = (bool)stream.ReceiveNext();
            isvaluechanged = (bool)stream.ReceiveNext();
          
        }
    }
}
