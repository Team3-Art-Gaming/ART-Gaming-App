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
    void Start()
    {
        maps = new List<Maps>();
        Build_Entries();
    }

    public void Build_Entries()
    {
        Destroy_Entries();
        Image I = Instantiate(MapPrefab, Grid.transform);
        I.SendMessage("SetName", "New Map");
        I.SendMessage("SetData", "NEW");
        entries.Add(I);
        foreach (Maps m in maps)
        {
            I = Instantiate(MapPrefab, Grid.transform);
            I.SendMessage("SetName", m.mapName);
            I.SendMessage("SetData", m.mapString);
            entries.Add(I);
        }
    }

    public void Destroy_Entries()
    {
        foreach (Image entry in entries)
        {
            Destroy(entry.gameObject);
        }
        entries.Clear();
    }
}

