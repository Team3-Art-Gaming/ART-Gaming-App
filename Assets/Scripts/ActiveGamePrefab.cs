using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ActiveGamePrefab : MonoBehaviour
{
    [SerializeField]
    Text SessionName;
    [SerializeField]
    Text HostName;
    [SerializeField]
    Button joinActiveGameButton;

    private string gameName;
    private string hostName;
    private GameObject popUp;

    // Start is called before the first frame update
    public void setSessionName(string name)
    {
        this.gameName = name;
        this.SessionName.text = name;
    }

    public void setHostName(string name)
    {
        this.hostName = name;
        this.HostName.text = name;
    }

    public void getArenaPrefab()
    {
        this.popUp = GameObject.Find("LIVE_Arena(Clone)");
        this.popUp.SendMessage("activateLiveArena");
        PlayerPrefs.SetString("CurrentSession", this.SessionName.text);
        PlayerPrefs.Save();
    }
}
