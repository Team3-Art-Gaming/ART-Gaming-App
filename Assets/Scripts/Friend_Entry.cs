using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Friend_Entry : MonoBehaviour
{
    [SerializeField] 
    Text Friend_Name;
    [SerializeField] 
    Image Friend_Icon;
    [SerializeField] 
    Button Send_Button;

    public void SetName(string N)
    {
        Friend_Name.text = N;
    }

    public string GetName()
    {
        return Friend_Name.text;
    }

    public void SetIcon()
    {
        
    }

    public Image GetIcon()
    {
        return Friend_Icon;
    }

    public void SetButton()
    {
        
    }

    public Button GetButton()
    {
        return Send_Button;
    }
}
