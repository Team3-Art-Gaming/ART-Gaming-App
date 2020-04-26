using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Maps_List : MonoBehaviour
{
    [SerializeField]
    Image MapPrefab;
    [SerializeField]
    Image Grid;

    public List<Maps> maps;
    public List<Image> entries;

    private GamesManager mapScript;

    private void Awake()
    {
    }

    void Start()
    {
        this.mapScript = GameObject.Find("SceneManager").GetComponent<GamesManager>() as GamesManager;
        this.maps = new List<Maps>();
        this.maps = mapScript.GetCreatedMapsList();

        
        /*Debug.Log("Youre in Maps_List.cs");
        foreach(Maps child in this.maps)
        {
            Debug.Log(child.mapName + ": " + child.mapString);
        }*/

        StartCoroutine(Test());
    }

    IEnumerator Test()
    {
        yield return new WaitForSeconds(1);

        this.Build_Entries();
    }

    public void Build_Entries()
    {
        Destroy_Entries();
        Image I = Instantiate(MapPrefab, Grid.transform);
        I.SendMessage("SetName", "New Map");
        I.SendMessage("SetData", "NEW");
        entries.Add(I);
        
        Debug.Log("Constructing Prefabs");
        foreach (Maps child in this.maps)
        {
            Debug.Log(child.mapName + ": " + child.mapString);
            I = Instantiate(MapPrefab, Grid.transform);
            I.SendMessage("SetMap", child);
            //I.SendMessage("SetData", child.mapString);
            this.entries.Add(I);
        }
    }

    public void Destroy_Entries()
    {
        foreach (Image entry in entries)
        {
            Destroy(entry.gameObject);
        }
        this.entries.Clear();
    }
}

