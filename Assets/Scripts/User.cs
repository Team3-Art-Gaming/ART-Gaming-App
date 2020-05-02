using System.Collections.Generic;
using UnityEngine;

public class User : ScriptableObject
{ 
    public string email;
    public string userid;
    public string username;
    public List<Friends> FriendsList = new List<Friends>();
    public List<string> CreatedMaps = new List<string>();

    public void init(string name, string email, string userid)
    {
        this.email = email;
        this.userid = userid;
        this.username = name;

        Friends F = new Friends("John", "Pending");
        FriendsList.Add(F);
        F = new Friends("Alice", "Requesting");
        FriendsList.Add(F);
        F = new Friends("Mike", "Pending");
        FriendsList.Add(F);
        F = new Friends("Oscar", "Requesting");
        FriendsList.Add(F);
        F = new Friends("Diana", "Friend");
        FriendsList.Add(F);
        F = new Friends("Max", "Friend");
        FriendsList.Add(F);
        F = new Friends("George", "Friend");
        FriendsList.Add(F);
        F = new Friends("James", "Requesting");
        FriendsList.Add(F);
        F = new Friends("Hector", "Pending");
        FriendsList.Add(F);
        F = new Friends("Jessie", "Requesting");
        FriendsList.Add(F);
        F = new Friends("Ashley", "Pending");
        FriendsList.Add(F);
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
