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

    private string mapName = "NEW";
    private string mapData = "NEW";

    private string ppMapNameKey = "SelectedMap";
    private string ppMapDataKey = "SelectedMapData";

    public void SetMap(Maps m)
    {
        this.mapName = m.mapName;
        this.mapNameTextField.text = this.mapName;
        this.mapData = m.mapString;
    }

    public void SetName(string n)
    {
        mapName = n;
        mapNameTextField.text = n;
    }

    public string GetName()
    {
        return this.mapName;
    }

    public void SetData(string d)
    {
        mapData = d;
    }

    public string GetData()
    {
        return this.mapData;
    }

    public void ClickedEdit()
    {
        PlayerPrefs.SetString(ppMapNameKey, this.mapName);
        PlayerPrefs.SetString(ppMapDataKey, this.mapData);
        PlayerPrefs.Save();
        SceneManager.LoadScene(7);
    }

    public void ClickedPlay()
    {
        PlayerPrefs.SetString(ppMapNameKey, this.mapName);
        PlayerPrefs.SetString(ppMapDataKey, this.mapData);
        PlayerPrefs.Save();
        SceneManager.LoadScene(6);
    }
}
