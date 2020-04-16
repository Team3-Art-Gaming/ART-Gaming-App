using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Friends
{
    public string Name;
    public string Status;
    public bool isInvited;

    public Friends(string name, string status, bool invited = false)
    {
        this.Name = name;
        this.Status = status;
        this.isInvited = invited;
    }

}
