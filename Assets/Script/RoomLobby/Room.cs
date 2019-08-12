using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Playermodel;

namespace RoomModel
{
    [System.Serializable]
    public class Room
    {
        public string id;
        public string roomname;
        public string roompin;
        public List<Player> players;
        public long playerinroom;
        public bool status;
        public List<Result> result = new List<Result>();
    }

    [System.Serializable]
    public class Result
    {
        public string id;
        public string name;
        public int score;
    }

    [System.Serializable]
    public class RoomList
    {
        public List<Room> rooms;
    }
}
    
