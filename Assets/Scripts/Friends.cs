using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friends
{
    public string Name;
    public string Status;
    public bool Invited;

    public Friends(string name, string status)
    {
        this.Name = name;
        this.Status = status;
        this.Invited = false;
    }
    public Friends(string name, string status, bool invited)
    {
        this.Name = name;
        this.Status = status;
        this.Invited = invited;
    }
}
