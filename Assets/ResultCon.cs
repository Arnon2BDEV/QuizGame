using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RoomModel;

public class ResultCon : MonoBehaviour
{
    public RoomModel.Result result;
    public Text nametxt;
    public Text scoretxt;
    void Start()
    {
        this.name = result.name;
        nametxt.text = result.name;
        scoretxt.text = result.score.ToString();
    }
}
