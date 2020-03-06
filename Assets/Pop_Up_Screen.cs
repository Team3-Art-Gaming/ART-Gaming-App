using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pop_Up_Screen : MonoBehaviour
{
    [serializedfield]
    Text textbox;
    
    [serializedfield]
    Button ok_Button;
    
    [serializedfield]
    Button cancel_Button;
    
    public activatePopUp(string s)
    {
        transform.position = new Vector3(0,0,0);
        textbox.text = s;
    }

    public deactivatePopUp()
    {
            transform.position = new Vector(-10000, -10000, 0);
    }
}
