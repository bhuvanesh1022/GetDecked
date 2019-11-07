using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roomcanvases : MonoBehaviour
{
    [SerializeField]
    private CreateorJoinroomcanvas _createorJoinroomcanvas;
    [SerializeField]
    private Currentroomcanvas _currentroomcanvas;
   
    [SerializeField]
    private Createroom createroom;
    [SerializeField]
    private Roomlisting roomlisting;
    [SerializeField]
    private Levelselect levelselect;

    public Roomlisting RoomListing
    {
        get
        {
            return roomlisting;
        }
    }
    public CreateorJoinroomcanvas createorJoinroomcanvas
    {
        get
        {
            return _createorJoinroomcanvas;
        }
    }
  
    public Currentroomcanvas currentroomcanvas
    {
        get
        {
            return _currentroomcanvas;
        }
    }
   

    public Createroom CreateRoom

    {
        get
        {
            return createroom;
        }
       
    }
       public Levelselect LevelSelect
    {
        get
        {
            return levelselect;
        }
    }
        private void Awake()
    {
        FirstInitialize();
    }
    private void FirstInitialize()
    {
        createorJoinroomcanvas.Firstinitialize(this);
        currentroomcanvas.Firstinitialize(this);
        
        roomlisting.FirstInitialize(this);
      
    }
}
