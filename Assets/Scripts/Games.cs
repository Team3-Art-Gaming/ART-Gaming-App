using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Games 
{
    public string SessionName;
    public string Status;
    public string HostName;

    public Games(string name, string status, string hName)
	{
        this.SessionName = name;
        this.Status = status;
        this.HostName = hName;
	}
}
