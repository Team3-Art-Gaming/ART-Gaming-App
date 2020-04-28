using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class MasterProfileScript : MonoBehaviour
{

    public Text usernameTF;
    public Button profileButton;
    public InputField charnameIN;
    public InputField raceIN;
    public InputField heightIN;
    public InputField weightIN;
    public InputField ageIN;
    public Sprite[] sprites;
    int profileSelected;

    private CreateUserFB saveProfScript;
    private Dictionary<string, string> profile;
    private string username;
    // Start is called before the first frame update
    void Start()
    {
        sprites = Resources.LoadAll<Sprite>("Characters/App_Heroes");
        this.saveProfScript = GameObject.Find("SceneManager").GetComponent<CreateUserFB>() as CreateUserFB;
        this.profile = new Dictionary<string, string>();
        this.profile = this.saveProfScript.getProfile();

        if (PlayerPrefs.HasKey("Username"))
        {
            this.username = PlayerPrefs.GetString("Username").ToString();
        }
        else
        {
            this.username = "Anonymous";
        }

        StartCoroutine(Test());
    }

    IEnumerator Test()
    {
        yield return new WaitForSeconds(1);
        foreach (var child in this.profile)
        {
            Debug.Log(child.Key + ": " + child.Value);
        }
        if (this.profile == null)
        {
            profileSelected = 0;
        }
        else
        {
            this.setProfile(this.profile["Picture"], this.profile["CharName"], this.profile["Race"], this.profile["Height"], this.profile["Weight"], this.profile["Age"]);
        }
        this.usernameTF.text = this.username;
    }

    public void onButtonClick()
    {
        profileSelected++;
        if (profileSelected > sprites.Length) profileSelected = 0;
        profileButton.image.sprite = sprites[profileSelected];
    }

    public void updateProfile()
    {
        PlayerPrefs.SetString("ProfilePic", this.profileSelected.ToString());
        PlayerPrefs.SetString("CharName", this.charnameIN.text.ToString());
        PlayerPrefs.SetString("Race", this.raceIN.text.ToString());
        PlayerPrefs.SetString("Height", this.heightIN.text.ToString());
        PlayerPrefs.SetString("Weight", this.weightIN.text.ToString());
        PlayerPrefs.SetString("Age", this.ageIN.text.ToString());
        PlayerPrefs.Save();

        this.saveProfScript.setProfile(this.profileSelected.ToString(), this.charnameIN.text.ToString(), this.raceIN.text.ToString(), this.heightIN.text.ToString(), this.weightIN.text.ToString(), this.ageIN.text.ToString());
    }

    private void setProfile(string num, string name, string race, string height, string weight, string age)
    {
        this.profileSelected = int.Parse(num);
        profileButton.image.sprite = sprites[profileSelected];
        this.charnameIN.text = name;
        this.raceIN.text = race;
        this.heightIN.text = height;
        this.weightIN.text = weight;
        this.ageIN.text = age;

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
