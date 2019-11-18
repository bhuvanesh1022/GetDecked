﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class Playerobject : MonoBehaviourPunCallbacks, IPunObservable
{
    public int playerindex;
    public string username;
    public bool Gamestarted;
    public bool isvisualenabled;
    public Text usernametext;
    public float currenttime;
    public float maxtime;
    public float calculatedtime;

    public bool cardisplacedbyplayer;
    public bool cancalculate;
    public int health;
    public Text healthtext;
    public bool isvaluechanged;
    public bool isvaluechangedtothisplayer;
    public bool iscalculating;
    public bool nextturn;
    public bool isnothingchanged;
    public bool isnotchanged;
    public bool isgameended;
    public bool isvisualgameended;

    public int wagevalue;
    public int wagetobet;
    public bool iswagebetted;
    public int opponentbetted;
    public bool iswagevisualbet;
    public int maxwage;
    public int opponentwage;
    public bool iswin;
    //wage
    public string winname;
    public bool istimeover;
    public bool istimeendsvisual;
    public bool isturnover;
    public bool timeendwin;
    public bool isbothplayed;
    public bool isbothplayedvisual;
    public bool isdraw;
    public bool isvisualdraw;
    private void Start()
    {

        Startbuttonactive();
        Playerposition();

        Playerdetails();
        photonView.RPC("Addplayers", RpcTarget.AllBuffered, null);
        Manager.manager.startbutton.GetComponent<Button>().onClick.AddListener(Onclickstartbutton);
        Manager.manager.increamentbutton.onClick.AddListener(Onclickplus);
        Manager.manager.decreamentbutton.onClick.AddListener(Onclickminus);
        Manager.manager.wagebetbutton.GetComponent<Button>().onClick.AddListener(Onclickwagebetbutton);
        Waitforopponent();
    }
    public void Waitforopponent()
    {
        if (!Gamestarted && PhotonNetwork.IsMasterClient)
        {
            isvisualenabled = true;
            Manager.manager.visualtext.GetComponent<Text>().text = "Press play to start";
        }
        else if (!Gamestarted && !PhotonNetwork.IsMasterClient)
        {
            isvisualenabled = true;
            Manager.manager.visualtext.GetComponent<Text>().text = "Wait for opponent to start";
        }
    }

    private void Update()
    {
        Wagevisual();
        Opponentbetted();
        Opponentvalues();
        Visualinformation();
        Visualtext();
        StartCoroutine("Wait");
        Timer();
        Iscardplacedbyplayer();
        Health();
      Timeover();
        Healthvisual();
        Wincondition();
        Tokenreset();
        Betvalues();


    }
    public void Wagevisual()
    {
        if (Manager.manager == null)
        {
            Manager.manager = GameObject.Find("Manager").GetComponent<Manager>();
        }



        if (Manager.manager.canplay && !Manager.manager.iswagebetted && photonView.IsMine)
        {
            iswagevisualbet = true;
            Manager.manager.opponentbettedtext.SetActive(false);
           

        }
        else if (Manager.manager.canplay && Manager.manager.iswagebetted && photonView.IsMine)
        {
            iswagevisualbet = false;
           
        }
        if (iswagevisualbet && photonView.IsMine)
        {
           
            isvisualenabled = true;
            Manager.manager.visualtext.GetComponent<Text>().text = "Place your bet" +
                "\n(Tap the green button number to confirm)";

        }
        if (photonView.IsMine)
        {
            for (int i = 0; i < Manager.manager.playerlist.Count; i++)
        {
           
                if (!cardisplacedbyplayer && Manager.manager.canplay && Manager.manager.iswagebetted)
                {
                   isvisualenabled = true;
                    Manager.manager.visualtext.GetComponent<Text>().text = "Place your card" + "\n (Drag to centre of the table)";
                }
                else if (cardisplacedbyplayer && Manager.manager.canplay && Manager.manager.iswagebetted &&!nextturn)
                {
                   // Manager.manager.playerlist[i].GetComponent<Playerobject>().isvisualenabled = false;
                }
            }

        }
       




        }
    public void Onclickplus()
    {
        if (photonView.IsMine && Manager.manager.canplay && !Manager.manager.iswagebetted && Manager.manager.betadjust < Manager.manager.maxwagevalue)
        {
            Manager.manager.betadjust++;
            Manager.manager.wagevaluetext.text = Manager.manager.betadjust.ToString();
        }
        if (photonView.IsMine && Manager.manager.canplay && !Manager.manager.iswagebetted)
        {
            if (Manager.manager.wagevalue < Manager.manager.maxwagevalue)
            {
                Manager.manager.wagevalue++;



                wagevalue = Manager.manager.wagevalue;


            }
        }
    }




    public void Onclickminus()
    {
        if (photonView.IsMine && Manager.manager.canplay && !Manager.manager.iswagebetted && Manager.manager.betadjust > 0)
        {
            Manager.manager.betadjust--;
            Manager.manager.wagevaluetext.text = Manager.manager.betadjust.ToString();
        }

        if (photonView.IsMine && Manager.manager.canplay && !Manager.manager.iswagebetted)
        {
            if (Manager.manager.wagevalue > 0)
            {
                Manager.manager.wagevalue--;

                wagevalue = Manager.manager.wagevalue;


            }
        }
    }




    public void Onclickwagebetbutton()
    {
       
        if (Manager.manager.canplay && !Manager.manager.iswagebetted)
        {

            Manager.manager.betadjust = 0;
            Manager.manager.wagevaluetext.text = Manager.manager.betadjust.ToString();
            Manager.manager.iswagebetted = true;
            iswagebetted = true;


            wagetobet = wagevalue;

            Manager.manager.maxwagevalue = Manager.manager.maxwagevalue - Manager.manager.wagevalue;
            maxwage = Manager.manager.maxwagevalue;
            Debug.Log(Manager.manager.maxwagevalue + "maxwage");
            // Manager.manager.wagevalue = Manager.manager.maxwagevalue;
            Debug.Log(Manager.manager.wagevalue + "managerwage");
            wagevalue = 0;
            Manager.manager.wagevalue = 0;
            Debug.Log(wagevalue + "wage");
            Manager.manager.wagevaluetext.text = Manager.manager.betadjust.ToString();
            Manager.manager.chiptext.text = Manager.manager.maxwagevalue.ToString();

           
        }
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
            currenttime = 0;
            Manager.manager.timerimage.SetActive(true);
            Manager.manager.wageobject.SetActive(true);
            Manager.manager.wagebetbutton.SetActive(true);
            wagevalue =0;
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
       
            if (isvisualenabled)
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
        if (Gamestarted && isvisualenabled)
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
        if (Gamestarted)
        {
            isvisualenabled = true;
            yield return new WaitForSeconds(1.5f);
            Gamestarted = false;
            // isvisualenabled = false;
            Manager.manager.canplay = true;
        }



        if (Manager.manager.placedcardlist.Count == 2)
        {

            cancalculate = true;




        }
        if (cancalculate)
        {


            isvisualenabled = true;

            yield return new WaitForSeconds(2f);
            for (int c = 0; c < Manager.manager.cardlist.Count; c++)
            {
                Manager.manager.cardlist[c].GetComponent<Card>().canshowvalues = true;
            }
            cancalculate = false;
            iscalculating = true;


        }

        if (isvaluechangedtothisplayer)
        {
            isvisualenabled = true;
            yield return new WaitForSeconds(2f);
            isvisualenabled = false;
            isvaluechangedtothisplayer = false;
            isvaluechanged = true;

        }
       
        
        if (isvaluechanged)
        {

            isvisualenabled = true;
            yield return new WaitForSeconds(2f);
            isvaluechanged = false;
            isvisualenabled = false;
            nextturn = true;

        }
        if (isnothingchanged && !cancalculate)
        {

            isnotchanged = true;
            yield return new WaitForSeconds(2f);
            isnothingchanged = false;



        }
        if (isnotchanged)
        {
            isvisualenabled = true;
            yield return new WaitForSeconds(2f);
            isvisualenabled = false;
            isnotchanged = false;
            nextturn = true;
        }
        if(isturnover)
        {
            isvisualenabled = true;
            yield return new WaitForSeconds(2f);
            isvisualenabled = false;
            isturnover = false;
            istimeendsvisual = true;
        }
        if(istimeendsvisual)
        {
            isvisualenabled = true;
            yield return new WaitForSeconds(2f);
            isvisualenabled = false;
            istimeendsvisual = false;
           
            timeendwin = true;
        }
        if(timeendwin)
        {
            isvisualenabled = true;
            yield return new WaitForSeconds(2f);
            isvisualenabled = false;
            timeendwin = false;
            nextturn = true;
        }
        if(isbothplayed)
        {
            isvisualenabled = true;
            yield return new WaitForSeconds(2f);
            isvisualenabled = false;
            isbothplayed = false;
            isbothplayedvisual = true;
        }
       if(isbothplayedvisual)
        {
            isvisualenabled = true;
            yield return new WaitForSeconds(2f);
            isvisualenabled = false;
            isbothplayedvisual = false;
            nextturn = true;
        }
        if (nextturn)
        {

            iswagebetted = false;
            for (int c = 0; c < Manager.manager.cardlist.Count; c++)
            {
                Manager.manager.cardlist[c].GetComponent<Card>().canshowvalues = false;
            }
            isvisualenabled = true;
            yield return new WaitForSeconds(2f);
            Manager.manager.canplay = true;
            nextturn = false;
            Manager.manager.iswagebetted = false;
      
            for (int j = 0; j < Manager.manager.playerlist.Count; j++)
            {
                Manager.manager.playerlist[j].GetComponent<Playerobject>().cardisplacedbyplayer = false;

            }
            for (int i = 0; i < Manager.manager.playerlist.Count; i++)
            {
                for (int j = 0; j < Manager.manager.playerlist.Count; j++)
                {
                    Manager.manager.playerlist[i].GetComponent<Playerobject>().opponentbetted = 0;
                    Manager.manager.playerlist[i].GetComponent<Playerobject>().wagetobet = 0;
                    Manager.manager.playerlist[j].GetComponent<Playerobject>().opponentbetted = 0;
                    Manager.manager.playerlist[j].GetComponent<Playerobject>().wagetobet = 0;
                }

            }





        }
        if (isvisualgameended)
        {
            isvisualenabled = true;
            yield return new WaitForSeconds(2f);
            // isvisualgameended = false;

        }

    }

    void Timer()
    {
        if (photonView.IsMine)
        {
            if (Manager.manager.canplay && !cardisplacedbyplayer)
            {
                Manager.manager.timerimage.SetActive(true);
                currenttime += Time.deltaTime;
                calculatedtime = currenttime / maxtime;

                Manager.manager.timerimage.GetComponent<Slider>().value = calculatedtime;
            }
            if (currenttime > maxtime)
            {
                istimeover = true;
            }
            if (currenttime > maxtime || cardisplacedbyplayer)
            {
                Manager.manager.canplay = false;
                cardisplacedbyplayer = true;
                currenttime = 0;
                Manager.manager.timerimage.SetActive(false);
                calculatedtime = 0;
                Manager.manager.timerimage.GetComponent<Slider>().value = calculatedtime;

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
                    Manager.manager.timerimage.SetActive(false);

                }
              

            }

        }
    }



    public void Opponentbetted()
    {


        for (int i = 0; i < Manager.manager.playerlist.Count; i++)
        {
            if (!Manager.manager.playerlist[i].GetComponent<Playerobject>().photonView.IsMine)
            {
                if (Manager.manager.playerlist[i].GetComponent<Playerobject>().iswagebetted)
                {
                    if (photonView.IsMine)
                    {
                        opponentbetted = Manager.manager.playerlist[i].GetComponent<Playerobject>().wagetobet;
                    }
                }
            }
        }






    }
    public void Opponentvalues()
    {
        for (int i = 0; i < Manager.manager.playerlist.Count; i++)
        {
            if (!Manager.manager.playerlist[i].GetComponent<Playerobject>().photonView.IsMine)
            {
                if (Manager.manager.playerlist[i].GetComponent<Playerobject>().iswagebetted)
                {
                    if (photonView.IsMine)
                    {
                        opponentwage = Manager.manager.playerlist[i].GetComponent<Playerobject>().maxwage;
                        Debug.Log("oppo1" + opponentwage.ToString());
                        Manager.manager.opponentchiplefttext.text = opponentwage.ToString();
                    }
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
                for (int j = i + 1; j < Manager.manager.placedcardlist.Count; j++)
                {


                    if (Manager.manager.placedcardlist[i].GetComponent<Card>().cardvalue == 1 && Manager.manager.placedcardlist[j].GetComponent<Card>().cardvalue == 3)
                    {
                        for (int a = 0; a < Manager.manager.playerlist.Count; a++)
                        {
                            Debug.Log("Attack beats");
                            if (!Manager.manager.placedcardlist[i].GetComponent<Card>().photonView.IsMine && Manager.manager.playerlist[a].GetComponent<Playerobject>().photonView.IsMine)
                            {
                                Debug.Log(Manager.manager.playerlist[a].GetComponent<Playerobject>().username + "reduces");
                                Manager.manager.playerlist[a].GetComponent<Playerobject>().health -= Manager.manager.playerlist[a].GetComponent<Playerobject>().opponentbetted + 1;
                                //  Manager.manager.playerlist[a].GetComponent<Playerobject>().healthtext.text = Manager.manager.playerlist[a].GetComponent<Playerobject>().health.ToString();

                                Manager.manager.playerlist[a].GetComponent<Playerobject>().isvaluechangedtothisplayer = true;

                            }
                            if (Manager.manager.placedcardlist[i].GetComponent<Card>().photonView.IsMine)
                            {

                                Manager.manager.winname = Manager.manager.playerlist[Manager.manager.placedcardlist[i].GetComponent<Card>().whichplayer].GetComponent<Playerobject>().username;
                                
                            }
                            if (!Manager.manager.placedcardlist[i].GetComponent<Card>().photonView.IsMine)
                            {

                                Manager.manager.winname = Manager.manager.playerlist[Manager.manager.placedcardlist[i].GetComponent<Card>().whichplayer].GetComponent<Playerobject>().username;
                                
                            }

                        
                       

                    }
                }
                    else if (Manager.manager.placedcardlist[j].GetComponent<Card>().cardvalue == 1 && Manager.manager.placedcardlist[i].GetComponent<Card>().cardvalue == 3)
                    {
                        Debug.Log("Attack beats");
                        for (int a = 0; a < Manager.manager.playerlist.Count; a++)
                        {
                            if (!Manager.manager.placedcardlist[j].GetComponent<Card>().photonView.IsMine && Manager.manager.playerlist[a].GetComponent<Playerobject>().photonView.IsMine)
                            {
                                Debug.Log(Manager.manager.playerlist[a].GetComponent<Playerobject>().username + "reduces");
                                Manager.manager.playerlist[a].GetComponent<Playerobject>().health -= Manager.manager.playerlist[a].GetComponent<Playerobject>().opponentbetted + 1;
                                //   Manager.manager.playerlist[a].GetComponent<Playerobject>().healthtext.text = Manager.manager.playerlist[a].GetComponent<Playerobject>().health.ToString();
                                Manager.manager.visualtext.GetComponent<Text>().fontSize = 57;

                                Manager.manager.playerlist[a].GetComponent<Playerobject>().isvaluechangedtothisplayer = true;
                            }
                            if (Manager.manager.placedcardlist[j].GetComponent<Card>().photonView.IsMine)
                            {
                                Manager.manager.winname = Manager.manager.playerlist[Manager.manager.placedcardlist[j].GetComponent<Card>().whichplayer].GetComponent<Playerobject>().username;
                                
                            }
                            if (!Manager.manager.placedcardlist[j].GetComponent<Card>().photonView.IsMine)
                            {

                                Manager.manager.winname = Manager.manager.playerlist[Manager.manager.placedcardlist[j].GetComponent<Card>().whichplayer].GetComponent<Playerobject>().username;
                                
                            }

                        }
                    }
                    else if (Manager.manager.placedcardlist[i].GetComponent<Card>().cardvalue == 1 && Manager.manager.placedcardlist[j].GetComponent<Card>().cardvalue == 2)
                    {
                        Debug.Log("defend beats");
                        for (int a = 0; a < Manager.manager.playerlist.Count; a++)
                        {
                            if (!Manager.manager.placedcardlist[j].GetComponent<Card>().photonView.IsMine && Manager.manager.playerlist[a].GetComponent<Playerobject>().photonView.IsMine)
                            {
                                Debug.Log(Manager.manager.playerlist[a].GetComponent<Playerobject>().username + "reduces");
                                Manager.manager.playerlist[a].GetComponent<Playerobject>().health -= Manager.manager.playerlist[a].GetComponent<Playerobject>().opponentbetted + 1;
                                //  Manager.manager.playerlist[a].GetComponent<Playerobject>().healthtext.text = Manager.manager.playerlist[a].GetComponent<Playerobject>().health.ToString();
                                Manager.manager.visualtext.GetComponent<Text>().fontSize = 57;

                                Manager.manager.playerlist[a].GetComponent<Playerobject>().isvaluechangedtothisplayer = true;
                            }
                            if (Manager.manager.placedcardlist[j].GetComponent<Card>().photonView.IsMine)
                            {

                                Manager.manager.winname = Manager.manager.playerlist[Manager.manager.placedcardlist[j].GetComponent<Card>().whichplayer].GetComponent<Playerobject>().username;
                                
                            }
                            if (!Manager.manager.placedcardlist[j].GetComponent<Card>().photonView.IsMine)
                            {

                                Manager.manager.winname = Manager.manager.playerlist[Manager.manager.placedcardlist[j].GetComponent<Card>().whichplayer].GetComponent<Playerobject>().username;
                                
                            }

                        }
                    }
                    else if (Manager.manager.placedcardlist[j].GetComponent<Card>().cardvalue == 1 && Manager.manager.placedcardlist[i].GetComponent<Card>().cardvalue == 2)
                    {
                        Debug.Log("defend beats");
                        for (int a = 0; a < Manager.manager.playerlist.Count; a++)
                        {
                            if (!Manager.manager.placedcardlist[i].GetComponent<Card>().photonView.IsMine && Manager.manager.playerlist[a].GetComponent<Playerobject>().photonView.IsMine)
                            {
                                Debug.Log(Manager.manager.playerlist[a].GetComponent<Playerobject>().username + "reduces");
                                Manager.manager.playerlist[a].GetComponent<Playerobject>().health -= Manager.manager.playerlist[a].GetComponent<Playerobject>().opponentbetted + 1;
                                //    Manager.manager.playerlist[a].GetComponent<Playerobject>().healthtext.text = Manager.manager.playerlist[a].GetComponent<Playerobject>().health.ToString();
                                Manager.manager.visualtext.GetComponent<Text>().fontSize = 57;

                                Manager.manager.playerlist[a].GetComponent<Playerobject>().isvaluechangedtothisplayer = true;
                            }
                            if (Manager.manager.placedcardlist[i].GetComponent<Card>().photonView.IsMine)
                            {
                               

                                    Manager.manager.winname = Manager.manager.playerlist[Manager.manager.placedcardlist[i].GetComponent<Card>().whichplayer].GetComponent<Playerobject>().username;
                                
                            }
                            if (!Manager.manager.placedcardlist[i].GetComponent<Card>().photonView.IsMine)
                            {
                                
                                    Manager.manager.winname = Manager.manager.playerlist[Manager.manager.placedcardlist[i].GetComponent<Card>().whichplayer].GetComponent<Playerobject>().username;
                                
                            }

                        }
                    }
                    else if (Manager.manager.placedcardlist[i].GetComponent<Card>().cardvalue == 2 && Manager.manager.placedcardlist[j].GetComponent<Card>().cardvalue == 3)
                    {
                        Debug.Log("throw beats");
                        for (int a = 0; a < Manager.manager.playerlist.Count; a++)
                        {
                            if (!Manager.manager.placedcardlist[j].GetComponent<Card>().photonView.IsMine && Manager.manager.playerlist[a].GetComponent<Playerobject>().photonView.IsMine)
                            {
                                Debug.Log(Manager.manager.playerlist[a].GetComponent<Playerobject>().username + "reduces");
                                Manager.manager.playerlist[a].GetComponent<Playerobject>().health -= Manager.manager.playerlist[a].GetComponent<Playerobject>().opponentbetted + 1;

                                //  Manager.manager.playerlist[a].GetComponent<Playerobject>().healthtext.text = Manager.manager.playerlist[a].GetComponent<Playerobject>().health.ToString();
                                Manager.manager.visualtext.GetComponent<Text>().fontSize = 57;

                                Manager.manager.playerlist[a].GetComponent<Playerobject>().isvaluechangedtothisplayer = true;
                            }
                            if (Manager.manager.placedcardlist[j].GetComponent<Card>().photonView.IsMine)
                            {
                               
                                    Manager.manager.winname = Manager.manager.playerlist[Manager.manager.placedcardlist[j].GetComponent<Card>().whichplayer].GetComponent<Playerobject>().username;
                                
                                 
                                
                            }
                            if (!Manager.manager.placedcardlist[j].GetComponent<Card>().photonView.IsMine)
                            {

                                Manager.manager.winname = Manager.manager.playerlist[Manager.manager.placedcardlist[j].GetComponent<Card>().whichplayer].GetComponent<Playerobject>().username;
                                
                            }

                        }
                    }

                    else if (Manager.manager.placedcardlist[j].GetComponent<Card>().cardvalue == 2 && Manager.manager.placedcardlist[i].GetComponent<Card>().cardvalue == 3)
                    {
                        Debug.Log("throw beats");
                        for (int a = 0; a < Manager.manager.playerlist.Count; a++)
                        {
                            if (!Manager.manager.placedcardlist[i].GetComponent<Card>().photonView.IsMine && Manager.manager.playerlist[a].GetComponent<Playerobject>().photonView.IsMine)
                            {
                                Debug.Log(Manager.manager.playerlist[a].GetComponent<Playerobject>().username + "reduces");
                                Manager.manager.playerlist[a].GetComponent<Playerobject>().health -= Manager.manager.playerlist[a].GetComponent<Playerobject>().opponentbetted + 1;

                                //  Manager.manager.playerlist[a].GetComponent<Playerobject>().healthtext.text = Manager.manager.playerlist[a].GetComponent<Playerobject>().health.ToString();
                                Manager.manager.visualtext.GetComponent<Text>().fontSize = 57;
                                Manager.manager.playerlist[a].GetComponent<Playerobject>().isvaluechangedtothisplayer = true;

                            }

                            if (Manager.manager.placedcardlist[i].GetComponent<Card>().photonView.IsMine)
                            {

                                Manager.manager.winname = Manager.manager.playerlist[Manager.manager.placedcardlist[i].GetComponent<Card>().whichplayer].GetComponent<Playerobject>().username ;
                                
                            }
                            if (!Manager.manager.placedcardlist[i].GetComponent<Card>().photonView.IsMine)
                            {
                               
                                    Manager.manager.winname = Manager.manager.playerlist[Manager.manager.placedcardlist[i].GetComponent<Card>().whichplayer].GetComponent<Playerobject>().username;
                                
                            }

                        }
                    }
                    else if (Manager.manager.placedcardlist[j].GetComponent<Card>().cardvalue == Manager.manager.placedcardlist[i].GetComponent<Card>().cardvalue)
                    {
                        for (int a = 0; a < Manager.manager.playerlist.Count; a++)
                        {
                            Manager.manager.playerlist[a].GetComponent<Playerobject>().isnothingchanged = true;

                        }
                    }
                    else if (Manager.manager.placedcardlist[i].GetComponent<Card>().cardvalue == Manager.manager.placedcardlist[j].GetComponent<Card>().cardvalue)
                    {
                        for (int a = 0; a < Manager.manager.playerlist.Count; a++)
                        {
                            Manager.manager.playerlist[a].GetComponent<Playerobject>().isnothingchanged = true;

                        }
                    }


                }

            }
            iscalculating = false;
            Manager.manager.placedcardlist = new List<GameObject>();
        }
    }


    public void Healthvisual()
    {
        if (cancalculate )
        {

            Manager.manager.visualtext.GetComponent<Text>().fontSize = 57;

            Manager.manager.visualtext.GetComponent<Text>().text = "Turn Ends";
        }



        for (int i = 0; i < Manager.manager.playerlist.Count; i++)
        {
            if (Manager.manager.playerlist[i].GetComponent<Playerobject>().isvaluechangedtothisplayer && photonView.IsMine)
            {
                Manager.manager.visualtext.GetComponent<Text>().text = Manager.manager.winname + " wins ";
            }
            if (Manager.manager.playerlist[i].GetComponent<Playerobject>().isvaluechanged && photonView.IsMine)
            {
                Manager.manager.playerlist[i].GetComponent<Playerobject>().healthtext.text = Manager.manager.playerlist[i].GetComponent<Playerobject>().health.ToString();
                Manager.manager.visualtext.GetComponent<Text>().text = Manager.manager.playerlist[i].GetComponent<Playerobject>().username + " Health reduces to " + Manager.manager.playerlist[i].GetComponent<Playerobject>().health.ToString();

            }
            if (Manager.manager.playerlist[i].GetComponent<Playerobject>().isnotchanged && !Manager.manager.playerlist[i].GetComponent<Playerobject>().cancalculate && photonView.IsMine)
            {
                Manager.manager.visualtext.GetComponent<Text>().text = " Same Cards";
            }
            if(Manager.manager.playerlist[i].GetComponent<Playerobject>().isturnover &&photonView.IsMine)
            {
                Manager.manager.visualtext.GetComponent<Text>().text = Manager.manager.playerlist[i].GetComponent<Playerobject>().username+" Time Is Over";
            }
            if (Manager.manager.playerlist[i].GetComponent<Playerobject>().istimeendsvisual && photonView.IsMine)
            {
                Manager.manager.winname = Manager.manager.playerlist[Manager.manager.placedcardlist[0].GetComponent<Card>().whichplayer].GetComponent<Playerobject>().username;
                Manager.manager.visualtext.GetComponent<Text>().text = Manager.manager.winname + " Wins";

            }
            if (Manager.manager.playerlist[i].GetComponent<Playerobject>().timeendwin &&photonView.IsMine )
            {
                Manager.manager.playerlist[i].GetComponent<Playerobject>().healthtext.text = Manager.manager.playerlist[i].GetComponent<Playerobject>().health.ToString();
                Manager.manager.visualtext.GetComponent<Text>().text = Manager.manager.playerlist[i].GetComponent<Playerobject>().username + " Health reduces to " + Manager.manager.playerlist[i].GetComponent<Playerobject>().health.ToString();
                Manager.manager.playerlist[i].GetComponent<Playerobject>().healthtext.text = Manager.manager.playerlist[i].GetComponent<Playerobject>().health.ToString();
            }
            if (Manager.manager.playerlist[i].GetComponent<Playerobject>().isbothplayed && photonView.IsMine)
            {
               
                Manager.manager.visualtext.GetComponent<Text>().text =  " Both Player Didn't Play";

            }
            if (Manager.manager.playerlist[i].GetComponent<Playerobject>().isbothplayedvisual && photonView.IsMine)
            {

                Manager.manager.visualtext.GetComponent<Text>().text = " Both Player Health reduces by 1";
                Manager.manager.playerlist[i].GetComponent<Playerobject>().healthtext.text = Manager.manager.playerlist[i].GetComponent<Playerobject>().health.ToString();

            }
        }


        if (nextturn)
        {

            Manager.manager.placedcardlist = new List<GameObject>();
            Manager.manager.visualtext.GetComponent<Text>().text = "Next turn";
            iswagebetted = false;

           
            for (int c = 0; c < Manager.manager.cardlist.Count; c++)
            {
                if (Manager.manager.cardlist[c].GetComponent<Card>().isplaced)
                {

                    Manager.manager.cardlist[c].transform.position = Manager.manager.cardlist[c].GetComponent<Card>().startpos;
                    Manager.manager.cardlist[c].GetComponent<Card>().isplaced = false;
                    Manager.manager.cardlist[c].GetComponent<Card>().canshowvalues = false;
                   

                }
              
            }
           
            }


        if (isvisualgameended)
        {
            for (int i = 0; i < Manager.manager.playerlist.Count; i++)
            {
                if (Manager.manager.playerlist[i].GetComponent<Playerobject>().health > 0 && Manager.manager.playerlist[i].GetComponent<Playerobject>().isgameended)
                {
                    Manager.manager.visualtext.GetComponent<Text>().text = Manager.manager.playerlist[i].GetComponent<Playerobject>().username + " Had Won the Game";
                }
            }
        }
        if(isvisualdraw)
        {
            for (int i = 0; i < Manager.manager.playerlist.Count; i++)
            {
              
                    Manager.manager.visualtext.GetComponent<Text>().text = " Match Draw";
                
            }
        }


    }

    public void Wincondition()
    {
        for (int i = 0; i < Manager.manager.playerlist.Count; i++)
        {
            if (Manager.manager.playerlist[i].GetComponent<Playerobject>().health <= 0)
            {
                isgameended = true;
                isvisualgameended = true;
            }
        }
        for (int i = 0; i < Manager.manager.playerlist.Count; i++)
        {
            for (int j = i+1; j < Manager.manager.playerlist.Count; j++)
            {
                if (Manager.manager.playerlist[i].GetComponent<Playerobject>().health == 0 && Manager.manager.playerlist[j].GetComponent<Playerobject>().health==0)
                {
                    isdraw = true;
                    isvisualgameended = true;
                }
            }
        }
        if (isgameended)
        {
            for (int c = 0; c < Manager.manager.cardlist.Count; c++)
            {
                Manager.manager.cardlist[c].transform.position = Manager.manager.cardlist[c].GetComponent<Card>().startpos;
                Manager.manager.cardlist[c].GetComponent<Card>().isplaced = false;
                Manager.manager.cardlist[c].GetComponent<Card>().canshowvalues = false;
                Manager.manager.canplay = false;
            }
        }
        if (isdraw)
        {
            for (int c = 0; c < Manager.manager.cardlist.Count; c++)
            {
                isdraw = false;
                Manager.manager.cardlist[c].transform.position = Manager.manager.cardlist[c].GetComponent<Card>().startpos;
                Manager.manager.cardlist[c].GetComponent<Card>().isplaced = false;
                Manager.manager.cardlist[c].GetComponent<Card>().canshowvalues = false;
                Manager.manager.canplay = false;
                isvisualdraw = true;

            }
        }
    }
    void Betvalues()
    {
        if (Manager.manager == null)
        {
            Manager.manager = GameObject.Find("Manager").GetComponent<Manager>();
        }

        for (int j = 0; j < Manager.manager.playerlist.Count; j++)
        {
            if (!Manager.manager.playerlist[j].GetComponent<Playerobject>().photonView.IsMine)
            {
                if (Manager.manager.playerlist[j].GetComponent<Playerobject>().cardisplacedbyplayer)
                {

                    Debug.Log("opponent" + Manager.manager.playerlist[j].GetComponent<Playerobject>().opponentbetted.ToString());

                    if (photonView.IsMine)
                    {
                        for (int c = 0; c < Manager.manager.cardlist.Count; c++)
                        {
                            Debug.Log("opponent" + Manager.manager.playerlist[j].GetComponent<Playerobject>().opponentbetted.ToString());
                            if (!Manager.manager.cardlist[c].GetComponent<Card>().photonView.IsMine)
                            {
                                Manager.manager.cardlist[c].GetComponent<Card>().opponentbettetdobject.SetActive(true);
                                Manager.manager.cardlist[c].GetComponent<Card>().Opponentbettedtext.text = opponentbetted.ToString();
                            }
                        }



                    }
                }

            }


        }





    }
    void Tokenreset()
    {
        if (Manager.manager.placedcardlist.Count ==2 )
        {
           
            for (int i = 0; i < Manager.manager.playerlist.Count; i++)
            {
                for (int j = i + 1; j < Manager.manager.playerlist.Count; j++)
                {
                    if (Manager.manager.playerlist[i].GetComponent<Playerobject>().maxwage == 0 && Manager.manager.playerlist[j].GetComponent<Playerobject>().maxwage == 0)
                    {
                        
                        Debug.Log("Is executes");
                        Manager.manager.maxwagevalue =10;
                        Manager.manager.playerlist[i].GetComponent<Playerobject>().maxwage = 10;
                        Manager.manager.playerlist[j].GetComponent<Playerobject>().maxwage = 10;
                        Manager.manager.chiptext.text = Manager.manager.maxwagevalue.ToString();


                        Manager.manager.playerlist[i].GetComponent<Playerobject>().opponentwage = Manager.manager.playerlist[i].GetComponent<Playerobject>().maxwage;
                        Manager.manager.playerlist[j].GetComponent<Playerobject>().opponentwage = Manager.manager.playerlist[j].GetComponent<Playerobject>().maxwage;
                            Debug.Log("oppo2" + opponentwage.ToString());
                            Manager.manager.opponentchiplefttext.text = opponentwage.ToString();
                        
                    }
                }
            }
        }
    }

    public void Timeover()
    {
        if (Manager.manager.placedcardlist.Count == 1)
        {

            for (int i = 0; i < Manager.manager.playerlist.Count; i++)
            {
                if (Manager.manager.playerlist[i].GetComponent<Playerobject>().istimeover)
                {
                    istimeover = false;

                    Manager.manager.playerlist[i].GetComponent<Playerobject>().health -= Manager.manager.playerlist[i].GetComponent<Playerobject>().opponentbetted + 1;
                    //  Manager.manager.playerlist[i].GetComponent<Playerobject>().healthtext.text = Manager.manager.playerlist[i].GetComponent<Playerobject>().health.ToString();
                    Debug.Log(Manager.manager.playerlist[i].GetComponent<Playerobject>().health + "health");

                    Manager.manager.playerlist[i].GetComponent<Playerobject>().isturnover = true;

                }
            }
        }
        if (Manager.manager.placedcardlist.Count == 0)
        {
            for (int i = 0; i < Manager.manager.playerlist.Count; i++)
            {
                if (Manager.manager.playerlist[i].GetComponent<Playerobject>().istimeover && photonView.IsMine)
                {
                    istimeover = false;
                    isbothplayed = true;
                    Manager.manager.playerlist[i].GetComponent<Playerobject>().health -= Manager.manager.playerlist[i].GetComponent<Playerobject>().opponentbetted + 1;
                    //  Manager.manager.playerlist[i].GetComponent<Playerobject>().healthtext.text = Manager.manager.playerlist[i].GetComponent<Playerobject>().health.ToString();
                    Debug.Log(Manager.manager.playerlist[i].GetComponent<Playerobject>().health + "health");

                  

                }
            }
        }
        }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(playerindex);
            stream.SendNext(health);
            stream.SendNext(cancalculate);
            stream.SendNext(cardisplacedbyplayer);
            stream.SendNext(healthtext.text);
            stream.SendNext(isvaluechangedtothisplayer);
            stream.SendNext(isvaluechanged);
            stream.SendNext(isnothingchanged);
            stream.SendNext(isnotchanged);
            stream.SendNext(isgameended);
            stream.SendNext(iswagebetted);
            stream.SendNext(wagetobet);
            stream.SendNext(wagevalue);
            stream.SendNext(opponentbetted);
            stream.SendNext(iswagevisualbet);
            stream.SendNext(opponentwage);
            stream.SendNext(Manager.manager.bettedtext.text);
            stream.SendNext(maxwage);
            stream.SendNext(iswin);
            stream.SendNext(winname);
            stream.SendNext(isturnover);
            stream.SendNext(istimeendsvisual);
        }
        else if (stream.IsReading)
        {
            playerindex = (int)stream.ReceiveNext();
            health = (int)stream.ReceiveNext();
            cancalculate = (bool)stream.ReceiveNext();
            cardisplacedbyplayer = (bool)stream.ReceiveNext();
            healthtext.text = (string)stream.ReceiveNext();
            isvaluechangedtothisplayer = (bool)stream.ReceiveNext();
            isvaluechanged = (bool)stream.ReceiveNext();
            isnothingchanged = (bool)stream.ReceiveNext();
            isnotchanged = (bool)stream.ReceiveNext();
            isgameended = (bool)stream.ReceiveNext();
            iswagebetted = (bool)stream.ReceiveNext();
            wagetobet = (int)stream.ReceiveNext();
            wagevalue = (int)stream.ReceiveNext();
            opponentbetted = (int)stream.ReceiveNext();
            iswagevisualbet = (bool)stream.ReceiveNext();
            opponentwage = (int)stream.ReceiveNext();
            Manager.manager.bettedtext.text = (string)stream.ReceiveNext();
            maxwage = (int)stream.ReceiveNext();
            iswin = (bool)stream.ReceiveNext();
            winname = (string)stream.ReceiveNext();
            isturnover = (bool)stream.ReceiveNext();
            istimeendsvisual = (bool)stream.ReceiveNext();
        }
    }
}
