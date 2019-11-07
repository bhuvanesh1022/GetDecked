
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Roomlisting : MonoBehaviourPunCallbacks
{

    [SerializeField]
    private Transform content;
    [SerializeField]
    private Roomlist roomlisting;
    public List<Roomlist> rmlist = new List<Roomlist>();
    private Roomcanvases roomcanvas;
    public void FirstInitialize(Roomcanvases canvases)
    {
        roomcanvas = canvases;
    }
    public override void OnRoomListUpdate(List<RoomInfo> roominfo)
    {
      
        foreach (RoomInfo list in roominfo)
        {
            if (list.RemovedFromList)
            {
                int index = rmlist.FindIndex(x => x.rominfo.Name == list.Name);
                if (index != -1)
                {
                    Debug.Log(index + "index");
                    Destroy(rmlist[index].gameObject);
                    rmlist.RemoveAt(index);
                }
            }
            else
            {
                int index = rmlist.FindIndex(x => x.rominfo.Name == list.Name);
                Debug.Log(index + "index");
                if (index == -1)
                {
                    Debug.Log(index + "index2");
                    Roomlist listing = Instantiate(roomlisting, content);
                    if (listing != null)

                        listing.SetRoominfo(list);
                    rmlist.Add(listing);
                }
                else
                {
                    //Do something
                    Debug.Log("Not room created");
                }

            }
        }
    }

    public void Onclickleaveroom()
    {
        PhotonNetwork.LeaveRoom();
      rmlist.Clear();
        Debug.Log("Leaved");
      
     //   roomcanvas.currentroomcanvas.canvases.Hide();
        
    }
}
