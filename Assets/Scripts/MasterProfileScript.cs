using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MasterProfileScript : MonoBehaviour
{
    [SerializeField]
    Text usernameTF;
    [SerializeField]
    Button profileButton;
    [SerializeField]
    Input charnameIN;
    [SerializeField]
    Input raceIN;
    [SerializeField]
    Input heightIN;
    [SerializeField]
    Input weightIN;
    [SerializeField]
    Input ageIN;
    Sprite[] sprites;
    int profileSelected;
    // Start is called before the first frame update
    void Start()
    {
        sprites = Resources.LoadAll<Sprite>("Characters/App_Heroes");
        profileSelected = 0; //GET PREVIOUS PROFILE SELECTION FROM SYSTEM
        //usernameTF.text = GET USERNAME FROM SYSTEM;

    }

    public void onButtonClick()
    {
        profileSelected++;
        if (profileSelected > sprites.Length) profileSelected = 0;
        profileButton.image.sprite = sprites[profileSelected];
    }

    //FUNCTION TO RETRIEVE INFO FROM DB
    /*
    {
        usernameTF.text = "";
        profileSelected = 0;
        profileButton.image.sprite = sprites[profileSelected];
        if(charnameExists) charnameIN.text = "";
        if(raceExists) raceIN.text = "";
        if(heightExists) heightIN.text = "";
        if(weightExists) weightIN.text = "";
        if(ageExists) ageIN.text = "";
    }
    */
}
