using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class Card : MonoBehaviourPunCallbacks,IPunObservable
{

    public Vector2 startpos;
    public bool iscollided;
    public string cardname;
    public List<string> cardattributes = new List<string>();
    public Text cardattributetext;
    public int cardvalue;
    public bool isplaced;
    public bool canshowvalues;

    private void Start()
    {
        Attributes();
        Manager.manager.cardcount++;
        startpos = transform.position;
        Localscale();
        Cardnamesync();
        photonView.RPC("Addcard", RpcTarget.AllBuffered, null);
    }
    private void Update()
    {
        Cardshow();
        Gameplay();
    }

    //card scaled
    public void Localscale()
    {
        if(!photonView.IsMine)
        {
            transform.localScale = new Vector3(0f, 0f, 0f);
        }
    }

    //card name
    void Cardnamesync()
    {
        if (photonView.IsMine)
        {
            cardname = Mastermanager._gamesettings.Nickname;
        }
        this.gameObject.name = cardname + "'s" + "card" + Manager.manager.cardcount;
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
        if (photonView.IsMine &&Manager.manager.canplay)
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
        if (photonView.IsMine &&Manager.manager.canplay)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = pos;
            gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = 2;
        }
    }
    private void OnMouseUp()
    {
        if(Manager.manager==null)
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
        if(collision.gameObject.tag=="Table")
        {
            iscollided = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Table")
        {
            iscollided =false;
        }
    }


    void Attributes()
    {
        if (photonView.IsMine)
        {
            cardvalue = Random.Range(0, cardattributes.Count);

            cardattributetext.text = cardattributes[cardvalue];
        }
    }



    void Cardshow()
    {
        if (Manager.manager == null)
        {
            Manager.manager = FindObjectOfType<Manager>();
        }
        if (isplaced &&!photonView.IsMine)
        {
            transform.localScale = Manager.manager.cellscale;
        }
        else if(!isplaced && !photonView.IsMine)
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
        if(Manager.manager.placedcardlist.Count==2)
        {
            canshowvalues = true;

        }
        else
        {
            canshowvalues = false;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
       if(stream.IsWriting)
        {
            stream.SendNext(iscollided);
            stream.SendNext(cardname);
            stream.SendNext(isplaced);
            stream.SendNext(gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder);
            if(isplaced)
            {
                stream.SendNext(transform.position);
            }
            if(canshowvalues)
            {
                stream.SendNext(cardvalue);
                stream.SendNext(cardattributetext.text);
                stream.SendNext(transform.position);
            }
        }else if(stream.IsReading)
        {
            iscollided = (bool)stream.ReceiveNext();
            cardname = (string)stream.ReceiveNext();
            isplaced = (bool)stream.ReceiveNext();
            gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = (int)stream.ReceiveNext();
            if(isplaced)
            {
                transform.position = (Vector3)stream.ReceiveNext();
            }
            if (canshowvalues)
            {
                cardvalue = (int)stream.ReceiveNext();
                cardattributetext.text = (string)stream.ReceiveNext();
                transform.position = (Vector3)stream.ReceiveNext();
            }
        }
    }
}
