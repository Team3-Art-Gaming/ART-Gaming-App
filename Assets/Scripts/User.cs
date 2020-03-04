using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User : MonoBehaviour
{
    private string username;
    private string userid;
    private string email;
    private string password;

    public User() { }
    public User(string name, string id, string email, string pass)
    {
        this.username = name;
        this.userid = id;
        this.email = email;
        this.password = pass;
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
