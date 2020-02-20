using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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

    private const int mapSize = 100;


    private List<Button> mapButtons;
    private List<Button> selectorButtons;

    private int curCategory;
    private int curSelection;
    private Sprite[] sprites;

    private class mapmarker
    {
        mapmarker(int c, int s, Sprite sp, int r = 0)
        {
            cat = c;
            sel = s;
            sprite = sp;
            rot = r;
        }
        int cat;
        int sel;
        Sprite sprite;
        int rot;
    }


    // Start is called before the first frame update
    void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        curCategory = -1;
        curSelection = -1;
        CreateMapButtons();
        CreateSelectorButtons(curCategory);

    }


    private void CreateMapButtons()
    {
        mapButtons = new List<Button>();
        GridLayoutGroup layout = mapGrid.GetComponent<GridLayoutGroup>();
        layout.constraintCount = mapSize;
        for (int i = 0; i < mapSize; ++i)
        {
            for (int j = 0; j < mapSize; ++j)
            {
                Button b = Instantiate(mapBTNTemplate);
                mapButtons.Add(b);
                b.transform.SetParent(mapGrid.transform);
                b.name = (i * mapSize + j).ToString();
            }
        }
        layout.transform.position = Vector3.zero;
    }

    private void CreateSelectorButtons(int cat)
    {
        selectorButtons = new List<Button>();
        string dir = "_Cat";
        if (cat >= 0) dir = sprites[cat].name;
        sprites = Resources.LoadAll<Sprite>("Sets/" + dir);
        int counter = 0;
        foreach (Sprite sprite in sprites)
        {
            //Debug.Log(sprite.name);
            Button b = Instantiate(selectorTemplate);
            selectorButtons.Add(b);
            b.image.sprite = sprite;
            b.name = (counter++).ToString();
            b.transform.SetParent(selectorGrid.transform);
        }
        selectorGrid.transform.localPosition = Vector3.zero;
    }

    private void DestroySelectorButtons()
    {
        foreach(Button child in selectorGrid.GetComponentsInChildren<Button>())
        {
            Destroy(child.gameObject);
        }
        selectorButtons.Clear();
    }

    public void SetSelectors(int caller)
    {
        //Debug.Log("NAME: " + name);
        if(caller == -1)//View directory of categories
        {
            curCategory = -1; //No category currently selected
            curSelection = -1; //No item currently selected
            DestroySelectorButtons();
            CreateSelectorButtons(curCategory);
        }
        else if(curCategory == -1) //View items in category
        {
            curCategory = caller;
            DestroySelectorButtons();
            CreateSelectorButtons(caller);
        }
        else//Select item in category
        {
            if (curSelection > -1) selectorButtons[curSelection].SendMessage("SelectionChanged", false);
            curSelection = caller;
            selectorButtons[curSelection].SendMessage("SelectionChanged", true);
        }
    }

    public void MapClicked(int caller)
    {
        //Debug.Log("Heard from " + caller);
        if(curCategory > -1 && curSelection > -1)
        {
            mapButtons[caller].SendMessage("SetSprite", sprites[curSelection]);   
        }
    }
}
