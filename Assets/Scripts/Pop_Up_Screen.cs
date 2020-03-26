using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pop_Up_Screen : MonoBehaviour
{
    [SerializeField]
    Text textbox;
    
    [SerializeField]
    Button ok_Button;
    
    [SerializeField]
    Button cancel_Button;

    private GameObject caller;
    
    public void activatePopUp(string s, GameObject caller)
    {
        this.caller = caller;
        transform.position = new Vector3(0,0,0);
        textbox.text = s;
    }

    public void deactivatePopUp()
    {
            transform.position = new Vector3(-10000, -10000, 0);
    }

    public void okay_Click()
    {
        caller.SendMessage("POP_UP_RESPONSE", "OK");
    }

    public void cancel_Click()
    {
        caller.SendMessage("POP_UP_RESPONSE", "Cancel");
    }
}
