using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChoicesController : MonoBehaviour
{
    public QuestionModel.Choices choices;
    public Text choicetxt;

    public bool Isclick;

    void Start()
    {

    }
    public void Setup (QuestionModel.Choices data)
    {
        this.name = choices.text;
        choices = data;
        choicetxt.text = choices.text;
    }
}