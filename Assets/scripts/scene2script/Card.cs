using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class Card : MonoBehaviourPunCallbacks, IPunObservable
{

    public Vector2 startpos;
    public bool iscollided;
    public string cardname;
    public List<string> cardattributes = new List<string>();
    public Text cardattributetext;
    public int cardvalue;
    public bool isplaced;
    public bool canshowvalues;
    public bool respawned;
   
    private void Start()
    {
        respawned = true;
        Cardnamesync();
        Attributes();

        startpos = transform.position;
        Localscale();

        photonView.RPC("Addcard", RpcTarget.AllBuffered, null);
    }
    private void Update()
    {
        Gameplay();
        Cardshow();
       
        Cardcovered();

    }

    //card scaled
    public void Localscale()
    {
        if (!photonView.IsMine)
        {
            transform.localScale = new Vector3(0f, 0f, 0f);
        }
    }

    //card name
    void Cardnamesync()
    {
        if (photonView.IsMine)
        {
            Manager.manager.cardcount++;
            cardvalue = Manager.manager.cardcount;
            cardname = Mastermanager._gamesettings.Nickname;
            this.gameObject.name = cardname + "'s" + "card" + cardvalue;
        }


    }

    [PunRPC]
    public void Addcard()
    {
        if (Manager.manager == null)
        {
            Manager.manager = GameObject.Find("Manager").GetComponent<Manager>();
        }

        if (Manager.manager.cardlist.Contains(this.gameObject))
            return;
        Manager.manager.cardlist.Add(this.gameObject);

    }
    private void OnMouseDown()
    {
        if (Manager.manager == null)
        {
            Manager.manager = FindObjectOfType<Manager>();
        }
        if (photonView.IsMine && Manager.manager.canplay&&Manager.manager.iswagebetted)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = pos;
            gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = 2;
        }
    }
    private void OnMouseDrag()
    {
        if (Manager.manager == null)
        {
            Manager.manager = FindObjectOfType<Manager>();
        }
        if (photonView.IsMine && Manager.manager.canplay && Manager.manager.iswagebetted)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = pos;
            gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = 2;
        }
    }
    private void OnMouseUp()
    {
        if (Manager.manager == null)
        {
            Manager.manager = FindObjectOfType<Manager>();
        }
        transform.position = startpos;
        isplaced = false;
        gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = 0;
        if (iscollided)
        {
            transform.position = Manager.manager.cardplaceposition[Mastermanager._gamesettings.playerenteredindex].position;
            gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = 2;
            isplaced = true;
            photonView.RPC("Addplacedcard", RpcTarget.AllBuffered, null);

        }
       

    }

   



    //triggering it
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Table")
        {
            iscollided = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Table")
        {
            iscollided = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Table")
        {
            iscollided = false;
        }
    }


    void Attributes()
    {
        if (photonView.IsMine && respawned)
        {

            // cardvalue = Random.Range(0, cardattributes.Count);

            cardattributetext.text = cardattributes[cardvalue - 1];
            respawned = false;
        }
    }



    void Cardshow()
    {
        if (Manager.manager == null)
        {
            Manager.manager = FindObjectOfType<Manager>();
        }
        if (isplaced && !photonView.IsMine)
        {
            transform.localScale = Manager.manager.cellscale;
        }
        else if (!isplaced && !photonView.IsMine)
        {
            transform.localScale = new Vector3(0f, 0f, 0f);
        }
    }

    [PunRPC]
    public void Addplacedcard()
    {
        if (Manager.manager.placedcardlist.Contains(this.gameObject))
            return;
        Manager.manager.placedcardlist.Add(this.gameObject);
    }



    public void Gameplay()
    {
        if (Manager.manager.placedcardlist.Count == 2)
        {
            canshowvalues = true;

        }

    }
    void Cardcovered()
    {
        if (!photonView.IsMine)
        {
            if (canshowvalues)
            {
                this.gameObject.GetComponentInChildren<SpriteRenderer>().sprite = Manager.manager.coveredsprite[0];
                this.gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = 1;
            }
            else
            {
                this.gameObject.GetComponentInChildren<SpriteRenderer>().sprite = Manager.manager.coveredsprite[1];
                this.gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = 3;
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(iscollided);
            stream.SendNext(this.gameObject.name);
            stream.SendNext(isplaced);
            stream.SendNext(gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder);
            stream.SendNext(cardvalue);
            stream.SendNext(canshowvalues);
            stream.SendNext(respawned);
           
            if (isplaced)
            {
                stream.SendNext(transform.position);
            }
            if (canshowvalues && iscollided)
            {

                stream.SendNext(cardattributetext.text);
                stream.SendNext(transform.position);
            }


        }
        else if (stream.IsReading)
        {
            iscollided = (bool)stream.ReceiveNext();
            this.gameObject.name = (string)stream.ReceiveNext();
            isplaced = (bool)stream.ReceiveNext();
            gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = (int)stream.ReceiveNext();
            cardvalue = (int)stream.ReceiveNext();
            canshowvalues = (bool)stream.ReceiveNext();
            respawned = (bool)stream.ReceiveNext();
        
            if (isplaced)
            {
                transform.position = (Vector3)stream.ReceiveNext();
            }

            if (canshowvalues && iscollided)
            {

                cardattributetext.text = (string)stream.ReceiveNext();
                transform.position = (Vector3)stream.ReceiveNext();
            }
        }
    }
}
