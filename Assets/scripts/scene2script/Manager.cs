using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class Manager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static Manager manager;
    //about player
    public GameObject playeref;
    private GameObject playerobject;
    public List<Transform> playerposition = new List<Transform>();
    public List<GameObject> playerlist = new List<GameObject>();


    private GameObject cam;



    //buttons
    public GameObject reloadbutton;
    public GameObject exitbutton;







    //about cards
    public int row;
    public int coloumns;
    public Vector2 gridsize;
    public Vector2 gridoffset;
    public GameObject cellobject;
    public Vector2 cellscale;
    public Vector2 cellsize;
    public List<Sprite> cardsprite = new List<Sprite>();
    public Vector3 newpos;
    public List<Transform> cardspawnpos = new List<Transform>();
    public List<Transform> cardplaceposition = new List<Transform>();
    private GameObject g;
    public int cardcount;
    public List<GameObject> cardlist = new List<GameObject>();
    public List<GameObject> placedcardlist = new List<GameObject>();
    public int numberofcard;
    public List<Sprite> coveredsprite = new List<Sprite>();
    public List<GameObject> cardchipbet = new List<GameObject>();

    //visual text
    public GameObject visualtext;
    public GameObject startbutton;
    public bool canplay;
    //player timer
    public GameObject timerimage;

    public GameObject wageobject;
    public Button increamentbutton;
    public Button decreamentbutton;
    public int wagevalue;
    public int maxwagevalue;
    public Text wagevaluetext;
    public Text chiptext;
    public GameObject wagebetbutton;
    public bool iswagebetted;
    public Text opponentchiplefttext;




    public GameObject opponentbettedtext;
    public Text bettedtext;
    public int betadjust;

    public string winname;
    //speacial button
    public GameObject specialBtn,OpenentSpecialBtn;
    public List<GameObject> specials = new List<GameObject>();
    public List<GameObject> activespecial = new List<GameObject>();
    public int opponentspecialnumber;
    public GameObject cardpanel;
    public int maxspecialcount;
    public List<Sprite> charactersprite = new List<Sprite>();

    private void Start()
    {
        Spawn();
        // Camrotation();
        Cardspawn();
        reloadbutton.GetComponent<Button>().onClick.AddListener(Onclickreloadscene);
        exitbutton.GetComponent<Button>().onClick.AddListener(Onclickexit);

    }

    private void Update()
    {
      
    }




    //Player spawn
    void Spawn()
    {
        playerobject = PhotonNetwork.Instantiate(playeref.name, new Vector3(0, 0, 0), Quaternion.identity);
    }





    //camera rotation for localplayer at the bottom
    void Camrotation()
    {
        cam = GameObject.Find("Main Camera");
        if (Mastermanager._gamesettings.playerenteredindex == 1)
        {
            cam.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
        }
        else
        {
            cam.transform.rotation = Quaternion.identity;
        }
    }


    //card spawn
    void Cardspawn()
    {



        cellsize = cardsprite[Mastermanager._gamesettings.playerenteredindex].bounds.size;
        Vector2 newcellsize = new Vector2(gridsize.x / (float)coloumns, gridsize.y / (float)row);

        cellscale = new Vector2(newcellsize.x / cellsize.x, newcellsize.y / cellsize.y);
        cellsize = newcellsize;

        Debug.Log(cellobject.transform.position + "cpos");

        cellobject.transform.localScale = new Vector2(cellscale.x, cellscale.y);

        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < coloumns; j++)
            {





                newpos = new Vector2(j * cellsize.x + gridoffset.x, i * cellsize.y + gridoffset.y);

                g = PhotonNetwork.Instantiate(cellobject.name, newpos + cardspawnpos[0].transform.position, Quaternion.identity);




                g.transform.localScale = new Vector2(cellscale.x, cellscale.y);








            }

        }








    }


    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, gridsize);
    }







    //reload scene
    public void Onclickreloadscene()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LeaveRoom();

            PhotonNetwork.LoadLevel(0);
        }
        else
        {
            PhotonNetwork.LoadLevel(0);
        }

    }
    public void Onclickexit()
    {
        Application.Quit();
    }

  

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // stream.SendNext(cardcount);
            stream.SendNext(winname);
            

        }
        else if (stream.IsReading)
        {
            //cardcount = (int)stream.ReceiveNext();
            winname = (string)stream.ReceiveNext();

        }
    }
}
