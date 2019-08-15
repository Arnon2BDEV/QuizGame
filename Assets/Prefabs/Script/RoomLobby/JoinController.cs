using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RoomModel;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JoinController : MonoBehaviour
{
    public Button JoinBtn;
    public Button CJoinBtn;
    public InputField JoinIn;
    public Text Roomname;
    public Text Msg;
    public Room room;

    public void Onclick_CJoin()
    {
        JoinIn.text = "";
        Msg.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
    }
}
