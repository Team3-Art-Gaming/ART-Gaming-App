using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LaunchSessionNamePopUp : MonoBehaviour
{
    public InputField inputBox;
    public Text textBox;
    private GameObject caller;
    public Button ok_btn, c_btn;

    private List<string> friends;
    private GamesManager launchScript;

    public void setPrefab(GameObject caller)
    {
        this.caller = caller;

        this.friends = new List<string>();
        this.launchScript = GameObject.Find("SceneManager").GetComponent<GamesManager>() as GamesManager;
    }

    public void activatePopUp(string s)
    {
        transform.position = new Vector3(540, 960, 0);

        string friendString = "";
        foreach(string child in this.friends)
        {
            friendString += child + ", ";
        }
        this.textBox.text = s + friendString + "to this Session.";
    }

    public void deactivatePopUp()
    {
        transform.position = new Vector3(-10000, -10000, 0);
    }

    public void saveInvitedFriends(List<string> friends)
    {
        this.friends = friends;
    }

    public void okay_Click()
    {
        this.caller.SendMessage("POP_UP_RESPONSE", "OK");
    }

    public void cancel_Click()
    {
        this.caller.SendMessage("POP_UP_RESPONSE", "Cancel");
    }

    public void POP_UP_RESPONSE(string s)
    {
        switch (s)
        {
            case "OK":
                Debug.Log("OK");
                if (inputBox.text != "")
                {
                    launchScript.createInvitedSessionUsers(friends, this.inputBox.text.ToString());
                    launchScript.LaunchGame(this.inputBox.text.ToString(), PlayerPrefs.GetString("SelectedMap"), PlayerPrefs.GetString("SelectedMapData"), friends);
                    PlayerPrefs.SetString("CurrentSession", inputBox.text);
                    PlayerPrefs.Save();
                    SceneManager.LoadScene(8);
                }
                this.deactivatePopUp();
                break;
            case "Cancel":
                Debug.Log("Cancel");
                this.deactivatePopUp();
                break;
            default:
                this.deactivatePopUp();
                break;
        }
    }
}
