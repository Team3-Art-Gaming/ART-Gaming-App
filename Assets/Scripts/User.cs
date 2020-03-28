using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : ScriptableObject
{ 
    public string email;
    public string userid;
    public string username;
    public List<string> ReceivedFriendRequests = new List<string>();
    public List<string> SentFriendRequests = new List<string>();
    public List<string> CurrentFriends = new List<string>();
    public List<string> CreatedMaps = new List<string>();

    public void init(string name, string email, string userid)
    {
        this.email = email;
        this.userid = userid;
        this.username = name;
        ReceivedFriendRequests.Add("placeholder");
        SentFriendRequests.Add("placeholder");
        CurrentFriends.Add("placeholder");
        CreatedMaps.Add("{date : name }");
    }

    public string getUsername()
    {
        return this.username;
    }

    public string getUserid()
    {
        return this.userid;
    }

    public string getEmail()
    {
        return this.email;
    }

    public void setUsername(string name)
    {
        this.username = name;
    }

    public void setEmail(string email)
    {
        this.email = email;
    }

    public void setUserId(string userid)
    {
        this.userid = userid;
    }
}
