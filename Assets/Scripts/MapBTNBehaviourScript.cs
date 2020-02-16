using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapBTNBehaviourScript : MonoBehaviour
{

    private int myNumber;
    private MasterBehaviourScript master;
    private Image image;

    [SerializeField]
    private Sprite defaultSprite;

    private string curCategory;
    private string curSelection;
    private int rot;

    // Start is called before the first frame update
    void Start()
    {
        curCategory ="";
        curSelection = "";
        rot = 0;
        master = GetComponentInParent<MasterBehaviourScript>() as MasterBehaviourScript;
        image = GetComponent<Image>() as Image;
        image.sprite = defaultSprite;
    }

    public void OnClick()
    {
        master.MapClicked(name);
    }

    public void SetActive(string[] param)
    {
        if (param[0] == name) //Message is meant for this btn
        {
            if (param[1] == curCategory && param[2] == curSelection)//The sprite is already present on this square so rotate or delete
            {
                rot += 90;
                image.transform.rotation = Quaternion.Euler(0, 0, rot);
                if (rot == 360)
                {
                    image.sprite = defaultSprite;
                    curCategory = "";
                    curSelection = "";
                    rot = 0;
                }

            }
            else
            {
                string path = "Sets/" + param[1] + "/" + param[2];
                Sprite sprite = Resources.Load<Sprite>(path);
                image.sprite = sprite;
                curCategory = param[1];
                curSelection = param[2];
                rot = 0;
            }
        }
    }
    // Update is called once per frame
    /*
    void Update()
    {
        
    }
    */
}
