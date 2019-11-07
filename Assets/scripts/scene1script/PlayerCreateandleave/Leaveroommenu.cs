using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Leaveroommenu : MonoBehaviour
{

    private Roomcanvases canvases;

    public void FirstInitialize(Roomcanvases canvasroom)
    {
        canvases = canvasroom;
    }



    public void Onclickleaveroom()
    {
        PhotonNetwork.LeaveRoom(true);
        canvases.currentroomcanvas.Hide();
    }
 


}
