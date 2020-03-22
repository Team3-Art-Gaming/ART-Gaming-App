using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LE_Category
{
    public int id { get; private set; }
    public string name { get; private set; }
    public string hex { get; private set; }
    public Sprite sprite { get; private set; }

    private List<LE_Selector> selectors;

    public LE_Category(int id, string name, string hex, Sprite sprite)
    {
        this.id = id;
        this.name = name;
        this.hex = hex;
        this.sprite = sprite;
        selectors = new List<LE_Selector>();
    }

    public void AddSelector(LE_Selector s)
    {
        selectors.Add(s);
    }

    public LE_Selector GetSelector(int i)
    {
        return selectors[i];
    }
    public LE_Selector GetSelector(string hexValue)
    {
        int index = 0;
        while(index < selectors.Count)
        {
            if(selectors[index].hex == hexValue)
            {
                return selectors[index];
            }
            index++;
        }
        return null;
    }
    public int Length()
    {
        return selectors.Count;
    }
}