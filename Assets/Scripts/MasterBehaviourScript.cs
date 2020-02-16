using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterBehaviourScript : MonoBehaviour
{
    [SerializeField]
    private Button selectorTemplate;
    [SerializeField]
    private Image selectorGrid;
    [SerializeField]
    private Button mapBTNTemplate;
    [SerializeField]
    private Image mapGrid;

    private string curCategory;
    private string curSelection;


    // Start is called before the first frame update
    void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        curCategory = "";
        curSelection = "";
        CreateSelectors("_Cat");
    }

    // Update is called once per frame
    /*
    void Update()
    {
        
    }
    */

    private void CreateSelectors(string dir)
    {
        Debug.Log("DIR: " + dir);
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sets/"+dir);
        int counter = 0;
        foreach (Sprite sprite in sprites)
        {
            //Debug.Log(sprite.name);
            Button b = Instantiate(selectorTemplate);
            //Sprite s = Sprite.Create(i, new Rect(0.0f, 0.0f, 128, 128), new Vector2(0.5f, 0.5f), 100.0f);
            b.image.sprite = sprite;
            b.name = sprite.name;
            b.transform.SetParent(selectorGrid.transform);
        }
        selectorGrid.transform.localPosition = Vector3.zero;
    }

    private void DestroySelectors()
    {
        foreach(Button child in selectorGrid.GetComponentsInChildren<Button>())
        {
            Destroy(child.gameObject);
        }
    }

    public void SetSelectors(string name)
    {
        //Debug.Log("NAME: " + name);
        if(name=="_Cat")//View directory of categories
        {
            curCategory = ""; //No category currently selected
            curSelection = ""; //No item currently selected
            DestroySelectors();
            CreateSelectors(name);
        }
        else if(name[0] == '_') //View items in category
        {
            curCategory = name.Substring(1);
            DestroySelectors();
            CreateSelectors(name.Substring(1));
        }
        else//Select item in category
        {
            curSelection = name;
            BroadcastMessage("SelectionChanged", name);
            //Debug.Log("Cat: " + curCategory + " Sel: " + curSelection);
        }
    }

    public void MapClicked(string caller)
    {
        //Debug.Log("Heard from " + caller);
        if(curCategory != "" && curSelection != "")
        {
            string[] param = new string[] {caller, curCategory, curSelection};
            BroadcastMessage("SetActive", param);
        }
        
    }
}
