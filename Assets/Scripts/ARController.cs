using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ARController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] icons;
    [SerializeField]
    private GameObject mapHolder;

    private int curIcon;

    private float moveIncrement = 0.315f;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (GameObject icon in icons) icon.SetActive(false);
        curIcon = 0;
        icons[curIcon].SetActive(true);
        //transform.position -= new Vector3(moveIncrement * 1.5f, 0, moveIncrement * 4.5f);
    }

    public void SetIcon(int i)
    {
        Debug.Log("Heard Set Icon");
        icons[curIcon].SetActive(false);
        curIcon = i;
        icons[curIcon].SetActive(true);
    }
}
