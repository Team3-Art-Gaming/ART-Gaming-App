using UnityEngine;

public class LE_Selector
{
    public int id { get; private set; }
    public string name { get; private set; }
    public string hex { get; private set; }
    public Sprite sprite { get; private set; } 


    public LE_Selector(int id, string name, string hex, Sprite sprite)
    {
        this.id = id;
        this.name = name;
        this.hex = hex;
        this.sprite = sprite;
    }
}
