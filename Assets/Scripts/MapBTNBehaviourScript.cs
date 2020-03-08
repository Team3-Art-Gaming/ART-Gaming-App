using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapBTNBehaviourScript : MonoBehaviour
{
    private MasterBehaviourScript master;

    private int myNum;

    // Start is called before the first frame update
    void Start()
    {
        master = GetComponentInParent<MasterBehaviourScript>() as MasterBehaviourScript;
    }

    public void OnClick()
    {
        master.MapClicked(int.Parse(name));
    }
}
