using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectorBTNBehaviourScript : MonoBehaviour
{
    private MasterBehaviourScript master;
    // Start is called before the first frame update
    void Start()
    {
        master = GetComponentInParent<MasterBehaviourScript>();
    }

    /*
    // Update is called once per frame
    void Update()
    {
        
    }
    */

    public void OnClick()
    {
        Debug.Log("CLICKED");
        master.SetSelectors(this.name);
    }

    public void SelectionChanged(string selected)
    {
        if(selected == this.name)
        {
            this.GetComponent<Image>().color = Color.red;
        }
        else
        {
            this.GetComponent<Image>().color = Color.white;
        }
    }
}
