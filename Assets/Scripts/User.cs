using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    public string username;
    public string userid;
    public string email;
    public string password;
    public List<string> friendslist = new List<string>();
    public List<string> friendrequests = new List<string>();
    public List<string> createdmaps = new List<string>();

    public User() { }
    public User(string name, string id, string email, string pass)
    {
        this.username = name;
        this.userid = id;
        this.email = email;
        this.password = pass;
        this.friendslist.Add("placeholder");
        this.friendrequests.Add("placeholder");
        this.createdmaps.Add("placeholder");
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

    public string getPassword()
    {
        return this.password;
    }

    public void setUsername(string name)
    {
        this.username = name;
    }
}
