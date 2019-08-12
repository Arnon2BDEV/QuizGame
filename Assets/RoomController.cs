using System.Collections;
using System.Collections.Generic;
using RoomModel;
using UnityEngine;
using UnityEngine.UI;

public class RoomController : MonoBehaviour
{
    public Room room;
    public static int Maxplayer = 5;
    public static int Minplayer = 2;
    public Text nameTxt;
    public Text numberTxt;
    public Room Loadscene;
    void Start()
    {
        this.name = room.roomname;
        nameTxt.text = "Room : " + room.roomname;
        numberTxt.text = room.playerinroom + " / 5";
    }
    private void FixedUpdate()
    {
        numberTxt.text = room.playerinroom + " / 5";
        if (room.playerinroom == Maxplayer)
        {
            numberTxt.text = "Full";
        }
    }
}
