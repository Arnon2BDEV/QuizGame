using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SocketIO;
using System.Text.RegularExpressions;
using RoomModel;
using Playermodel;
using System;

public class LobbyController : MonoBehaviour
{
    public SocketIOComponent socket;
    public Transform content;
    public JoinController join;
    public PlayerCon playerCon;
    public RoomController roomController;
    private RoomController room;

    public List<RoomController> roomList;
    public Button btn_createroom;
    public Button btn_close;
    public Text Msg;
    public InputField CreateRoomIn;
    private string password;

    void Start ()
    { 
        socket.On("UPDATE_ROOM", startroom);
        StartCoroutine("CallRoomList");
        socket.On("ADD_ROOM",addroom);
        socket.On("UPDATE",updateroom);
        socket.On("INROOM",inroom);
        socket.On("FULL",msg);
        socket.On("DROOM",deleteroom);
        btn_createroom.onClick.AddListener(() => onClick_CreateRoom());
        btn_close.onClick.AddListener(() => onClick_Close());
    }

    private IEnumerator CallRoomList()
    {
        yield return new WaitForSeconds(0.3f);
        socket.Emit("UPDATE_START");
    }

    public void startroom(SocketIOEvent obj)
    {
        RoomList roomslist = new RoomList();
        Debug.Log(obj.data.ToString());

        roomslist = JsonUtility.FromJson<RoomList>(obj.data.ToString());

        roomList = new List<RoomController>();
        foreach (Room item in roomslist.rooms)
        {
            Debug.Log(item.roomname);
            RoomController room = Instantiate(roomController, content) as RoomController;
            room.room = item;
            Button roomclick = room.GetComponent<Button>();
            roomclick.onClick.AddListener(() =>
            {
                var i = 0;
                if (i == 0)
                {
                    Debug.Log(room.room.id + " is click " + i);
                    Onclick_join(room.room);
                }
                else return;
            });
            roomList.Add(room);
        }
    }

    public void addroom(SocketIOEvent obj)
    {
        Debug.Log("Add Room" + 3);
        string Jsonroom = fixJson(obj.data.ToString());
        Debug.Log(Jsonroom);
        RoomList roomslist = new RoomList();
        roomslist = JsonUtility.FromJson<RoomList>(Jsonroom);
        foreach (Room item in roomslist.rooms)
        {
            Debug.Log(item.roomname);
            RoomController room = Instantiate(roomController, content) as RoomController;
            room.room = item;
            Button roomclick = room.GetComponent<Button>();
            roomclick.onClick.AddListener(() =>
            {
                Debug.Log(room.room.id + " is click ");
                Onclick_join(room.room);
            });
            roomList.Add(room);
        }
    }

    private void updateroom(SocketIOEvent obj)
    {
        Debug.Log("Update Room");
        string Jsonroom = fixJson(obj.data.ToString());
        Debug.Log(Jsonroom);
        RoomList roomslist = new RoomList();
        roomslist = JsonUtility.FromJson<RoomList>(Jsonroom);
        foreach (Room item in roomslist.rooms)
        {
            for (var i = 0; i < roomList.Count; i++)
            {
                if (item.roomname == roomList[i].room.roomname)
                {
                    room = GameObject.Find(item.roomname).GetComponent<RoomController>();
                    room.room = item;
                    Debug.Log(room.room.roomname + "IS UPdate");
                }
            }
            Debug.Log(item.roomname);
        }
    }

    private void inroom(SocketIOEvent obj)
    {
        Debug.Log("inroom");
        string Jsonstring = obj.data.ToString();
        Debug.Log(Jsonstring);
        Player play = JsonUtility.FromJson<Player>(Jsonstring);
        playerCon.player = play;
        SceneManager.LoadScene("PlayerLobby");
    }

    private void deleteroom(SocketIOEvent obj)
    {
        Room del = JsonUtility.FromJson<Room>(obj.data.ToString());
        GameObject[] res = GameObject.FindGameObjectsWithTag("Room");
        for (var i = 0; i < res.Length; i++)
        {
            if (res[i].name == del.roomname)
            {
                Destroy(res[i]);
            }
        }
    }

    private void msg(SocketIOEvent obj)
    {
        join.Msg.text = "Room is full";
        join.Msg.gameObject.SetActive(true);
        StartCoroutine("Textdisable");
    }

    public void onClick_CreateRoom()
    {
        if (CreateRoomIn.text != "")
        {
            Debug.Log(playerCon.player.name);
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["roomname"] = GameObject.Find("CreateRoomIn").GetComponent<InputField>().text;
            data["name"] = playerCon.player.name;
            data["id"] = playerCon.player.id;
            socket.Emit("CREATE_ROOM", new JSONObject(data));
        }
        else
        {
            Msg.text = "Cannot use this name";
            Msg.enabled = true;
            StartCoroutine("Textdisable");
        }
    }

    private void onClick_Close()
    {
        var data = playerCon.player.id;
        Debug.Log("User " + data + "is disconnect");
        socket.Emit("BACK", data);
        SceneManager.LoadScene("Open");
    }

    public void Onclick_join(Room room)
    {
        if(room.status == true)
        {
            join.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log(room.roompin);
            join.gameObject.SetActive(true);
            join.Roomname.text = room.roomname;
            join.JoinBtn.onClick.AddListener(() => {
                string password = GameObject.Find("JoinIn").GetComponent<InputField>().text;
                if (password == room.roompin)
                {
                    Dictionary<string, string> data = new Dictionary<string, string>();
                    data["name"] = playerCon.player.name;
                    data["id"] = playerCon.player.id;
                    data["roomname"] = room.roomname;
                    socket.Emit("JOINROOM", new JSONObject(data));
                    join.gameObject.SetActive(false);
                    return;
                }
                else
                {
                    Debug.Log("Please try again");
                    join.Msg.text = "Password Missmatch";
                    join.Msg.gameObject.SetActive(true);
                    StartCoroutine("Textdisable");
                    return;
                }
            });
        }

    }

    public IEnumerator Textdisable()
    {
        yield return new WaitForSeconds(1f);
        join.Msg.gameObject.SetActive(false);
    }

    string fixJson(string value)
    {
        value = "{\"rooms\":[" + value + "]}";
        return value;
    }

}
