using System.Collections.Generic;

public class QuestionModel
{
    public int id;
    public string question;
    public List<Choices> choices;
    public string answer;

    [System.Serializable]
    public class Choices
    {
        public int id;
        public string text;
        public bool value;
    }
}

