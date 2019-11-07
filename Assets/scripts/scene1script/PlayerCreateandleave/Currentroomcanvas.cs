using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Currentroomcanvas : MonoBehaviour
{
    private Roomcanvases roomcanvases;

    [SerializeField]
    private Playerlisting playerlisting;
    [SerializeField]
    private Leaveroommenu leaveroommenu;

    public Leaveroommenu LeaveroomMenu
    {
        get
        {
            return leaveroommenu;
        }
    }
  
    public Playerlisting PlayerListing
    {
        get
        {
            return playerlisting;
        }
    }
    public void Firstinitialize(Roomcanvases canvases)
    {
        roomcanvases = canvases;
        playerlisting.Firstinitialize(roomcanvases);
       // leaveroommenu.FirstInitialize(roomcanvases);
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
