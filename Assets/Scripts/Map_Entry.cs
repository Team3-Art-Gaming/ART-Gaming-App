using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Map_Entry : MonoBehaviour
{
    [SerializeField]
    Text mapNameTextField;
    [SerializeField]
    Button Edit_Button;
    [SerializeField]
    Button Play_Button;

    string mapName = "NEW";
    string mapData = "NEW";

    string ppMapNameKey = "SelectedMap";
    string ppMapDataKey = "SelectedMapData";

    public void SetName(string n)
    {
        mapName = n;
        mapNameTextField.text = n;
    }

    public string GetName()
    {
        return mapName;
    }

    public void SetData(string d)
    {
        mapData = d;
    }

    public string GetData()
    {
        return mapData;
    }

    public void ClickedEdit()
    {
        PlayerPrefs.SetString(ppMapNameKey, mapName);
        PlayerPrefs.SetString(ppMapDataKey, mapData);
        PlayerPrefs.Save();
        SceneManager.LoadScene(7);
    }

    public void ClickedPlay()
    {
        PlayerPrefs.SetString(ppMapNameKey, mapName);
        PlayerPrefs.SetString(ppMapDataKey, mapData);
        PlayerPrefs.Save();
        SceneManager.LoadScene(6);
    }
}
