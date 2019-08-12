using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SocketIO;
using RoomModel;
using System;

public class ResultController : MonoBehaviour
{
    public SocketIOComponent socket;
    public Result result;
    public PlayerCon playerCon;
    public ResultCon resultCon;
    public Transform resultPanel;
    public Button Backbtn;
    void Start()
    {
        Backbtn.onClick.AddListener(() => Leaveroom());
        if (socket != null)
        {
            socket.On("Leave",backtolobby);
            socket.On("ShowResult", showresult);
            StartCoroutine("CallResult");
        }
        else
        {
            Debug.Log("Socket is null");
        }
    }

    public void backtolobby(SocketIOEvent obj)
    {
        SceneManager.LoadScene("RoomLobby");
    }

    public void showresult(SocketIOEvent obj)
    {
        Debug.Log("Result" + obj.data);
        Room room = JsonUtility.FromJson<Room>(obj.data.ToString());
        List<Result> resultList = new List<Result>();
        resultList = room.result;
        foreach(var item in resultList)
        {
            ResultCon showresult = Instantiate(resultCon, resultPanel) as ResultCon;
            showresult.result = item;
        }
    }

    public IEnumerator CallResult()
    {
        yield return new WaitForSeconds(0.3f);
        var data = JsonUtility.ToJson(playerCon.player);
        socket.Emit("LoadResult", new JSONObject(data));
    }
    public void Leaveroom()
    {
        Debug.Log("Back to Lobby");
        var data = JsonUtility.ToJson(playerCon.player);
        socket.Emit("LeaveRoom",new JSONObject(data));
        Debug.Log("Emit Leave Room");
    }
}
