using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Playermodel;

public class PlayerCon : MonoBehaviour
{
    public Player player;
    public Text nameTxt;
    public Text StatusTxt;
    public void Start ()
    {
        this.name = player.name;
        
    }
    private void FixedUpdate()
    {
        nameTxt.text = player.name;
        if (player.status == 1)
        {
            StatusTxt.text = "Owner";
        }
        else StatusTxt.text = "";
    }
}
