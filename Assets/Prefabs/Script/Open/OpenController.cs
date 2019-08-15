using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using SocketIO;
using System.Text.RegularExpressions;
using Playermodel;

public class OpenController : MonoBehaviour
{
    public Button EnterBtn;
    public InputField nameIn;
    public SocketIOComponent socket;
    public Text text;
    public PlayerCon playerCon;
    void Start()
    {
        EnterBtn.onClick.AddListener(() => OnClickEnter());
        socket.On("NEXT", next);
        socket.On("CUNAME", cuName);
    }

    public void next(SocketIOEvent obj)
    {
        Debug.Log(obj.data);
        playerCon.player = JsonUtility.FromJson<Player>(obj.data.ToString());
        Debug.Log("Player : " + playerCon.player.name + " has join ");
        SceneManager.LoadScene("RoomLobby");
    }
    public void cuName(SocketIOEvent obj)
    {
        text.text = "This name has use.Please try Again";
        text.enabled = true;
        StartCoroutine("Textdisable");
    }

    public void OnClickEnter()
    {
        if (nameIn.text != "")
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data["name"] = nameIn.GetComponent<InputField>().text;
            Debug.Log(data["name"]);
            socket.Emit("SETNAME", new JSONObject(data));
        }
        else
        {
            text.text = "Please Assign your name";
            text.enabled = true;
            StartCoroutine("Textdisable");
        }
    }

    public IEnumerator Textdisable()
    {
        yield return new WaitForSeconds(3);
        text.enabled = false;
    }
    string JsonToString(string target, string s)
    {
        string[] newstring = Regex.Split(target, s);
        return newstring[1];
    }
    string fixJson(string value)
    {
        value = "{\"player\":" + value + "}";
        return value;
    }
}
