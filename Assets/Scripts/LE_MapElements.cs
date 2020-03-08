using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LE_MapElements
{
    public static Sprite defaultSprite { get; set; } 
    public int id { get; private set; }
    public string catHex { get; private set; }
    public string itmHex { get; private set; }
    public Button btn { get; private set; }
    public Sprite sprite { get; private set; }
    public int spriteRot { get; private set; }
    public LE_MapElements(int id, Button btn)
    {
        this.id = id;
        this.btn = btn;
        this.btn.name = id.ToString();
        catHex = "N";
        itmHex = "N";
        sprite = null;
        spriteRot = 0;
    }

    public void UpdateSprite(string catHex, string itmHex, Sprite sprite)
    {
        if (catHex == this.catHex && itmHex == this.itmHex)
        {
            RotateSprite();
        }
        else
        {
            this.catHex = catHex;
            this.itmHex = itmHex;
            this.sprite = sprite;
            btn.image.sprite = this.sprite;
            spriteRot = 0;
            btn.image.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void BlankSprite()
    {
        spriteRot = 0;
        sprite = null;
        btn.image.sprite = defaultSprite;
        catHex = "N";
        itmHex = "N";
    }

    private void RotateSprite()
    {
        spriteRot++;
        if (spriteRot > 3) spriteRot -= 4;
        btn.image.transform.rotation = Quaternion.Euler(0, 0, spriteRot * 90);
    }

    public void RotateSprite(int rot)
    {
        spriteRot = rot;
        btn.image.transform.rotation = Quaternion.Euler(0, 0, spriteRot * 90);
    }
}
