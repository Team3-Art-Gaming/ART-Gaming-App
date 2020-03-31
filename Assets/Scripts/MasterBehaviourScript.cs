using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

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

    private const int mapSize = 128;

    private Text readout;


    //private List<Button> mapButtons;
    private List<Button> selectorButtons;

    private int curCategory;
    private int curSelection;

    private const int descriptorSize = 8;

    private List<LE_Category> categories;
    private List<LE_MapElements> map;

    private void Awake()
    { 
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        if (PlayerPrefs.HasKey("MapName"))
        {
            mapNameField.text = PlayerPrefs.GetString("MapName");
        }
        else
        {
            mapNameField.text = "Default Map";
        }
        curCategory = -1;
        curSelection = -1;
        BuildSelectorStructure();
        BuildSelectors(-1);
        BuildMap();
    }

    private void BuildSelectorStructure()
    {
        categories = new List<LE_Category>();
        Sprite[] packages = Resources.LoadAll<Sprite>("Sets/_Cat");
        Debug.Log("Found " + packages.Length + " packages");
        for(int i = 0; i < packages.Length; ++i)
        {
            Sprite package = packages[i];
            Debug.Log(package.name);
            LE_Category tempCat = new LE_Category(i, package.name, Convert.ToString(i, 16).PadLeft(3,'0'), package);
            Sprite[] selectors = Resources.LoadAll<Sprite>("Sets/" + package.name);
            Debug.Log("Found " + selectors.Length + " selectors");
            for(int j = 0; j < selectors.Length; ++j)
            {
                Sprite selector = selectors[j];
                LE_Selector tempSel = new LE_Selector(j, selector.name, Convert.ToString(j, 16).PadLeft(3,'0'), selector);
                tempCat.AddSelector(tempSel);
            }
            categories.Add(tempCat);
        }
    }

    /*
    private void BuildSelectorStructure()
    {
        categories = new List<LE_Category>();
        string pathToSets = Application.dataPath + "/Resources/Sets";
        DirectoryInfo dir = new DirectoryInfo(pathToSets);
        if (dir.Exists) readout.text = "YAY";
        else readout.text = "BOOO";
        DirectoryInfo[] dirs = dir.GetDirectories();
        int catCount = 0;
        foreach (DirectoryInfo di in dirs)
        {
            string category = di.Name;
            Debug.Log("Checking: " + pathToSets + "/" + category);
            Sprite[] sprites = Resources.LoadAll<Sprite>("Sets/" + category);
            int catID = catCount++;
            string catName = category;
            string catHex = sprites[0].name.Substring(1);
            LE_Category lec = new LE_Category(catID, catName, catHex, sprites[0]);
            for (int i = 1; i < sprites.Length; ++i)
            {
                LE_Selector les = new LE_Selector(i, sprites[i].name, Convert.ToString(i, 16).PadLeft(3,'0'), sprites[i]);
                Debug.Log(les.hex);
                lec.AddSelector(les);
            }
            categories.Add(lec);
            Debug.Log("Check: " + lec.name);
        }
    }
    */
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
        foreach(LE_MapElements lem in map)
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
        //Text t = GetComponentInChildren<Text>();
        //readout.text = mapString;
        PlayerPrefs.SetString("TempLevel", mapString);
        string mapName = mapNameField.text;
        PlayerPrefs.SetString("MapName", mapName);
        PlayerPrefs.Save();
        Debug.Log("Saved map " + mapName);
        Debug.Log(mapString);
    }

    public void LoadClicked()
    {
        if(PlayerPrefs.HasKey("TempLevel"))
        {
            string level = PlayerPrefs.GetString("TempLevel");
            Debug.Log("Loaded: " + level);
            BuildLevel(level);
        }
    }

    private void BuildLevel(string level)
    {
        int stringIndex = 0;
        for(int i = 0; i < mapSize*mapSize; ++i)
        {
            char c = level[stringIndex];
            if(c == 'N')
            {
                //Debug.Log("N");
                stringIndex++;
                map[i].BlankSprite();
            }
            else
            {
                string piece = level.Substring(stringIndex,descriptorSize);
                Debug.Log(piece);
                
                string catHex = piece.Substring(0,3);
                string selHex = piece.Substring(3,3);
                int rot = int.Parse(piece.Substring(6,1));
                Debug.Log(selHex);
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
                    Debug.Log("Rotating " + rot); 
                }
                stringIndex += descriptorSize; 
            }
        }
    }
}
