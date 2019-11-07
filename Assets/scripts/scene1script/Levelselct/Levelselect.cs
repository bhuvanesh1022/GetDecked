using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Photon.Pun;
public class Levelselect : MonoBehaviourPunCallbacks
{
    public List<Button> levelelectionbuttonlist = new List<Button>();
    public List<Button> addedlevel = new List<Button>();
    public bool levelselected;
    [SerializeField]
   
    public int index;
    public PhotonView pv;
    
   
    private Roomcanvases roomcanvases;
public void FirstInitialize(Roomcanvases canvases)
    {
        roomcanvases = canvases;
    }
    public void OnClicked(Button button)
    {
        levelselected = true;
        Debug.Log(button.name);
        addedlevel.Add(button);
        index = levelelectionbuttonlist.FindIndex(x => x.name == button.name);
      
       
       // roomcanvases.LevelSelect.LevelCanvas.sortingOrder = 0;
     //   roomcanvases.currentroomcanvas.canvases.currentroomcanvases.sortingOrder = 3;
      
      

    }

    private void Update()
    {
      
           roomcanvases.currentroomcanvas.PlayerListing. readycolor.interactable = true;
        
    }




}
