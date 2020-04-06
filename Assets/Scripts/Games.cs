using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Games 
{
    public string SessionName;
    public string Status;

    public Games(string name,string status)
	{
        this.SessionName = name;
        this.Status = status;
	}
}
