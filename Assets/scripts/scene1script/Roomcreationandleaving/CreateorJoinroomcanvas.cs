using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateorJoinroomcanvas : MonoBehaviour
{
    private Roomcanvases roomcanvases;
    [SerializeField]
    private Createroom createroom;

    public void Firstinitialize(Roomcanvases canvases)
    {
        roomcanvases = canvases;
        createroom.Firstinitialize(canvases);
    }
}
