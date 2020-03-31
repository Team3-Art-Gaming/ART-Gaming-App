using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alertHandler : MonoBehaviour
{
    public GameObject caller;
    private GameObject popNotif;
    void Start()
    {
        this.popNotif = Instantiate(caller, new Vector3(0, 0, 1), Quaternion.identity);
    }
}
