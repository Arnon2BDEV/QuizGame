using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RoomModel;
public class RoomfulMsg : MonoBehaviour
{
    public Room room;
    public Text RoomnameTxt;
    public Text Msg;
    public Button Backbtn;

    public void Start()
    {
    }
    public void Setup(Room room)
    {
        RoomnameTxt.text = room.roomname;
        Msg.text = "This room is full";
        Backbtn.onClick.AddListener(() => {
            this.gameObject.SetActive(false);
        });
    }
}