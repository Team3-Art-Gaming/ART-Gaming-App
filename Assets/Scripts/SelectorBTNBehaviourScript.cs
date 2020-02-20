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
        master.SetSelectors(int.Parse(name));
    }

    public void SelectionChanged(bool active)
    {
        if(active)
        {
            this.GetComponent<Image>().color = Color.red;
        }
        else
        {
            this.GetComponent<Image>().color = Color.white;
        }
    }
}
