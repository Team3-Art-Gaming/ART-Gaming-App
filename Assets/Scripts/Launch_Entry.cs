using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Launch_Entry : MonoBehaviour
{
    [SerializeField]
    Text Friend_Name;
    [SerializeField]
    Toggle toggleBTN;


    void Start()
    {
        toggleBTN.isOn = false;
    }

    public void SetName(string name)
    {
        this.name = name;
        Friend_Name.text = name;
    }

    public bool GetInvitedStatus()
    {
        return toggleBTN.isOn;
    }
}
