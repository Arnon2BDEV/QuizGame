using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using RoomModel;
using Playermodel;
using SocketIO;
using System;

public class PlayerLobbyController : MonoBehaviour
{
    public PlayerCon playerCon;
    public RoomController roomController;
    public Button btn_startGame;
    public Button btn_Close;
    public Text roomPin;
    public Text roomName;
    public Transform content;
    public SocketIOComponent socket;
    public List<Player> play = new List<Player>();
    PlayerCon player;
    public void Start()
    {
        if (socket != null)
        {
            Debug.Log("socket Connect");
            StartCoroutine("Loadroom");
            socket.On("ADD_PLAYER", updateroom);
            socket.On("OUT",exitroom);
            socket.On("DPLAYER",deleteplayer);
            socket.On("SETROOM", loadplayer);
            socket.On("START",startgame);
        }
        else
        {
            Debug.Log("socket null");
        }
        btn_Close.onClick.AddListener(() => Onclick_close());
        btn_startGame.onClick.AddListener(() => Onclick_start());
    }

    public IEnumerator Loadroom()
    {
        yield return new WaitForSeconds(0.3f);
        var data = JsonUtility.ToJson(playerCon.player);
        Debug.Log(data);
        socket.Emit("LOAD", new JSONObject(data));
        Debug.Log("Sending");
    }


    public void FixedUpdate()
    {
        var counter = play.Count;
        if (playerCon.player.status == 1)
        {
            if (counter > 1)
            {
                btn_startGame.gameObject.SetActive(true);
            }
            else btn_startGame.gameObject.SetActive(false);
        }
        else btn_startGame.gameObject.SetActive(false);
    }

    

    public void loadplayer(SocketIOEvent obj)
    {
        Debug.Log(obj.data);
        Room load = JsonUtility.FromJson<Room>(obj.data.ToString());
        List<Player> players = new List<Player>();
        players = load.players;
        roomPin.text = "GamePin : " + load.roompin;
        roomPin.gameObject.SetActive(true);
        roomName.text = load.roomname;
        roomName.gameObject.SetActive(true);
        roomController.room = load;
        foreach (Player item in load.players)
        {
            Debug.Log(item.name);
            player = Instantiate(playerCon, content) as PlayerCon;
            player.player = item;
            play.Add(item);
        }
    }

    private void exitroom(SocketIOEvent obj)
    {
        SceneManager.LoadScene("RoomLobby");
    }

    private void Onclick_close()
    {
        Player checkplayer = playerCon.player;
        Debug.Log(checkplayer.status);
        if (checkplayer.status == 0)
        {
            var player = JsonUtility.ToJson(checkplayer);
            Debug.Log(roomController.room.roomname);
            Debug.Log(checkplayer.name + ": exit room");
            Debug.Log(player);
            socket.Emit("EXITROOM",new JSONObject(player));
        }
        else if (checkplayer.status == 1)
        {
            var owner = JsonUtility.ToJson(checkplayer);
            Debug.Log(checkplayer.name + ": Destroy Room");
            Debug.Log(owner);
            socket.Emit("DESTROY_ROOM", new JSONObject(owner));
        }
    }
    public void updateroom(SocketIOEvent obj)
    {
        Debug.Log("update Room : " + obj.data);
        Player load = JsonUtility.FromJson<Player>(obj.data.ToString());
        player = Instantiate(playerCon, content) as PlayerCon;
        player.player = load;
        play.Add(load);
        Debug.Log("Play has " + play.Count);
    }

    public void deleteplayer(SocketIOEvent obj)
    {
        Player del = JsonUtility.FromJson<Player>(obj.data.ToString());
        GameObject[] res = GameObject.FindGameObjectsWithTag("Player");
        foreach(Player item in play)
        {
            if(item.id == del.id)
            {
                for (var i = 0; i < res.Length; i++)
                {
                    if (res[i].name == del.name)
                    {
                        Destroy(res[i]);
                    }
                }
                play.Remove(item);
                Debug.Log("playList has "+ play.Count);
            }
        }
    }

    private void Onclick_start()
    {
        var data = playerCon.player.inroomname;
        socket.Emit("STARTGAME",data);
    }

    private void startgame(SocketIOEvent obj)
    {
        SceneManager.LoadScene("Game");
    }
}
