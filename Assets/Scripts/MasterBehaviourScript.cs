﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Firebase.Unity.Editor;
using Firebase.Database;
using System.Threading.Tasks;
using Firebase;
using Firebase.Extensions;
using Firebase.Auth;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

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
    [SerializeField]
    private Sprite defaultSprite;
    [SerializeField]
    private InputField mapNameField;
    public InputField MapName;

    private const int mapSize = 128;
    string blankMap = "";

    private Text readout;

    string ppMapNameKey = "SelectedMap";
    string ppMapDataKey = "SelectedMapData";

    private List<Button> selectorButtons;

    private int curCategory;
    private int curSelection;

    private const int descriptorSize = 8;

    private List<LE_Category> categories;
    private List<LE_MapElements> map;

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        for(int i = 0; i < mapSize * mapSize; ++i)
        {
            blankMap += "N";
        }
        curCategory = -1;
        curSelection = -1;
        BuildSelectorStructure();
        BuildSelectors(-1);
        BuildMap();

        if (PlayerPrefs.HasKey(ppMapNameKey))
        {
            mapNameField.text = PlayerPrefs.GetString(ppMapNameKey);
            if(PlayerPrefs.HasKey(ppMapDataKey))
            { 
                if(PlayerPrefs.GetString(ppMapDataKey) == "NEW")
                {
                    PlayerPrefs.SetString(ppMapDataKey, blankMap);
                }
                BuildLevel(PlayerPrefs.GetString(ppMapDataKey));
            }
        }
        else
        {
            mapNameField.text = "Default Map";
        }
    }

    private void BuildSelectorStructure()
    {
        categories = new List<LE_Category>();
        Sprite[] packages = Resources.LoadAll<Sprite>("Sets/_Cat");
        for(int i = 0; i < packages.Length; ++i)
        {
            Sprite package = packages[i];
            LE_Category tempCat = new LE_Category(i, package.name, Convert.ToString(i, 16).PadLeft(3,'0'), package);
            Sprite[] selectors = Resources.LoadAll<Sprite>("Sets/" + package.name);
            for(int j = 0; j < selectors.Length; ++j)
            {
                Sprite selector = selectors[j];
                LE_Selector tempSel = new LE_Selector(j, selector.name, Convert.ToString(j, 16).PadLeft(3,'0'), selector);
                tempCat.AddSelector(tempSel);
            }
            categories.Add(tempCat);
        }
    }

    private void BuildSelectors(int cat)
    {
        selectorButtons = new List<Button>();
        if (cat == -1)
        {
            for (int i = 0; i < categories.Count; ++i)
            {
                Button b = Instantiate(selectorTemplate);
                selectorButtons.Add(b);
                b.image.sprite = categories[i].sprite;
                b.name = i.ToString();
                b.transform.SetParent(selectorGrid.transform);
            }
        }
        else
        {
            for (int i = 0; i < categories[cat].Length(); ++i)
            {
                Button b = Instantiate(selectorTemplate);
                selectorButtons.Add(b);
                b.image.sprite = categories[cat].GetSelector(i).sprite;
                b.name = i.ToString();
                b.transform.SetParent(selectorGrid.transform);
            }
        }
    }

    private void DestroySelectorButtons()
    {
        foreach (Button child in selectorGrid.GetComponentsInChildren<Button>())
        {
            Destroy(child.gameObject);
        }
        selectorButtons.Clear();
    }

    private void BuildMap()
    {
        map = new List<LE_MapElements>();
        GridLayoutGroup layout = mapGrid.GetComponent<GridLayoutGroup>();
        layout.constraintCount = mapSize;
        for (int y = 0; y < mapSize; ++y)
        {
            for(int x = 0; x < mapSize; ++x)
            {
                Button b = Instantiate(mapBTNTemplate);
                b.name = (y * mapSize + x).ToString();
                b.transform.SetParent(mapGrid.transform);
                LE_MapElements lem = new LE_MapElements(y * mapSize + x, b);
                map.Add(lem);
            }
        }
        layout.transform.position = Vector3.zero;
        LE_MapElements.defaultSprite = defaultSprite;
    }

    public void SetSelectors(int caller)
    { 
        if (caller == -1)//View directory of categories
        {
            curCategory = -1; //No category currently selected
            curSelection = -1; //No item currently selected
            DestroySelectorButtons();
            BuildSelectors(curCategory);
        }
        else if(caller == -2)
        {
            if (curSelection > -1) selectorButtons[curSelection].SendMessage("SelectionChanged", false);
            curSelection = caller;
        }
        else if (curCategory == -1) //View items in category
        {
            curCategory = caller;
            DestroySelectorButtons();
            BuildSelectors(caller);
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
        if(curCategory > -1 && curSelection > -1)
        {
            Sprite s = categories[curCategory].GetSelector(curSelection).sprite;
            map[caller].UpdateSprite(categories[curCategory].hex, categories[curCategory].GetSelector(curSelection).hex, s);
        }
        else if(curSelection == -2)
        {
            map[caller].BlankSprite();
        }
    }

    public void SaveClicked()
    {
        string mapString = "";
        foreach (LE_MapElements lem in map)
        {
            if (lem.catHex == "N")
            {
                mapString += "N";
            }
            else
            {
                mapString += lem.catHex + lem.itmHex + lem.spriteRot + "0";
            }
        }
        string Map = MapName.text;
        PlayerPrefs.SetString(Map, mapString);

        PlayerPrefs.Save();
        PushMap(mapString, Map);

    }

    public void PushMap(string map, string MapName)
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://art-152.firebaseio.com/");
        DatabaseReference DBreference = FirebaseDatabase.DefaultInstance.RootReference;

        string name = PlayerPrefs.GetString("Username");
        string path = "/users/";
        path = string.Concat(path, name);
        path = string.Concat(path, "/CreatedMaps/");
        FirebaseDatabase.DefaultInstance.GetReference(path).Child(MapName).SetValueAsync(map);

        return;
    }

    public void LoadClicked()
    {
        //TODO: SET TO OPEN MAP SELECT SCENE

        /*
        if(PlayerPrefs.HasKey("TempLevel"))
        {
            string level = PlayerPrefs.GetString("TempLevel");
            Debug.Log("Loaded: " + level);
            BuildLevel(level);
        }
        */
    }


    private void BuildLevel(string level)
    {
        int stringIndex = 0;
        for(int i = 0; i < mapSize*mapSize; ++i)
        {
            char c = level[stringIndex];
            if(c == 'N')
            {
                stringIndex++;
                map[i].BlankSprite();
            }
            else
            {
                string piece = level.Substring(stringIndex,descriptorSize);
                
                string catHex = piece.Substring(0,3);
                string selHex = piece.Substring(3,3);
                int rot = int.Parse(piece.Substring(6,1));
                int index = -1;
                int count = 0;
                foreach(LE_Category cat in categories)
                {
                    if(cat.hex == catHex) index = count;
                    count++;
                }
                if(index > -1)
                {
                    Sprite s = categories[index].GetSelector(selHex).sprite;
                    map[i].UpdateSprite(categories[index].hex, categories[index].GetSelector(selHex).hex, s);
                    map[i].RotateSprite(rot);
                }
                stringIndex += descriptorSize; 
            }
        }
    }
}
