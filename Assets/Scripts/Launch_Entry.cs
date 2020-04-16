using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Launch_Entry : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Text Friend_Name;
    [SerializeField]
    Toggle toggleBTN;

    //string Name;

    //private string Name;

    void Start()
    {
        toggleBTN.isOn = false;
    }

    public void SetName(string name)
    {
        this.name = name;
        //this.Name = name;
        Friend_Name.text = name;
    }

    public bool GetInvitedStatus()
    {
        return toggleBTN.isOn;
    }
}
